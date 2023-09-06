using DealerPro.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using test.carrefour;
using test.GTS;
using test.HMG;
using test.Hubs;
using test.Interfaces;
using test.Models;
using test.OS;
using test.smartBuy;
using test.smartBuyMobiles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DealerPro.fireBase;
using DealerPro.crawlersServices.cityCenter;
using DealerPro.crawlersServices.GTS;
using DealerPro.crawlersServices.OS;
using DealerPro.crawlersServices.carreFour;
using DealerPro.carrefour;
using DealerPro.Interfaces;
using DealerPro.crawlersServices.HMG;
using DealerPro.crawlersServices.smartBuy;
using DealerPro.OS;
using DealerPro.GTS;
using DealerPro.CityCenter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddHangfire(config =>
config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
.UseSimpleAssemblyNameTypeSerializer()
.UseDefaultTypeSerializer()
.UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddSingleton<ILaptopNamesHub, cityCenterLaptopsHub>();
builder.Services.AddSingleton<IcityCenterCpu, cityCenterCpu>();
builder.Services.AddSingleton<IcityCenterMotherBoards, cityCenterMotherBoards>();
builder.Services.AddSingleton<IcityCenterGraphicCard, cityCenterGraphicCard>();
builder.Services.AddSingleton<IcityCenterSSD, cityCenterSSD>();
builder.Services.AddSingleton<IcityCenterPCs, cityCenterPCs>();
builder.Services.AddSingleton<IcityCenterRams, cityCenterRams>();
builder.Services.AddSingleton<IcityCenterPSU, cityCenterPSU>();
builder.Services.AddSingleton<IcityCenterHDD, cityCenterHDD>();
builder.Services.AddSingleton<IChairs, cityCenterChairs>();
builder.Services.AddSingleton<IcityCenterMouses, cityCenterMouses>();
builder.Services.AddSingleton<IcityCenterMonitors, cityCenterMonitors>();
builder.Services.AddSingleton<IcityCenterKeyBoards, cityCenterKeyBoards>();
builder.Services.AddSingleton<IGTSLaptops, GTSLaptops>();
builder.Services.AddSingleton<IGTSComputers, GTSComputers>();
builder.Services.AddSingleton<IGTSCpu, GTSCpu>();
builder.Services.AddSingleton<IGTSGpu, GTSGpu>();
builder.Services.AddSingleton<IGTSPsu, GTSPsu>();
builder.Services.AddSingleton<IGTSRams, GTSRams>();
builder.Services.AddSingleton<IGTSHDD, GTSHDD>();
builder.Services.AddSingleton<IGTSSSD, GTSSSD>();
builder.Services.AddSingleton<IGTSKeyboards, GTSKeyboards>();
builder.Services.AddSingleton<IGTSHeadset, GTSHeadset>();
builder.Services.AddSingleton<IGTSMonitor, GTSMonitor>();
builder.Services.AddSingleton<IGTSMouse, GTSMouse>();
builder.Services.AddSingleton<IOSComputers, OSComputers>();
builder.Services.AddSingleton<IOSLaptops, OSLaptops>();
builder.Services.AddSingleton<IOSCpus, OSCPUS>();
builder.Services.AddSingleton<IOSRams, OSRams>();
builder.Services.AddSingleton<IOSGpu, OSGpu>();
builder.Services.AddSingleton<iOSMB, OSMB>();
builder.Services.AddSingleton<IOSHdd, OSHdd>();
builder.Services.AddSingleton<IOSSdd, OSSdd>();
builder.Services.AddSingleton<IChairs, OSChairs>();
builder.Services.AddSingleton<IOSHeadset, OSHeadset>();
builder.Services.AddSingleton<IOSKeyboards, OSKeyboards>();
builder.Services.AddSingleton<IOSMouse, OSMouse>();
builder.Services.AddSingleton<IOSMonitors, OSMonitors>();
builder.Services.AddSingleton<IOSPsu, OSPsu>();
builder.Services.AddSingleton<IsmartBuyMobiles, smartBuyMobiles>();
builder.Services.AddSingleton<IsmartBuyTv_s, smartBuyTv_s>();
builder.Services.AddSingleton<IsmartBuyWashingMachines, smartBuyWashingMachines>();
builder.Services.AddSingleton<IsmartBuyDishWashers, smartBuyDishWashers>();
builder.Services.AddSingleton<IsmartBuyRefrigerators, smartBuyRefrigerators>();
builder.Services.AddSingleton<IHMGRefrigerators, HMGRefrigerators>();
builder.Services.AddSingleton<IHMGTvs, HMGTvs>();
builder.Services.AddSingleton<IHMGWashingMachines, HMGWashingMachines>();
builder.Services.AddSingleton<IHMGDishwashers, HMGDishwashers>();
builder.Services.AddSingleton<IHMGPhones, HMGPhones>();
builder.Services.AddSingleton<IcarrefourTv_s, carrefourTv_s>();
builder.Services.AddSingleton<IcarrefourDishWashers, carrefourDishWashers>();
builder.Services.AddSingleton<IcarrefourMobiles, carrefourMobiles>();
builder.Services.AddSingleton<IcarrefourWashingMachines, carrefourWashingMachines>();
builder.Services.AddSingleton<FireBase, FireBaseFunctions>();
builder.Services.AddSingleton<CityCenterDataCrawler>();
builder.Services.AddSingleton<GTSDataCrawler>();
builder.Services.AddSingleton<OSDataCrawler>();
builder.Services.AddSingleton<CarreFourDataCrawler>();
builder.Services.AddSingleton<HMGDataCrawler>();
builder.Services.AddSingleton<smartBuyDataCrawler>();
builder.Services.AddSingleton<IOSEveryDayUseLaptops, OSEveryDayUseLaptops>();
builder.Services.AddSingleton<IGTSEveryDayLaptops, GTSEveryDayLaptops>();
builder.Services.AddSingleton<IcityCenterEveryDayUseLaptops, cityCenterEveryDayUseLaptops>();
builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Logging.AddConsole();
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: true);
});
// Configure application services
builder.Services.Configure<firebaseOptions>(builder.Configuration.GetSection("Firebase"));
// Add Identity services
// Add other necessary services and configurations
// Add controllers and views
builder.Services.AddControllersWithViews();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DealerPro", Version = "v1" });
});
var app = builder.Build();
var backgroundJobClient = app.Services.GetService<IBackgroundJobClient>();
var recurringJopManager = app.Services.GetService<IRecurringJobManager>();
var serviceProvider = app.Services.GetService<IServiceProvider>();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DealerPro V1");
    });
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/home")),
    RequestPath = "",
    EnableDefaultFiles = true,

});

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/scrapping")),
    RequestPath = "/home/scrappingPage",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/categories")),
    RequestPath = "/home/categories",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/products")),
    RequestPath = "/home/products",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/products")),
    RequestPath = "/home/products",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/compare")),
    RequestPath = "/home/compare",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/gamingCategories")),
    RequestPath = "/home/gamingCategories",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/homeElectricCategories")),
    RequestPath = "/home/homeElectricCategories",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/products")),
    RequestPath = "/home/gamingCategories/products",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/products")),
    RequestPath = "/home/homeElectricCategories/products",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/mobilesAndTablets")),
    RequestPath = "/home/mobilesAndTablets",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/products")),
    RequestPath = "/home/mobilesAndTablets/products",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/Login")),
    RequestPath = "/login",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/signUp")),
    RequestPath = "/signUp",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/storeDashBoard")),
    RequestPath = "/dashboard",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/forgetPassword")),
    RequestPath = "/login/forgetPassowrd",
    EnableDefaultFiles = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pages/dealerGuide")),
    RequestPath = "/DealerGuide",
    EnableDefaultFiles = true
});

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<cityCenterCpu>("/cityCenterCpu");
    endpoints.MapHub<GTSMouse>("/GTSMouse");
    endpoints.MapHub<GTSMonitor>("/GTSMonitor");
    endpoints.MapHub<GTSHeadset>("/GTSHeadset");
    endpoints.MapHub<GTSKeyboards>("/GTSKeyboards");
    endpoints.MapHub<GTSSSD>("/GTSSSD");
    endpoints.MapHub<GTSHDD>("/GTSHDD");
    endpoints.MapHub<GTSRams>("/GTSRams");
    endpoints.MapHub<GTSPsu>("/GTSPsu");
    endpoints.MapHub<GTSGpu>("/GTSGpu");
    endpoints.MapHub<GTSCpu>("/GTSCpu");
    endpoints.MapHub<GTSComputers>("/GTSComputers");
    endpoints.MapHub<GTSLaptops>("/GTSLaptops");
    endpoints.MapHub<cityCenterKeyBoards>("/cityCenterKeyBoards");
    endpoints.MapHub<cityCenterMonitors>("/cityCenterMonitors");
    endpoints.MapHub<cityCenterMouses>("/cityCenterMouses");
    endpoints.MapHub<cityCenterChairs>("/cityCenterChairs");
    endpoints.MapHub<cityCenterHDD>("/cityCenterHDD");
    endpoints.MapHub<cityCenterPSU>("/cityCenterPSU");
    endpoints.MapHub<cityCenterRams>("/cityCenterRams");
    endpoints.MapHub<cityCenterPCs>("/cityCenterPCs");
    endpoints.MapHub<cityCenterSSD>("/cityCenterSSD");
    endpoints.MapHub<cityCenterGraphicCard>("/cityCenterGraphicCard");
    endpoints.MapHub<cityCenterMotherBoards>("/cityCenterMotherBoards");
    endpoints.MapHub<OSGpu>("/OSGpu");
    endpoints.MapHub<cityCenterLaptopsHub>("/cityCenterLaptopsHub");
    endpoints.MapHangfireDashboard();
    endpoints.MapHub<OSComputers>("/OSComputers");
    endpoints.MapHub<OSLaptops>("/OSLaptops");
    endpoints.MapHub<OSCPUS>("/OSCPUS");
    endpoints.MapHub<OSRams>("/OSRams");
    endpoints.MapHub<OSMB>("/OSMB");
    endpoints.MapHub<OSHdd>("/OSHdd");
    endpoints.MapHub<OSChairs>("/OSChairs");
    endpoints.MapHub<OSSdd>("/OSSdd");
    endpoints.MapHub<OSHeadset>("/OSHeadset");
    endpoints.MapHub<OSKeyboards>("/OSKeyboards");
    endpoints.MapHub<OSMouse>("/OSMouse");
    endpoints.MapHub<OSMonitors>("/OSMonitors");
    endpoints.MapHub<OSPsu>("/OSPsu");
    endpoints.MapHub<smartBuyMobiles>("/smartBuyMobiles");
    endpoints.MapHub<smartBuyTv_s>("/smartBuyTv_s");
    endpoints.MapHub<smartBuyWashingMachines>("/smartBuyWashingMachines");
    endpoints.MapHub<smartBuyDishWashers>("/smartBuyDishWashers");
    endpoints.MapHub<smartBuyRefrigerators>("/smartBuyRefrigerators");
    endpoints.MapHub<HMGRefrigerators>("/HMGRefrigerators");
    endpoints.MapHub<HMGTvs>("/HMGTvs");
    endpoints.MapHub<HMGWashingMachines>("/HMGWashingMachines");
    endpoints.MapHub<HMGDishwashers>("/HMGDishwashers");
    endpoints.MapHub<HMGPhones>("/HMGPhones");
    endpoints.MapHub<carrefourTv_s>("/carrefourTv_s");
    endpoints.MapHub<carrefourDishWashers>("/carrefourDishWashers");
    endpoints.MapHub<carrefourMobiles>("/carrefourMobiles");
    endpoints.MapHub<carrefourWashingMachines>("/carrefourWashingMachines");
    endpoints.MapHub<cityCenterEveryDayUseLaptops>("/cityCenterEveryDayUseLaptops");
    endpoints.MapHub<GTSEveryDayLaptops>("/GTSEveryDayLaptops");
    endpoints.MapHub<OSEveryDayUseLaptops>("/OSEveryDayUseLaptops");


});

backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hangfire job!"));
//recurringJopManager.AddOrUpdate("Run every minute", () => serviceProvider.GetService<IChairs>().GetChair(), "* * * * *");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseFileServer();
app.UseRouting();
app.Run();
