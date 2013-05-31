using System;
using System.Web;
using Funq;
using ServiceStack.WebHost.Endpoints;
using PrimeiroTesteComServiceStack.WebService;
using PrimeiroTesteComServiceStack.Rest;

namespace PrimeiroTesteComServiceStack
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("ServiceStack makes services easy!", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            Routes
              .Add<Hello>("/hello")
              .Add<Hello>("/hello/{Name}");

            Routes
              .Add<Movie>("/movies", "POST,PUT,DELETE")
              .Add<Movie>("/movies/{Id}")
              .Add<Movies>("/movies")
              .Add<Movies>("/movies/genres/{Genre}");
        
        }
    }

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
    }
}
