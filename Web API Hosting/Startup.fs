namespace Alex75.Common.WebApiHosting 

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting

type private Startup () =

    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddControllers() |> ignore     // add the service able to create API controllers. Use "fun opt -> opt" to configure it
        //services.AddMvc()                     // what is this doing ?
        services.AddCors()                      // required to create custom policy
        |> ignore
        

    member this.Configure(app:IApplicationBuilder, env:IWebHostEnvironment ) =
    
        app.UseRouting() |> ignore
        //app.UseAuthorization();

        //app.UseCors("local_and_production");
                
        //if env.IsDevelopment() then
        //    let buildPolicy = fun (builder:CorsPolicyBuilder) -> 
        //        (
        //            // null is required for static HTML files
        //            builder.WithOrigins([|"http://localhost"; "https://localhost"; "null"|]) 
        //                .AllowAnyMethod()
        //                .AllowAnyHeader()
        //                .Build()
        //        ) |> ignore
        //    app.UseCors(buildPolicy) |> ignore            
        

        app.UseEndpoints( fun endpoints ->

            // API
            endpoints.MapControllers() |> ignore 

            // SignalR
            //endpoints.MapHub<PingHub>("/ping") |> ignore 
        ) |> ignore