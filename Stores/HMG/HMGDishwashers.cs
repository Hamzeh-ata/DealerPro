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
    public class HMGDishwashers : Hub, IHMGDishwashers
    {
        string endPoint = "Products/Dishwashers/";
        string storeName = "HMG";
        string url = "https://hmg.jo/product-category/dishwashers/";
        private FireBase firebase;
        private HMGDataCrawler crawler;
        public HMGDishwashers(FireBase firebase, HMGDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getGDishwashers()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
