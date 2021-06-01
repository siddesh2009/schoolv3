using Microsoft.AspNetCore.Mvc;
using School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.WebLayer.Controllers
{
    [Controller]
    public abstract class BaseController: Controller
    {
        public AuthenticateResponseViewModel Account => (AuthenticateResponseViewModel)HttpContext.Items["Account"];
    }
}
