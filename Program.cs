using Microsoft.EntityFrameworkCore;
namespace BeaverServerReborn
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors();
            // Add services to the container.

            builder.Services.AddControllers();
			builder.Services.AddControllersWithViews();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			

			// получаем строку подключения из файла конфигурации
			string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			// добавляем контекст ApplicationContext в качестве сервиса в приложение
			builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));


	/*		builder.Services.AddScoped<ILeaderBoardService, LeaderBoardService>();
			builder.Services.AddSingleton<LeaderBoardUpdateService>();*/

			builder.Services.AddAuthentication("Cookies").AddCookie(options =>
			{
				options.LoginPath = "/Login"; // путь к странице логина
			});
			builder.Services.AddAuthorization();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.UseAuthentication();
			app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod());

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };
            app.UseWebSockets(webSocketOptions);

			app.MapControllers();

			app.Run();
		}
	}
}
