using Haketon.Models;
using Nancy;

namespace Haketon
{


    public class IndexModule : NancyModule
    {
        public IndexModule(IDBContext dbContext)
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };

           
        }
    }
}