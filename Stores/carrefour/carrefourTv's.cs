using DealerPro.crawlersServices.carreFour;
using DealerPro.fireBase;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using test.Interfaces;

namespace test.carrefour
{
    public class carrefourTv_s : Hub,IcarrefourTv_s
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Tv's/";
        string storeName = "Carrefour";
        string url = "https://www.carrefourjordan.com/mafjor/en/c/NFJOR4080400";
        private FireBase firebase;
        private CarreFourDataCrawler crawler;
        public carrefourTv_s(FireBase firebase, CarreFourDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getTv()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
         }
    }

