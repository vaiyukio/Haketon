using Haketon.Models;
using Nancy;

namespace Haketon
{


    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };

            Get["/verify"] = parameters =>
            {
                return View["verify"];
            };
        }
    }
}
