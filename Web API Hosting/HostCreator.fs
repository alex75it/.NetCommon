namespace Alex75.Common.WebApiHosting 

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Mvc

 // https://dev.to/samueleresca/build-web-service-using-f-and-aspnet-core-52l8

type HostCreator () =        

    static member CreateApiHost(url:string, services:IServiceCollection) =

        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder
                    .UseUrls(url)
                    .UseStartup<Startup>()
                    
                |> ignore
            )
            .Build().Start()

