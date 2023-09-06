using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using test.Interfaces;
using DealerPro.crawlersServices.GTS;
using DealerPro.fireBase;

namespace test.GTS
{
    public class GTSCpu : Hub, IGTSCpu
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/CPU/";
        string storeName = "GTS";
        string url = "https://gts.jo/components/processors-cpu";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSCpu(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getCpu()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
