using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1333");

ISubscriber subscriber = connection.GetSubscriber();

#region Publisher

while (true)
{
    Console.Write("Mesaj: ");
    string message = Console.ReadLine()!;

    //await subscriber.PublishAsync(channel: "mychannel", message);

    // Channel Pattern
    await subscriber.PublishAsync(channel: "tech.*", message);
}

#endregion

#region Subscriber

await subscriber.SubscribeAsync(channel: "tech.*", (channel, value) =>
{
    Console.WriteLine($"{channel}: {value}");
});

Console.Read();

#endregion
