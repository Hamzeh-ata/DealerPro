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
    public class OSHdd : Hub, IOSHdd
    {
        
     private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/HDD/";
        string storeName = "OrintalStore";
        string url = "https://os-jo.com/components/storage/hdd-storage";
        private FireBase firebase;
        private OSDataCrawler crawler;
        public OSHdd(FireBase firebase, OSDataCrawler crawler)
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

