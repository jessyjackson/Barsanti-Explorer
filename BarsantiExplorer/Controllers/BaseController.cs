using BarsantiExplorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

public class BaseController: Controller
{
    protected BarsantiDbContext DB;
    
    public BaseController(BarsantiDbContext context)
    {
        DB = context;
    }
}
