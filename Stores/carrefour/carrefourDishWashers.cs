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
    public class carrefourDishWashers : Hub, IcarrefourDishWashers
    {
        string endPoint = "Products/Dishwashers/";
        string storeName = "Carrefour";
        string url = "https://www.carrefourjordan.com/mafjor/en/c/NFJOR4100300";
        private FireBase firebase;
        private CarreFourDataCrawler crawler;
        public carrefourDishWashers(FireBase firebase, CarreFourDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }

        public async Task getDishWasher()
        {

            await crawler.CrawlerAsync(url, endPoint, storeName);

        }

    }
}

