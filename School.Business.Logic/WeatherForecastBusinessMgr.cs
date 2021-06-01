using School.Business.Contract;
using School.Data.Contract;
using School.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Business.Logic
{
    public class WeatherForecastBusinessMgr : IWeatherForecastBusinessMgr
    {
        private readonly IWeatherForecastDataMgr _weatherForecastDataMgr;

        public WeatherForecastBusinessMgr(IWeatherForecastDataMgr weatherForecastDataMgr)
        {
            _weatherForecastDataMgr = weatherForecastDataMgr;
        }

        public IEnumerable<WeatherForecastViewModel> Get()
        {
            return _weatherForecastDataMgr.Get();
        }
    }
}
