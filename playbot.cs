using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    internal class PlayBot
    {
        private readonly TelegramBotClient _bot;

        public PlayBot()
        {
            _bot = new TelegramBotClient("5432445557:AAHr_pYI0t802Goe6F_v2ISvDI-I0tYIO-k");
        }

        public void StartBot()
        {
            _bot.StartReceiving(HandleUpdate, HandleError);
            while (true)
            {
                Console.WriteLine("Bot is worked all right");
                Thread.Sleep(int.MaxValue);
            }
        } 

            private async Task Start(Message message)
        {
            var user = message.From;
            if (user == null)
                return;
            if (message.Text.ToLower() == "/start")
            {
                await _bot.SendTextMessageAsync(user.Id, "HI! Commands: /help = for info  |  /game = to start play");
            }
            if (message.Text.ToLower() == "/help")
            {
                await _bot.SendTextMessageAsync(user.Id, "Rock wins against scissors; paper wins against rock; and scissors wins against paper. If both players throw the same hand signal, it is considered a tie, and play resumes until there is a clear winner.");
            }
            if (message.Text.ToLower() == "/game")
            {
                
                await _bot.SendTextMessageAsync(user.Id, "You put:", replyMarkup: Markup());
            }
        }
        public static string RockPaperScissors(string first, string second)
        {
            string result = first.ToLower() + second.ToLower();
            switch (result)
            {
                case "rockpaper":
                    return "rock is covered by paper. Paper wins.";
                case "rockscissors":
                    return "rock breaks scissors. Rock wins.";
                case "paperrock":
                    return "paper covers rock. Paper wins.";
                case "paperscissors":
                    return "paper is cut by scissors. Scissors wins.";
                case "scissorsrock":
                    return "scissors is broken by rock. Rock wins.";
                case "scissorpaper":
                    return "scissors cuts paper. Scissors wins.";
                default:
                    return "tie";
            }
        }
        private async Task HandleCallbackQuery(CallbackQuery cbq)
        {
            if (cbq.Data == "End")
            {
                await _bot.SendTextMessageAsync(cbq.Message.Chat.Id, "Thank you for playing");
                await Console.Out.WriteLineAsync("End");
            }
            else if (cbq.Data == "Try")
            {
                await _bot.SendTextMessageAsync(cbq.Message.Chat.Id, "You put:", replyMarkup: Markup());
            }
            else
            {
                var markup = new InlineKeyboardMarkup(new[]
                    {
                    new InlineKeyboardButton("Try"){ Text = "Try Again", CallbackData = "Try"},
                    new InlineKeyboardButton("End"){ Text = "End Game", CallbackData = "End"},
                });
                await _bot.SendTextMessageAsync(cbq.Message.Chat.Id, "Bot move " + BotMove() + " | " + RockPaperScissors(cbq.Data, BotMove()), replyMarkup: markup);
            }
        }
        private InlineKeyboardMarkup Markup()
        {
            return new InlineKeyboardMarkup(new[] { new InlineKeyboardButton("Rock") { Text = "Rock", CallbackData = "Rock" }, new InlineKeyboardButton("Scissors") { Text = "Scissors", CallbackData = "Scissors" }, new InlineKeyboardButton("Paper") { Text = "Paper", CallbackData = "Paper" } });
        }
        private string BotMove()
        {
            Random random = new Random();
            int temp = random.Next(1, 4);
            if (temp == 1)
                return "rock";
            if (temp == 2)
                return "paper";
            if (temp == 3)
                return "scissors";
            return "default";
        }

    

        private async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await Start(update.Message);
                    break;
                case UpdateType.CallbackQuery:
                    await HandleCallbackQuery(update.CallbackQuery);
                    break;
            }
        }
        private async Task HandleError(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
        {
            await Console.Error.WriteLineAsync(exception.Message);
        }
    }
}

