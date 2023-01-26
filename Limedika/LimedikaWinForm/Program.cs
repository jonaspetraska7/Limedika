using Common.Data;
using Common.Entities;
using Common.Interfaces;
using Common.Services;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LimedikaWinForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            ConfigureServices(services);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dataConnection = scope.ServiceProvider.GetService<LimedikaDataConnection>();
                    var sp = dataConnection.DataProvider.GetSchemaProvider();
                    var dbSchema = sp.GetSchema(dataConnection);
                    if (!dbSchema.Tables.Any(t => t.TableName == nameof(Client)))
                    {
                        dataConnection.CreateTable<Client>();
                    }
                    if (!dbSchema.Tables.Any(t => t.TableName == nameof(Log)))
                    {
                        dataConnection.CreateTable<Log>();
                    }
                }

                var form1 = serviceProvider.GetRequiredService<Form1>();
                Application.Run(form1);
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["LimedikaDb"] ?? "";

            services.AddLinqToDBContext<LimedikaDataConnection>((provider, options) => {
                options
                .UseSqlServer(connectionString)
                .UseDefaultLogging(provider);
            });

            services.AddHttpClient<IPostCodeService, PostItService>(httpClient =>
            {
                var baseUrl = System.Configuration.ConfigurationManager.AppSettings["PostItUrl"] ?? "";
                var key = System.Configuration.ConfigurationManager.AppSettings["PostItKey"] ?? "";
                httpClient.BaseAddress = new Uri($"{baseUrl}?key={key}&term=");
            });

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IClientPageService, ClientPageService>();
            services.AddTransient<IBufferedFileUploadService, BufferedFileUploadService>();
            services.AddScoped<Form1>();
        }
    }
}