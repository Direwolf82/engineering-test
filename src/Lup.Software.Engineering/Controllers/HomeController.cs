namespace Lup.Software.Engineering.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        [Route("{token:required}")]
        public IActionResult Index(string token)
        {
            return this.View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
