using DealerPro.crawlersServices.cityCenter;
using DealerPro.fireBase;
using DealerPro.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DealerPro.CityCenter
{
    public class cityCenterEveryDayUseLaptops : Hub, IcityCenterEveryDayUseLaptops
    {
        string endPoint = "Products/EveryDayUseLaptops/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/pc-and-laptops/pc-and-laptops-laptops/home-student";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;
        public cityCenterEveryDayUseLaptops(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getLaptop()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
