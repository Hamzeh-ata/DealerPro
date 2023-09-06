using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using DealerPro.fireBase;
using DealerPro.crawlersServices.OS;

namespace test.OS
{
    public class OSSdd : Hub, IOSSdd
    {
        string endPoint = "Products/SSD/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/components/storage/solid-state-drive-ssd";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSSdd(FireBase firebase, OSDataCrawler crawler)
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
