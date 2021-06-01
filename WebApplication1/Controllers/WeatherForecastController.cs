using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using School.Business.Contract;
using School.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/h{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastBusinessMgr _weatherForecastBusinessMgr;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            , IWeatherForecastBusinessMgr weatherForecastBusinessMgr)
        {
            _logger = logger;
            _weatherForecastBusinessMgr = weatherForecastBusinessMgr;
        }

        [HttpGet]
        [Route("Get")]
        public IEnumerable<WeatherForecastViewModel> Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecastViewModel
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
            return _weatherForecastBusinessMgr.Get();
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        [Route("GetV1_1")]
        public ActionResult<IEnumerable<string>> GetV1_1()
        {
            return new string[] { "version 1.1 value 1", "version 1.1 value2 " };
        }
    }
}
