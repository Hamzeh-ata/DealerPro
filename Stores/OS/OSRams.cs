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
    public class OSRams : Hub, IOSRams
    {
        string endPoint = "Products/RAM/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/components/system-memory";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSRams(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getRam()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
