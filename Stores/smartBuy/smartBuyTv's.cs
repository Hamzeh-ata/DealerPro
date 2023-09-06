using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using DealerPro.fireBase;
using DealerPro.crawlersServices.smartBuy;

namespace test.smartBuy
{
    public class smartBuyTv_s : Hub, IsmartBuyTv_s
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Tv's/";
        string storeName = "SmartBuy";
        string url = "https://smartbuy-me.com/smartbuystore/ar/%D8%A7%D9%84%D8%B4%D8%A7%D8%B4%D8%A7%D8%AA-%D9%88-%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D8%AA%D8%B1%D9%81%D9%8A%D9%87/%D8%A7%D9%84%D8%B4%D8%A7%D8%B4%D8%A7%D8%AA-%D9%88-%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D8%AA%D8%B1%D9%81%D9%8A%D9%87/c/403";
        private FireBase firebase;
        private smartBuyDataCrawler crawler;

        public smartBuyTv_s(FireBase firebase, smartBuyDataCrawler crawler)
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
