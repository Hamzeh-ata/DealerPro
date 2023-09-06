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
    public class GTSGpu : Hub, IGTSGpu
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/GPU/";
        string storeName = "GTS";
        string url = "https://gts.jo/components/computer-components/graphic-cards";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSGpu(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getGPU()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
