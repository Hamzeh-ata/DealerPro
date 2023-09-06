using DealerPro.crawlersServices.OS;
using DealerPro.fireBase;
using DealerPro.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DealerPro.OS
{
    public class OSEveryDayUseLaptops : Hub, IOSEveryDayUseLaptops
    {
        string endPoint = "Products/EveryDayUseLaptops/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/computer-systems/laptops/home-laptops";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSEveryDayUseLaptops(FireBase firebase, OSDataCrawler crawler)
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
