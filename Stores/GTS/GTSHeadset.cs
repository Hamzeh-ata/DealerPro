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
    public class GTSHeadset : Hub, IGTSHeadset
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Headset/";
        string storeName = "GTS";
        string url = "https://gts.jo/gaming/gaming-headset";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSHeadset(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getHeadset()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
