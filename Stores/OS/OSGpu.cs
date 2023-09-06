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
    public class OSGpu : Hub, IOSGpu
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/GPU/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/components/graphics-cards";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSGpu(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getGpu()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
