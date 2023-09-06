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
    public class OSMouse : Hub, IOSMouse
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Mouses/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/accessories/keyboard-and-mouse/mouse";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSMouse(FireBase firebase, OSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getMouses()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
