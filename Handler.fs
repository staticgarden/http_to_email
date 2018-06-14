namespace AwsDotnetFsharp
open Amazon.Lambda.Core
open Amazon.Lambda.APIGatewayEvents
open FSharp.Data
open Amazon.SimpleEmail
open Amazon.SimpleEmail.Model

[<assembly:LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
do ()


type EmailRequest = JsonProvider<"""
{
  "subject": "Awesome email subject",
  "body": "Awesome Email Body"
}
""">


type Email = {
  From: string
  To: string list
  Subject: string
  TextBody: string
}

module Emailer =
  [<Literal>]
  let Sender = "notifier@mail.staticgarden.com"

  let Send(email: Email) =
      use client = new AmazonSimpleEmailServiceClient()
      let emailReq = SendEmailRequest()
      emailReq.Source <- Sender
      let dest = new Destination()
      (List.iter dest.ToAddresses.Add, email.To) |> ignore
      emailReq.Destination <- dest
      emailReq.Message <- Message(Content(email.Subject), Body(Content(email.TextBody)))
      let resp = client.SendEmailAsync emailReq
      resp.Wait()

module Handler =

    let hello(request: APIGatewayProxyRequest) =
      let emailReq = EmailRequest.Parse(request.Body)

      { From = Emailer.Sender
        To = ["minhajuddink@gmail.com"]
        Subject = emailReq.Subject
        TextBody = emailReq.Body
      } |> Emailer.Send |> ignore
      APIGatewayProxyResponse(StatusCode=200, Body = "{}", Headers = Map[("content-type", "application/json")])
