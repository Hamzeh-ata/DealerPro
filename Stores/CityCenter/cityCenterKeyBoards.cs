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
    public class cityCenterKeyBoards : Hub, IcityCenterKeyBoards
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Keyboards/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-peripherals/keyboard";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;

        public cityCenterKeyBoards(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getKeyBoards()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
