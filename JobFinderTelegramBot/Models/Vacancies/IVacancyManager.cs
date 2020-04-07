using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobFinderTelegramBot.Models.Vacancies
{
    public interface IVacancyManager
    {
        IEnumerable<Vacancy> GetRelevantVacancies();
        Task<IEnumerable<Vacancy>> GetRelevantVacanciesAsync();
        IEnumerable<Vacancy> GetTestVacancies();
    }
}
