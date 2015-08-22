using System;
using Nancy;

namespace Haketon.Modules
{
    public class RegistrationModule : NancyModule
    {
        public RegistrationModule()
        {
            Get["/registration/{Value:int}"] = parameters => 
            {
                return View["registration"];
            };
        }
    }
}
