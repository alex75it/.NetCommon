namespace Alex75.Common.WebApi

open Microsoft.AspNetCore.Mvc

[<ApiController; Route("api/[controller]")>]
type PingController () =
    inherit ControllerBase()

    [<HttpGet; Route("")>]
    member this.Ping() : IActionResult = 
        match this.Request.ContentType with
        | "application/json" ->
            let result = {| data = "pong" |}  // Anonymous Record
            JsonResult( result ) :> IActionResult
        | _ -> this.Ok("pong") :> IActionResult
        
