using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinderTelegramBot.Models.Rabota.UA
{
    public class RabotaUaDocumentShort
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string Date { get; set; }
        public string DateTxt { get; set; }
        public string DesignBannerFullUrl { get; set; }
        public string ShortDescription { get; set; }
        public string CompanyName { get; set; }
        public string NotebookId { get; set; }

        public string Url => $"https://rabota.ua/company{NotebookId}/vacancy{Id}";
    }
}
