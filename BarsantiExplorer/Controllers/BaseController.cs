using BarsantiExplorer.Models;
using BarsantiExplorer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

public class BaseController : Controller
{
    protected BarsantiDbContext DB;
    protected IConfiguration? AppSettings;

    public BaseController(BarsantiDbContext context)
    {
        DB = context;
    }

    public BaseController(BarsantiDbContext context, IConfiguration appSettings)
    {
        DB = context;
        AppSettings = appSettings;
    }
}