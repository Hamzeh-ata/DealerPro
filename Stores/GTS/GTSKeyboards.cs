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
    public class GTSKeyboards : Hub, IGTSKeyboards
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Keyboards/";
        string storeName = "GTS";
        string url = "https://gts.jo/gaming/gaming-keyboard";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSKeyboards(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getKeyboard()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
