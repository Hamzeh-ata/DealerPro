using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium.Interactions;
using DealerPro.crawlersServices.cityCenter;
using DealerPro.fireBase;

namespace test.Hubs
{
    public class cityCenterPCs : Hub, IcityCenterPCs
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Computers/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-gaming-pcs";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;
        public cityCenterPCs(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetPC()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
