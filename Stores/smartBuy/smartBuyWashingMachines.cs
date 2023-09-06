using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using DealerPro.crawlersServices.smartBuy;
using DealerPro.fireBase;

namespace test.smartBuy
{
    public class smartBuyWashingMachines : Hub, IsmartBuyWashingMachines
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/washingMachines/";
        string storeName = "SmartBuy";
        string url = "https://smartbuy-me.com/smartbuystore/ar/%D8%A7%D9%84%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D9%85%D9%86%D8%B2%D9%84%D9%8A%D8%A9/%D8%A7%D9%84%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D9%83%D8%A8%D9%8A%D8%B1%D8%A9/%D8%BA%D8%B3%D8%A7%D9%84%D8%A7%D8%AA/c/30109";
        private FireBase firebase;
        private smartBuyDataCrawler crawler;
        public smartBuyWashingMachines(FireBase firebase, smartBuyDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getWashingMachine()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
