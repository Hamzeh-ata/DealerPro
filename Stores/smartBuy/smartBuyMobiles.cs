using DealerPro.crawlersServices.smartBuy;
using DealerPro.fireBase;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using test.Interfaces;

namespace test.smartBuyMobiles
{
    public class smartBuyMobiles : Hub, IsmartBuyMobiles
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Mobiles/";
        string storeName = "SmartBuy";
        private FireBase firebase;
        private smartBuyDataCrawler crawler;
        string url = "https://smartbuy-me.com/smartbuystore/ar/%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D8%A7%D8%AA%D8%B5%D8%A7%D9%84/%D8%A7%D9%84%D9%85%D9%88%D8%A8%D8%A7%D9%8A%D9%84-%D9%88-%D8%A7%D9%84%D8%AA%D8%A7%D8%A8%D9%84%D8%AA/%D9%87%D9%88%D8%A7%D8%AA%D9%81-%D9%86%D9%82%D8%A7%D9%84%D8%A9/c/20303";
        public smartBuyMobiles(FireBase firebase, smartBuyDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getMobile()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);

        }
    }
}
