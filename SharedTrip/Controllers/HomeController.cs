
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace SharedTrip.Controllers
{

    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            if (User.IsAuthenticated)
                return Redirect("/Trips/All");

            return this.View();
        }
    }
}