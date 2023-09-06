using test.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using test.Models;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Telerik.JustMock;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using DealerPro.crawlersServices.cityCenter;
using DealerPro.fireBase;

namespace test.Hubs
{
    public class cityCenterSSD : Hub, IcityCenterSSD
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/SSD/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming-storage-drive/ssds";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;
        public cityCenterSSD(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetSSD()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}

