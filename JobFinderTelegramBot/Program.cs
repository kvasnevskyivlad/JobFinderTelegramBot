using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobFinderTelegramBot.Models;
using JobFinderTelegramBot.Models.Rabota.UA;
using Newtonsoft.Json;
using RestSharp;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace JobFinderTelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            IVacancyManager vacancyManager = new RabotaUaVacancyManager();

            //foreach (var vacancy in vacancies)
            //{
            //    Console.ForegroundColor = ConsoleColor.DarkGreen;
            //    Console.WriteLine($"{new string('-', 25)}\n");
            //    Console.ForegroundColor = ConsoleColor.White;
            //    Console.WriteLine(vacancy);
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine($"Relevant: {vacancy.Relevant}");
            //    Console.ForegroundColor = ConsoleColor.DarkGreen;
            //    Console.WriteLine($"\n{new string('-', 25)}\n");
            //}

            var bot = new TelegramBotClient("1218634931:AAGoCrVfv6neKITCAx4qIvnXZuRJ7n7rk4Q");
            bot.OnMessage +=
                async (sender, eventArgs) =>
                {
                    switch (eventArgs.Message.Text)
                    {
                        case @"/relevant_vacancies":

                            await bot.SendTextMessageAsync(eventArgs.Message.Chat.Id, "Please wait, a job search is in progress, this may take several minutes...");

                            var vacancies = await vacancyManager.GetRelevantVacanciesAsync();

                            foreach (var vacancy in vacancies)
                            {
                                await bot.SendTextMessageAsync(eventArgs.Message.Chat.Id,
                                        $"{vacancy.Name}\n{vacancy.ShortDescription}\n{vacancy.Url}");
                            }
                            break;
                        case @"/new_vacancies":
                            break;
                        default:
                            await bot.SendTextMessageAsync(eventArgs.Message.Chat.Id, "Sorry, I don't understand you 😢");
                            break;
                    }
                };
            bot.StartReceiving();

            Console.ReadKey();
        }
    }
}
