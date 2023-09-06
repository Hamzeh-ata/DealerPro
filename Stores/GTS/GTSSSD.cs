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
    public class GTSSSD : Hub, IGTSSSD
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/SSD/";
        string storeName = "GTS";
        string url = "https://gts.jo/components/internal-storage/ssd-internal-storage";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSSSD(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getSSD()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
