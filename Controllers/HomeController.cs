using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using FileReaderAPI.Areas.HelpPage.ModelDescriptions;
using FileReaderAPI.Areas.HelpPage.Models;


namespace FileReaderAPI.Controllers
{
	public class HomeController : Controller
	{

        public HomeController()
        : this(GlobalConfiguration.Configuration)
        {
        }

        public HomeController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }
        public ActionResult Index()
		{

            var apiDescriptions = Configuration.Services.GetApiExplorer().ApiDescriptions;

            ViewBag.ApiDescriptions = apiDescriptions.ToList();
            return View();
        }



        
    }

    public class RouteApi
    {
        public string Method { get; set; }
        public string Path { get; set; }

        public string FriendlyName { get; set; }
        public RouteApi()
        {
            
        }
    }



}
