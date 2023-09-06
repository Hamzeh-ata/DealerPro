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
    public class GTSPsu : Hub, IGTSPsu
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/powerSupplies/";
        string storeName = "GTS";
        bool flag = true;
        string url = "https://gts.jo/components/computer-components/power-supplies";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSPsu(FireBase firebase, GTSDataCrawler crawler)
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
