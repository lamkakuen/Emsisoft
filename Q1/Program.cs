using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Q1.Data;
using Q1.Repositories;
using Q1.Services;

namespace Q1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        var configuration = hostContext.Configuration;

                        services.AddDbContext<DataContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                        services.AddScoped<HashRepository>();
                        services.AddScoped<HashService>();

                        services.AddSingleton<IConnection>(sp =>
                        {
                            var factory = new ConnectionFactory
                            {
                                HostName = "localhost",
                                UserName = "guest",
                                Password = "guest"
                            };
                            return factory.CreateConnection();
                        });

                        services.AddSingleton<IModel>(sp =>
                        {
                            var connection = sp.GetRequiredService<IConnection>();
                            var channel = connection.CreateModel();
                            channel.QueueDeclare(queue: "hashes", durable: false, exclusive: false, autoDelete: false, arguments: null);
                            return channel;
                        });

                        services.AddControllers(); // Add this line to register the controllers
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}
