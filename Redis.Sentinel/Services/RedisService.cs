using StackExchange.Redis;

namespace Redis.Sentinel.Services
{
    public class RedisService
    {
        static ConfigurationOptions sentinelOptions => new()
        {
            // Sentinel sunucularımız
            EndPoints =
            {
                {"localhost", 6383 },
                {"localhost", 6384 },
                {"localhost", 6385 },
            },

            CommandMap = CommandMap.Sentinel,
            AbortOnConnectFail = false,
        };

        static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false,
        };

        public static async Task<IDatabase> GetRedisMasterDatabase()
        {
            ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);

            System.Net.EndPoint masterEndPoint = null;

            foreach (System.Net.EndPoint endPoint in sentinelConnection.GetEndPoints())
            {
                IServer server = sentinelConnection.GetServer(endPoint);
                if (!server.IsConnected)
                    continue;

                // mymaster -> sentinel.conf dosyası içerisinde belirtilen master ismi
                masterEndPoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");
                break;
            }

            // Bize docker'daki internal IP'sini dönüyor. Fakat bize şu anda localhost gerekiyor.
            var localMasterIP = masterEndPoint.ToString() switch
            {
                "172.18.0.2:6379" => "localhost:1333",
                "172.18.0.3:6379" => "localhost:1334",
                "172.18.0.4:6379" => "localhost:1335",
                "172.18.0.5:6379" => "localhost:1336",
            };

            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);

            IDatabase database = masterConnection.GetDatabase();
            return database;
        }
    }
}
