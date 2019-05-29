using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelBot
{
    class Program
    {
        private const string _helpText = "Available commands:\nsendAll {message} (send a message to all chats)\nsend {chatId} {message} (send message to specific chat)\nshowMessages {chatId} (show all messages from chat)\nshowAllChatIds\nquit\nshowCommands";

        public static async Task Main()
        {
            TelegramBotClient botClient = new TelegramBotClient(BotSettings.ApiToken);
            var bot = new BotWrapper(botClient, @"C:\My Files\chatIds.txt");

            Console.WriteLine(_helpText);
            bool cont = true;
            do
            {
                Console.WriteLine("Please Enter command");
                string command = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }
                (BotCommand comm, string arg) = ParseCommand(command);

                switch (comm)
                {
                    case BotCommand.send:
                        SendMessage(arg, bot);
                        break;
                    case BotCommand.sendAll:
                        bot.SendMessageToAll(arg);
                        Console.WriteLine("Sent!");
                        break;
                    case BotCommand.quit:
                        cont = false;
                        break;
                    case BotCommand.showMessages:
                        ShowMessages(arg, bot);
                        break;
                    case BotCommand.showCommands:
                        Console.WriteLine(_helpText);
                        break;
                    case BotCommand.showAllChatIds:
                        ShowAllChatIds(bot);
                        break;
                    case BotCommand.none:
                        Console.WriteLine($"Command {command} not found");
                        break;
                };
            }
            while (cont);
            Console.ReadLine();
        }

        private static void ShowAllChatIds(BotWrapper bot)
        {
            var chatIds = bot.GetChatIds();
            foreach (var id in chatIds)
                Console.WriteLine(id);
        }

        private static void SendMessage(string arg, BotWrapper bot)
        {
            (long chatId, string msg) = ParseArgForSend(arg);
            if (chatId != -1)
            {
                bot.SendMessage(chatId, msg);
                Console.WriteLine("Sent!");
            }
            else
            {
                Console.WriteLine("Incorrect Arguments");
            }
        }

        private static void ShowMessages(string arg, BotWrapper bot)
        {
            if (long.TryParse(arg.Trim(), out var id))
            {
                var msgs = bot.GetMessages(id).Select(m => new
                {
                    m.Text,
                    m.Date,
                    m.From.Username
                });
                foreach (var msg in msgs)
                {
                    Console.WriteLine($"-------\nDate: {msg.Date} From: {msg.Username} \n {msg.Text}");
                }
            }
            else
            {
                Console.WriteLine("ChatId is not provided");
            }
        }

        private static (long chatId, string msg) ParseArgForSend(string arg)
        {
            arg = arg.Trim();
            if (long.TryParse(string.Join("", arg.TakeWhile(s => s != ' ')).Trim(), out var id))
            {
                return (id, string.Join("", arg.SkipWhile(s => s != ' ')).Trim());
            }
            else
            {
                return (-1, null);
            }
        }

        private static (BotCommand comm, string arg) ParseCommand(string command)
        {
            command = command.Trim();
            string commStr = string.Join("", command.TakeWhile(s => s != ' '));
            if (Enum.TryParse<BotCommand>(commStr, out var commEnum))
            {
                return (commEnum, string.Join("", command.SkipWhile(s => s != ' ')));
            }
            else
            {
                return (BotCommand.none, null);
            }
        }
    }

    enum BotCommand
    {
        send,
        sendAll,
        quit,
        showMessages,
        showCommands,
        showAllChatIds,
        none
    }

    public class BotWrapper
    {
        private TelegramBotClient _botClient;
        private readonly string _fileWithChatIds;
        private readonly Dictionary<long, ICollection<Message>> _messages;

        public BotWrapper(TelegramBotClient client, string pathToFileWithChatIds)
        {
            _botClient = client ?? throw new ArgumentNullException(nameof(client));
            _fileWithChatIds = pathToFileWithChatIds ?? throw new ArgumentNullException(nameof(pathToFileWithChatIds));

            if (!System.IO.File.Exists(pathToFileWithChatIds))
            {
                System.IO.File.Create(pathToFileWithChatIds);
            }

            _messages = new Dictionary<long, ICollection<Message>>();

            var chatIds = System.IO.File.ReadAllLines(_fileWithChatIds).Select(long.Parse);
            foreach (var id in chatIds)
            {
                _messages.Add(id, new List<Message>());
            }

            _botClient.OnMessage += HandleMessage;
            _botClient.StartReceiving();
        }

        private void HandleMessage(object sender, MessageEventArgs messageEventArgs)
        {
            SaveMessage(messageEventArgs.Message);
        }

        public async void SendMessage(long chatId, string message)
        {
            Message msg = await _botClient.SendTextMessageAsync(chatId, message);
            SaveMessage(msg);
        }

        public async void SendMessageToAll(string message)
        {
            foreach (long i in _messages.Keys)
            {
                SaveMessage(await _botClient.SendTextMessageAsync(i, message));
            }
        }

        public ICollection<Message> GetMessages(long chatId)
        {
            if (_messages.ContainsKey(chatId))
            {
                return _messages[chatId];
            }

            return Array.Empty<Message>();
        }

        public long[] GetChatIds()
        {
            return _messages.Keys.ToArray();
        }

        private void SaveMessage(Message msg)
        {
            if (_messages.ContainsKey(msg.Chat.Id))
            {
                _messages[msg.Chat.Id].Add(msg);
            }
            else
            {
                _messages.Add(msg.Chat.Id, new List<Message> { msg });
                System.IO.File.AppendAllText(_fileWithChatIds, msg.Chat.Id.ToString());
            }
        }
    }
}
