using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using DealerPro.crawlersServices.OS;
using DealerPro.fireBase;

namespace test.OS
{
    public class OSHeadset : Hub, IOSHeadset
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Headset/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/accessories/audio/headset";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSHeadset(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetHeadSet()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
