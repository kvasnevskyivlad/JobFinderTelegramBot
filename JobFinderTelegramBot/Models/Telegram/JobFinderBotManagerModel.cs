using System;
using System.IO;
using JobFinderTelegramBot.Models.Rabota.UA;
using JobFinderTelegramBot.Models.Vacancies;
using Telegram.Bot;

namespace JobFinderTelegramBot.Models.Telegram
{
    public class JobFinderBotManagerModel : IJobFinderBotManager
    {
        private readonly string accessTokenFileName = "BotToken.tkn";
        private readonly TelegramBotClient botClient;
        private readonly IVacancyManager vacancyManager;

        public JobFinderBotManagerModel()
        {
            vacancyManager = new RabotaUaVacancyManager();
            botClient = new TelegramBotClient(GetAccessToken());
            botClient.OnMessage +=
                (sender, eventArgs) =>
                {
                    switch (eventArgs.Message.Text)
                    {
                        case @"/relevant_vacancies":

                            botClient.SendTextMessageAsync(eventArgs.Message.Chat.Id, "Please wait, a job search is in progress, this may take several minutes...");

                            vacancyManager.GetRelevantVacanciesAsync().ContinueWith(task =>
                            {
                                foreach (var vacancy in task.Result)
                                {
                                    botClient.SendTextMessageAsync(eventArgs.Message.Chat.Id, vacancy.Url);
                                }
                            });
                            break;
                        case @"/new_vacancies":
                            break;
                        default:
                            botClient.SendTextMessageAsync(eventArgs.Message.Chat.Id, "Sorry, I don't understand you 😢");
                            break;
                    }
                };
        }

        private string GetAccessToken()
        {
            try
            {
                return File.ReadAllText(accessTokenFileName);
            }
            catch (Exception)
            {
                throw new Exception("Access token read error!");
            }
        }

        public void Run()
        {
            botClient.StartReceiving();
        }
    }
}
