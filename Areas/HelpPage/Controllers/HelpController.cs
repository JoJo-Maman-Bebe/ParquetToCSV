using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using FileReaderAPI.Areas.HelpPage.ModelDescriptions;
using FileReaderAPI.Areas.HelpPage.Models;
using FileReaderAPI.Helpers;
using FileReaderAPI.Models;
namespace FileReaderAPI.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            
            return View(GetApiLogModels());

        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }

        private List<ApiLogModel> GetApiLogModels()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            List<ApiLogModel> apiLogModels = new List<ApiLogModel>();

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetApiList", connection);
                using (var reader = dbHelper.ExecuteCommandReader(command))
                {
                    while (reader.Read())
                    {
                        apiLogModels.Add(GetApiListData(reader));
                    }

                }
            }

                return apiLogModels;
        }

            private ApiLogModel GetApiListData(IDataReader reader)
            {
                return new ApiLogModel()
                {
                    ApiName = Convert.ToString(reader["Api Name"]),
                    LastActivity = Convert.ToDateTime(reader["Last Activity"]),
                    LastResult = Convert.ToString(reader["Last Result"]),



                };

            }


        }
}