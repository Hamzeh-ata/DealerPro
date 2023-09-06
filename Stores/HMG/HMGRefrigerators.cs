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
    public class HMGRefrigerators : Hub, IHMGRefrigerators
    {
        string endPoint = "Products/Refrigerators/";
        string storeName = "HMG";
        string url = "https://hmg.jo/product-category/refrigerators/";
        private FireBase firebase;
        private HMGDataCrawler crawler;
        public HMGRefrigerators(FireBase firebase, HMGDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getRefrigerator()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);

        }
    }
}
