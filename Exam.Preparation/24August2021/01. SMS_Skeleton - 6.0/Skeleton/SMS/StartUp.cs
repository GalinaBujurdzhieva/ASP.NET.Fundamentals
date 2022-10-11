namespace SMS
{
    using BasicWebServer.Server;
    using BasicWebServer.Server.Routing;
    using SMS.Contracts;
    using SMS.Data;
    using SMS.Data.Repo;
    using SMS.Services;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static async Task Main()
        {
            var server = new HttpServer(routes => routes
               .MapControllers()
               .MapStaticFiles());

           
            server.ServiceCollection
                .Add<SMSDbContext>()
                .Add<IRepository, Repository>()
                .Add<IUserService, UserService>()
                .Add<IValidationService, ValidationService>()
                .Add<IProductService, ProductService>()
                .Add<ICartService, CartService>();
            
            await server.Start();
        }
    }
}