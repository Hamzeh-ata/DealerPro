using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using DealerPro.crawlersServices.OS;
using DealerPro.fireBase;

namespace test.OS
{
    public class OSChairs : Hub, IChairs
    {
        string endPoint = "Products/Chairs/";
        string storeName = "OrintalStore";
        string url= "https://os-jo.com/accessories/gaming-chairs";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSChairs(FireBase firebase, OSDataCrawler crawler)
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
