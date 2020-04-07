using System;
using JobFinderTelegramBot.Models;
using JobFinderTelegramBot.Models.Rabota.UA;
using JobFinderTelegramBot.Models.Telegram;
using JobFinderTelegramBot.Models.Vacancies;
using Telegram.Bot;

namespace JobFinderTelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            IJobFinderBotManager jobFinderBotManager = new JobFinderBotManagerModel();
            jobFinderBotManager.Run();

            Console.ReadKey();
        }
    }
}
