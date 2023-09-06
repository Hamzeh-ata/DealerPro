using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using DealerPro.crawlersServices.GTS;
using DealerPro.fireBase;

namespace test.GTS
{
    public class GTSMonitor : Hub, IGTSMonitor
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Monitors/";
        string storeName = "GTS";
        string url = "https://gts.jo/gaming/gaming-monitor";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSMonitor(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getMonitor()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
