using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrimeiroTesteComServiceStack.WebService
{
    // DTO
    public class Hello
    {
        public string Name { get; set; }
    }

    // Response
    public class HelloResponse
    {
        public string Result { get; set; }
    }

    // Service
    public class HelloService : IService<Hello>
    {
        public object Execute(Hello request)
        {
            return new HelloResponse { Result = "Olá, " + request.Name };
        }
    }
}