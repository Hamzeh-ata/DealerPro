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
    public class GTSHDD : Hub, IGTSHDD
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/HDD/";
        string storeName = "GTS";
        string url = "https://gts.jo/components/internal-storage/hdd-internal-storage";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSHDD(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getHDD()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
