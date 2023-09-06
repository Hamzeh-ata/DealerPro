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
    public class GTSComputers : Hub, IGTSComputers
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Computers/";
        string storeName = "GTS";
        string url = "https://gts.jo/computers/custom-gaming-pc";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSComputers(FireBase firebase, GTSDataCrawler crawler)
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
