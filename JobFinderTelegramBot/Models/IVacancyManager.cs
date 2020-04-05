using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinderTelegramBot.Models
{
    public interface IVacancyManager
    {
        IEnumerable<Vacancy> GetRelevantVacancies();
        Task<IEnumerable<Vacancy>> GetRelevantVacanciesAsync();
        IEnumerable<Vacancy> GetTestVacancies();
    }
}
