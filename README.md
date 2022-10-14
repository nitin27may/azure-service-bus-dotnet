
# Azure Service Bus Example
Azure Service bus boilerplate. 
## Important

Update the Enviornment variables in appsettings.

First object is related to Service Bus connection and 'MailSettings' is used to send emails.

```
"ConnectionStrings": {
    "ServiceBus": "",
    "QueueName": ""
  },
  "MailSettings": {
    "Mail": "",
    "DisplayName": "",
    "Password": "",
    "Host": "smtp.gmail.com",
    "Port": 587
  }
```

## Next Todo

Next development

* [x] Implement Service Bus Queue
* [x] Swagger for API Documentation
* [ ] Implement Topics
* [ ] Implment Consumer as separate project
* [ ] Implement Sender and Consumer as part of Azure Functions

## Build with

Describes which version .

| Name       | Version  |
| ---------- | -------- |
| .Net     | v6.x     |
| Swagger | v6.4.0 |

## Objective

* Working Sample for Service Bus Queue
* Working Sample for Service Bus Topic
* Get hands on the Messaging Queue

<!-- ## Features

* Dotnet 6.0 -->

