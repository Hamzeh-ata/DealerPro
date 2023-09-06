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
using test.Interfaces;
using DealerPro.crawlersServices.cityCenter;
using DealerPro.fireBase;

namespace test.Hubs
{
    public class cityCenterGraphicCard : Hub, IcityCenterGraphicCard
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/GPU/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/gaming/gaming-graphic-card";
        private FireBase firebase;
        private CityCenterDataCrawler crawler;
        public cityCenterGraphicCard(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }

        public async Task GetGraphicCard()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }


    }
}
