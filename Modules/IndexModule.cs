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
<<<<<<< HEAD

           
=======
            Get["/verify"] = parameters =>
            {
                return View["verify"];
            };
>>>>>>> bd3b6281c3421b8cda4d7feb2f5aec2f6e0c65bd
        }
    }
}
