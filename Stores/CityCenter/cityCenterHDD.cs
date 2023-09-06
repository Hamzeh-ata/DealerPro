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
    public class cityCenterHDD : Hub, IcityCenterHDD
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/HDD/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-storage-drive/hard-drives";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;

        public cityCenterHDD(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetHDD()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
