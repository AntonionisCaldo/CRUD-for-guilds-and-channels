using Refit;

namespace discord_API_app;

public interface IDiscordApi
{
    // CRUD операции для серверов (Guilds)
    [Get("/users/@me/guilds")]
    Task<List<Guild>> GetGuilds();

    [Get("/guilds/{guildId}")]
    Task<Guild> GetGuild(string guildId);

    [Post("/guilds")]
    Task<Guild> CreateGuild([Body] GuildCreateRequest request);

    [Put("/guilds/{guildId}")]
    Task<Guild> UpdateGuild(string guildId, [Body] GuildUpdateRequest request);

    [Delete("/guilds/{guildId}")]
    Task DeleteGuild(string guildId);

    // CRUD операции для каналов (Channels)
    [Get("/guilds/{guildId}/channels")]
    Task<List<Channel>> GetChannels(string guildId);

    [Get("/channels/{channelId}")]
    Task<Channel> GetChannel(string channelId);

    [Post("/guilds/{guildId}/channels")]
    Task<Channel> CreateChannel(string guildId, [Body] ChannelCreateRequest request);

    [Put("/channels/{channelId}")]
    Task<Channel> UpdateChannel(string channelId, [Body] ChannelUpdateRequest request);

    [Delete("/channels/{channelId}")]
    Task DeleteChannel(string channelId);
}
public class Guild
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public Guild()
    {
        Id = string.Empty;
        Name = string.Empty;
        Country = string.Empty;
    }
}
public class GuildCreateRequest
{
    public string Name { get; set; }
    public string Country { get; set; }

    public GuildCreateRequest()
    {
        Name = string.Empty;
        Country = string.Empty;
    }
}
public class GuildUpdateRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public GuildUpdateRequest()
    {
        Name = string.Empty;
        Country = string.Empty;
    }
}
public class Channel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Channel()
    {
        Id = string.Empty;
        Name = string.Empty;
    }
}
public enum ChannelType
{
    General = 0,
    Text = 1,
    Voice = 2,
    Private = 3,
    Arhive = 4
}
public class ChannelCreateRequest
{
    public string Name { get; set; }
    public string Topic { get; set; }
    public ChannelType Type { get; set; }
    public ChannelCreateRequest()
    {
        Name = string.Empty;
        Topic = string.Empty;
        Type = ChannelType.General;
    }
    public ChannelCreateRequest(string name, string topic, int typeNumber)
    {
        Name = name;
        Topic = topic;
        
    }
}


public class ChannelUpdateRequest
{
    public string Name { get; set; }

    public ChannelUpdateRequest()
    {
        Name = string.Empty;
    }
}

public class Client
{
    public string Nick_Name { get; set; }
    public string First_Name { get; private set; }
    public string LastName { get; private set; }
    private DateTime BrithDay { get; set; }
    public int Age => DateTime.Now.Year - BrithDay.Year;
    public string E_Mail { get; private set; }

    public Client()
    {
        
    }
    
}
class Program
{
    static async Task Main(string[] args)
    {
        await CreateGuildAndChannel();
    }

    static async Task CreateGuildAndChannel()
    {
        // Создание экземпляра API для работы с Discord API
        var api = RestService.For<IDiscordApi>("https://api.discord.com");

        // Создание нового сервера (Guild)
        var newGuild = new Guild();
        newGuild.Name = "Новый сервер";
        newGuild.Country = "Россия";

        // Преобразование объекта Guild в объект GuildCreateRequest
        var guildCreateRequest = new GuildCreateRequest();
        guildCreateRequest.Name = newGuild.Name;
        guildCreateRequest.Country = newGuild.Country;

        // Вызов метода API для создания сервера
        var createdGuild = await api.CreateGuild(guildCreateRequest);

        // Проверка на успешное создание сервера
        if (createdGuild != null)
        {
            Console.WriteLine($"Создан сервер с идентификатором: {createdGuild.Id}");

            // Создание объекта для создания нового канала (Channel)
            var newChannelRequest = new ChannelCreateRequest();
            newChannelRequest.Name = "Новый канал";
            newChannelRequest.Topic = "Тема канала";
            newChannelRequest.Type = 0; // Тип канала (например, текстовый)

            // Вызов метода API для создания канала на созданном сервере
            var createdChannel = await api.CreateChannel(createdGuild.Id, newChannelRequest);

            // Проверка на успешное создание канала
            if (createdChannel != null)
            {
                Console.WriteLine($"Создан канал с идентификатором: {createdChannel.Id}");
            }
            else
            {
                Console.WriteLine("Ошибка при создании канала.");
            }
        }
        else
        {
            Console.WriteLine("Ошибка при создании сервера.");
        }
    }
}