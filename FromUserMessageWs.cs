namespace BeaverServerReborn
{
    public class FromUserMessageWs
    {
        public string Action { get; set; }
        public FromUserMessageWs(string action)
        {
            Action = action;
        }
    }
}
