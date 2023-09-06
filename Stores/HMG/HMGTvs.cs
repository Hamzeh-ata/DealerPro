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
    public class HMGTvs : Hub, IHMGTvs
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Tv's/";
        string storeName = "HMG";
        string url = "https://hmg.jo/product-category/audio-visual/";
        private FireBase firebase;
        private HMGDataCrawler crawler;
        public HMGTvs(FireBase firebase, HMGDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getTv()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }

}
