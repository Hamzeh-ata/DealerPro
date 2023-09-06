using DealerPro.crawlersServices.cityCenter;
using DealerPro.fireBase;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using test.Interfaces;

namespace test.Hubs
{
    public class cityCenterChairs : Hub, IChairs
    {
        string endPoint = "Products/Chairs/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-gaming-chairs";
        private FireBase firebase;
        private CityCenterDataCrawler crawler;
        public cityCenterChairs(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetChair()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
