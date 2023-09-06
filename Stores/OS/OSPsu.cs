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
    public class OSPsu : Hub, IOSPsu
    {
        string endPoint = "Products/powerSupplies/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/components/power-supplies";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSPsu(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getPsu()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
