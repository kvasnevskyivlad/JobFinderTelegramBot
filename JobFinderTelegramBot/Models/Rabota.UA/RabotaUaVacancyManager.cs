using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobFinderTelegramBot.Models.Vacancies;
using Newtonsoft.Json;
using RestSharp;

namespace JobFinderTelegramBot.Models.Rabota.UA
{
    public class RabotaUaVacancyManager : IVacancyManager
    {
        private readonly string vacancyName = "C# Developer";
        private readonly RestClient client = new RestClient("https://ua-api.rabota.ua");
        private readonly string[] keyWordsRelevant = new string[] { "c#", "wpf", "wcf" };
        private readonly string[] keyWordsAntiRelevant = new string[] { "asp" };
        private readonly double minRelevant = 0.5;

        public IEnumerable<Vacancy> GetRelevantVacancies()
        {
            var relevantVacancies = new List<Vacancy>();
            var vacancies = new List<Vacancy>();

            if (TryGetPageCount(out var pageCount))
            {
                for (var i = 0; i < pageCount; i++)
                {
                    var request = new RestRequest("vacancy/search", Method.GET);
                    request.AddParameter("page", i);
                    request.AddParameter("cityId", 1);
                    request.AddParameter("ukrainian", true);
                    request.AddParameter("keyWords", "C# Developer");

                    var response = client.Get(request);
                    if (response.IsSuccessful)
                    {
                        var rabotaResponse = JsonConvert.DeserializeObject<RabotaUaVacancies>(response.Content);
                        vacancies.AddRange(rabotaResponse.Documents.Select(rabotaVacancyDocument => new Vacancy(rabotaVacancyDocument)));
                    }
                }
            }

            foreach (var vacancy in vacancies)
            {
                var relevant = GetVacancyRelevant(vacancy);

                if (relevant >= minRelevant)
                {
                    vacancy.Relevant = relevant;
                    relevantVacancies.Add(vacancy);
                }
            }

            return relevantVacancies;
        }

        public Task<IEnumerable<Vacancy>> GetRelevantVacanciesAsync()
        {
            return Task.Run(GetRelevantVacancies);
        }

        public IEnumerable<Vacancy> GetTestVacancies()
        {
            var vacancies = new List<Vacancy>();

            var request = new RestRequest("vacancy/search", Method.GET);
            request.AddParameter("page", 0);
            request.AddParameter("cityId", 1);
            request.AddParameter("ukrainian", true);
            request.AddParameter("keyWords", "C# Developer");

            var response = client.Get(request);
            if (response.IsSuccessful)
            {
                var rabotaResponse = JsonConvert.DeserializeObject<RabotaUaVacancies>(response.Content);
                vacancies.AddRange(rabotaResponse.Documents.Select(rabotaVacancyDocument => new Vacancy(rabotaVacancyDocument)));
            }

            return vacancies;
        }

        private bool TryGetPageCount(out int pageCount)
        {
            try
            {
                var request = new RestRequest("vacancy/search", Method.GET);
                request.AddParameter("page", 0);
                request.AddParameter("cityId", 1);
                request.AddParameter("ukrainian", true);
                request.AddParameter("keyWords", vacancyName);

                var response = client.Get(request);
                if (response.IsSuccessful)
                {
                    var rabotaResponse = JsonConvert.DeserializeObject<RabotaUaVacanciesBasic>(response.Content);
                    pageCount = rabotaResponse.Total / rabotaResponse.Count;
                    return true;
                }

                pageCount = -1;
                return false;
            }
            catch (Exception)
            {
                pageCount = -1;
                return false;
            }
        }

        double GetVacancyRelevant(Vacancy vacancy)
        {
            var result = 0.0;

            var keyWordsRelevant = new string[] { "c#", "wpf", "wcf" };
            var keyWordsAntiRelevant = new string[] { "asp" };

            var request = new RestRequest("vacancy", Method.GET);
            request.AddParameter("id", vacancy.Id);
            request.AddParameter("ukrainian", true);
            request.AddParameter("isFromElasticSearch", true);

            var response = client.Get(request);
            if (response.IsSuccessful)
            {
                var rabotaUaDocumentFull = JsonConvert.DeserializeObject<RabotaUaDocumentFull>(response.Content);

                foreach (var keyWord in keyWordsAntiRelevant)
                {
                    var isContainsKeyWord = rabotaUaDocumentFull.Description.ToLower().Contains(keyWord);
                    if (isContainsKeyWord)
                    {
                        return 0.0;
                    }
                }

                foreach (var keyWord in keyWordsRelevant)
                {
                    var isContainsKeyWord = rabotaUaDocumentFull.Description.ToLower().Contains(keyWord);
                    if (isContainsKeyWord)
                    {
                        result += 1.0 / keyWordsRelevant.Length;
                    }
                }
            }

            return result;
        }

        private double CalculateVacancyRelevant(Vacancy vacancy)
        {
            var result = 0.0;

            var request = new RestRequest("vacancy", Method.GET);
            request.AddParameter("id", vacancy.Id);
            request.AddParameter("ukrainian", true);
            request.AddParameter("isFromElasticSearch", true);

            var response = client.Get(request);
            if (response.IsSuccessful)
            {
                var rabotaUaDocumentFull = JsonConvert.DeserializeObject<RabotaUaDocumentFull>(response.Content);

                foreach (var keyWord in keyWordsAntiRelevant)
                {
                    var isContainsKeyWord = rabotaUaDocumentFull.Description.ToLower().Contains(keyWord);
                    if (isContainsKeyWord)
                    {
                        return 0.0;
                    }
                }

                foreach (var keyWord in keyWordsRelevant)
                {
                    var isContainsKeyWord = rabotaUaDocumentFull.Description.ToLower().Contains(keyWord);
                    if (isContainsKeyWord)
                    {
                        result += 1.0 / keyWordsRelevant.Length;
                    }
                }
            }

            return result;
        }
    }
}
