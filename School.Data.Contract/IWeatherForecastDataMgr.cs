using School.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Data.Contract
{
    public interface IWeatherForecastDataMgr
    {
        IEnumerable<WeatherForecastViewModel> Get();
    }
}
