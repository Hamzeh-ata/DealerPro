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
    public class GTSLaptops : Hub, IGTSLaptops
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Laptops/";
        string storeName = "GTS";
        string url = "https://gts.jo/computers/laptops/gaming-laptop";
        private FireBase firebase;
        private GTSDataCrawler crawler;
        public GTSLaptops(FireBase firebase, GTSDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getLaptops()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);


        }
        }
    }

