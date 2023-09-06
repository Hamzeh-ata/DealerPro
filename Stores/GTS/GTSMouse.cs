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
    public class GTSMouse : Hub, IGTSMouse
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Mouses/";
        string storeName = "GTS";
        string url = "https://gts.jo/accessories/laptop-desktop-accessories/mouse-1";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSMouse(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getMouse()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
