namespace SharedTrip
{
    using System.Threading.Tasks;

    using MyWebServer;
    using MyWebServer.Controllers;

    using Controllers;
    using MyWebServer.Results.Views;
    using SharedTrip.Data;
    using SharedTrip.Data.Common;
    using SharedTrip.Common;
    using SharedTrip.Contracts;
    using SharedTrip.Services;

    public class Startup
    {
        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<ApplicationDbContext>()
                    .Add<IRepository, Repository>()
                    .Add<IValidator, Validator>()
                    .Add<IUserService, UserService>()
                    .Add<ITripService, TripService>()
                    .Add<IViewEngine, CompilationViewEngine>())
                .Start();
    }
}
