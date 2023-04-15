using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FileReaderAPI.NextWebService;

namespace FileReaderAPI.Controllers
{
	[RoutePrefix("api/data")]
    public class DataController : ApiController
    {
		[HttpGet]
		[Route("InsertTransactions")]

		public IHttpActionResult InsertTransactions()
		{


			return Ok("Success");
		}


	}
}
