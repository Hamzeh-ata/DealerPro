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
    public class OSComputers : Hub, IOSComputers
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Computers/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/computer-systems/desktop-computer";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSComputers(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getComputers()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
