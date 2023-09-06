namespace test.Hubs
{
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

    public class cityCenterLaptopsHub : Hub, ILaptopNamesHub
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        List<string> lapTopSpecifications = new List<string>();
        string endPoint = "Products/Laptops/";
        string storeName = "CityCenter";
        string url = "https://citycenter.jo/pc-and-laptops/pc-and-laptops-laptops/gaming-laptop";
        private CityCenterDataCrawler crawler;
        private FireBase firebase;

        public cityCenterLaptopsHub(FireBase firebase, CityCenterDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task GetLaptops()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
