using DealerPro.crawlersServices.carreFour;
using DealerPro.fireBase;
using DealerPro.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DealerPro.carrefour
{
    public class carrefourWashingMachines :Hub, IcarrefourWashingMachines
    {

        string endPoint = "Products/washingMachines/";
        string storeName = "Carrefour";
        string url = "https://www.carrefourjordan.com/mafjor/ar/c/NFJOR4100700";
        private FireBase firebase;
        private CarreFourDataCrawler crawler;
        public carrefourWashingMachines(FireBase firebase, CarreFourDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }

        public async Task getWashingMachine()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
