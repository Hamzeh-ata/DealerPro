using DealerPro.crawlersServices.carreFour;
using DealerPro.fireBase;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using test.Interfaces;
namespace test.carrefour
{
    public class carrefourMobiles: Hub ,IcarrefourMobiles
    {
        string endPoint = "Products/Mobiles/";
        string storeName = "Carrefour";
        string url = "https://www.carrefourjordan.com/mafjor/en/c/NFJOR1220200";
        private FireBase firebase;
        private CarreFourDataCrawler crawler;
        public carrefourMobiles(FireBase firebase, CarreFourDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }

        public async Task getMobile()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
