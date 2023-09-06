using DealerPro.crawlersServices.GTS;
using DealerPro.fireBase;
using DealerPro.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DealerPro.GTS
{
    public class GTSEveryDayLaptops : Hub, IGTSEveryDayLaptops
    {
        string endPoint = "Products/EveryDayUseLaptops/";
        string storeName = "GTS";
        string url = "https://gts.jo/computers/laptops/everyday-use-laptop";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSEveryDayLaptops(FireBase firebase, GTSDataCrawler crawler)
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
