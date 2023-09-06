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
    public class cityCenterMouses : Hub, IcityCenterMouses
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Mouses/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-peripherals/mouse";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;
        public cityCenterMouses(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetMouses()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);

        }
    }
   }

