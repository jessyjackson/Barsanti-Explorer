using BarsantiExplorer.Models;
using BarsantiExplorer.Services;
using BarsantiExplorer.TelegramBot;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

public class BaseController : Controller
{
    protected BarsantiDbContext DB;
    protected IConfiguration? AppSettings;
    protected Bot? TelegramBot;

    public BaseController(BarsantiDbContext context)
    {
        DB = context;
    }

    public BaseController(BarsantiDbContext context, IConfiguration appSettings)
    {
        DB = context;
        AppSettings = appSettings;
    }
    public BaseController(BarsantiDbContext context, IConfiguration appSettings,Bot telegramBot)
    {
        DB = context;
        AppSettings = appSettings;
        TelegramBot = telegramBot;
        
    }
}