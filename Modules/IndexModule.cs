namespace Haketon
{
    using Nancy;

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
