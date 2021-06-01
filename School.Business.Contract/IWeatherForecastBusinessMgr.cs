using School.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Business.Contract
{
    public interface IWeatherForecastBusinessMgr
    {
        IEnumerable<WeatherForecastViewModel> Get();
    }
}
