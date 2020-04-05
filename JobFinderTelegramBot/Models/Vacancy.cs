using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobFinderTelegramBot.Models.Rabota.UA;

namespace JobFinderTelegramBot.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string ShortDescription { get; set; }
        public string CompanyName { get; set; }
        public string Url { get; set; }
        public double Relevant { get; set; }

        public bool IsHaveCompanyLogo => !string.IsNullOrEmpty(CompanyLogoUrl);

        public Vacancy(RabotaUaDocumentShort documentShort)
        {
            Id = documentShort.Id;
            Name = documentShort.Name;
            City = documentShort.CityName;
            Date = documentShort.Date;
            CompanyLogoUrl = documentShort.DesignBannerFullUrl;
            ShortDescription = documentShort.ShortDescription;
            CompanyName = documentShort.CompanyName;
            Url = documentShort.Url;
        }

        public override string ToString()
        {
            return $"Name: {Name}\nCompany: {CompanyName}\nDate: {Date}\nShortDescription: {ShortDescription}\nUrl: {Url}";
        }
    }
}
