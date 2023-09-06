using DealerPro.crawlersServices.GTS;
using DealerPro.fireBase;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using test.Interfaces;

namespace test.GTS
{
    public class GTSRams : Hub, IGTSRams
    {
        string endPoint = "Products/RAM/";
        string storeName = "GTS";
        string url = "https://gts.jo/components/ram";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSRams(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getRam()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
