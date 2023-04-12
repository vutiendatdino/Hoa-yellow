using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

namespace SP23_G21_SHSMS.Services.Common_Services
{
    public class ConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        /*public void AddNewConnectionString(string connectionStringName, string connectionString)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Build the configuration from the appsettings.json file
            var configuration = configurationBuilder.Build();

            // Add the new connection string to the configuration
            configuration.GetSection("ConnectionStrings").GetSection(connectionStringName).Value = connectionString;

            // Save the updated configuration back to the appsettings.json file
            var json = JsonConvert.SerializeObject(configuration.GetSection("ConnectionStrings"), Formatting.Indented);
            var appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            System.IO.File.WriteAllText(appSettingsFilePath, json);
        }*/
        public void AddNewConnectionString(string name, string connectionString)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            IConfigurationRoot config = builder.Build();

            var connectionStrings = _configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
            connectionStrings.Add(name, connectionString);

            //get all appsetting 
            var appStt = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appsettings.json"));

            //convert cons dictionary to jobject
            var jObjCons = JObject.Parse(System.Text.Json.JsonSerializer.Serialize(connectionStrings, new JsonSerializerOptions { WriteIndented = true }));

            appStt["ConnectionStrings"] = jObjCons;

            File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(appStt, Formatting.Indented));

            config.Reload();
        }
    }
}
