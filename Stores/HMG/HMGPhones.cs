using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using DealerPro.crawlersServices.HMG;
using DealerPro.fireBase;

namespace test.HMG
{
    public class HMGPhones : Hub, IHMGPhones
    {
 
        string endPoint = "Products/Mobiles/";
        string storeName = "HMG";
        string url = "https://hmg.jo/product-category/electronics/mobile-phones/";
        private FireBase firebase;
        private HMGDataCrawler crawler;
        public HMGPhones(FireBase firebase, HMGDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getPhone()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
