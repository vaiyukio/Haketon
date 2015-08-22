using System;
using System.Collections.Generic;
using Nancy;
using Haketon.Models;
using Haketon.USSD;

namespace Haketon
{
    public class DasboardModule : NancyModule
    {
        public DasboardModule()
        {
            Get["/sunburst"] = parameters => 
            {
                return View["sunburst"];
            };
        }
    }
}
