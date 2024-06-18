using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;

namespace BeaverServerReborn.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/ws")]
    public class WsTestController : Controller
    {
        private readonly ILogger<WsTestController> _logger;
        private readonly ApplicationContext _context;
        private static Timer _saveTimer;
        private static List<Upgrade> _upgrades;
        private Dictionary<string, int> _userData = new Dictionary<string, int>();


        public WsTestController(ILogger<WsTestController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
            _upgrades = _context.Upgrades.ToList();

            if (_saveTimer == null)
            {
                _saveTimer = new Timer(SaveUserData, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            }

        }

        [HttpGet]
        public async Task Get()
        {
            _logger.LogInformation("Попытка подключения по вебсокет");

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.LogInformation("Подключение по вебсокет успешно");
                await Echo(HttpContext, webSocket, _logger, _context);
            }
            else
            {
                _logger.LogError("Тип подключения не является вебсокетом");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }


        private static async Task Echo(HttpContext httpcontext, WebSocket webSocket, ILogger _logger, ApplicationContext _context)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            #region setinfo
            /*можно вынести в сервис в который передвать контекст*/
            string username = httpcontext.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            int balance = user.Balance;

            Dictionary<string, int> userUpgradesDict = new Dictionary<string, int>();
            List<PlayerUpgrade> userUpgradesList = _context.PlayerUpgrades.Where(u => u.Username == username).ToList();

            foreach (var upgrade in userUpgradesList)
            {
                // заполнить словарь
                userUpgradesDict[upgrade.UpgradeName] = upgrade.Count;
            }

            /*можно вынести в сервис в который передвать контекст*/
            int userIncome = UserIncomeCalc(userUpgradesDict);

            TimeSpan totalDifference = DateTime.Now - user.LastEnteredDate;
            int userOfflineTime;

            if (totalDifference.TotalHours > 3)
            {   
                userOfflineTime = 3 * 60 * 60; // 3 часа в секундах
            }
            else
            {
                userOfflineTime = (int)totalDifference.TotalSeconds;
            }

            balance += userOfflineTime * userIncome;



            ToUserMessageWs messageObject = new ToUserMessageWs(balance, userUpgradesDict, _upgrades);
            string messageString = JsonConvert.SerializeObject(messageObject);
            _logger.LogInformation($"Server message string: {messageString}");
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(messageString);
            _logger.LogInformation($"Server message bytes long: {messageBytes.Length}");
            await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length),
                        WebSocketMessageType.Text, true, CancellationToken.None);


            Timer balanceTimer = new Timer(async _ =>
            {
                balance += userIncome;
                if (webSocket.State == WebSocketState.Open)
                {
                    ToUserMessageWs messageObject = new ToUserMessageWs(balance);
                    string messageString = JsonConvert.SerializeObject(messageObject);
                    byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(messageString);
                    await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length),
                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            #endregion


            while (!receiveResult.CloseStatus.HasValue)
            {
                string message;
                var receivedMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                FromUserMessageWs fromUserMessageWs = JsonConvert.DeserializeObject<FromUserMessageWs>(receivedMessage);
                switch (fromUserMessageWs.Action)
                {
                    case "ClickedBeaverButton":
                        balance += 1;
                        /*паста с таймера*/
                        messageObject = new ToUserMessageWs(balance);
                        messageString = JsonConvert.SerializeObject(messageObject);
                        messageBytes = System.Text.Encoding.UTF8.GetBytes(messageString);
                        await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        break;
                    default:
                        string upgradeName = fromUserMessageWs.Action;
                        int price = -1;
                        if (!userUpgradesDict.ContainsKey(upgradeName)) userUpgradesDict[upgradeName] = 0;
                        foreach (var upgrade in _upgrades)
                        {
                            if (upgrade.Name == upgradeName)
                            {
                                price = (int)Math.Ceiling(
                                    upgrade.Price * Math.Pow(1.15, (double)userUpgradesDict[upgradeName]));
                            }
                        }
                        if (price == -1) break;


                        if (balance < price) break;


                        balance -= price;
                        
                        userUpgradesDict[upgradeName]++;
                        userIncome = UserIncomeCalc(userUpgradesDict);
                        _logger.LogInformation($"User {username} bought: {upgradeName}");
                        /*копипаст*/
                        messageObject = new ToUserMessageWs(balance, userUpgradesDict);
                        messageString = JsonConvert.SerializeObject(messageObject);
                        messageBytes = System.Text.Encoding.UTF8.GetBytes(messageString);
                        _logger.LogInformation($"Server message bytes long: {messageBytes.Length}");
                        await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length),
                                    WebSocketMessageType.Text, true, CancellationToken.None);
                        /*копипаст*/
                        break;
                }

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);


            if (user != null)
            {
                user.LastEnteredDate = DateTime.UtcNow;
                user.Balance = balance;

                foreach (var upgrade in userUpgradesDict)
                {
                    string upgradeName = upgrade.Key;
                    int count = upgrade.Value;

                    var playerUpgrade = _context.PlayerUpgrades
                        .FirstOrDefault(uu => uu.Username == username && uu.UpgradeName == upgradeName);

                    if (playerUpgrade != null)
                    {
                        playerUpgrade.Count = count;
                    }
                    else
                    {
                        // Если записи нет, добавляем новую запись.
                        playerUpgrade = new PlayerUpgrade(username, upgradeName, count);
                        _context.PlayerUpgrades.Add(playerUpgrade);

                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Соединение по вебсокет разорвано");
            }
        }

        private void SaveUserData(object state)
        {

        }

        private static int UserIncomeCalc(Dictionary<string, int> userUpgradesDict)
        {
            int userIncome = 0;
            foreach (KeyValuePair<string, int> keyValuePair in userUpgradesDict)
            {
                foreach (var infUpgrade in _upgrades)
                {
                    if (keyValuePair.Key == infUpgrade.Name)
                    {
                        userIncome += keyValuePair.Value * infUpgrade.Income;
                    }
                }
            }
            return userIncome;
        }
    }
}

