namespace AwsDotnetFsharp
open Amazon.Lambda.Core
open Amazon.Lambda.APIGatewayEvents
open FSharp.Data

[<assembly:LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
do ()

type EmailRequest = JsonProvider<"""
{
  "subject": "Awesome email subject",
  "body": "Awesome Email Body"
}
""">

module Handler =
    open System
    open System.IO
    open System.Text

    let hello(request: APIGatewayProxyRequest) =
      let emailReq = EmailRequest.Parse(request.Body)
      APIGatewayProxyResponse(StatusCode=200, Body = (sprintf ">> %A" emailReq))
