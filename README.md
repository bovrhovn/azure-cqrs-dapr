# Demos about using CQRS and Dapr respectively

CQRS and Dapr demos, focusing on doing classic application and then moving to specific patterns:
1. Moving to [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
2. Moving to [Dapr](https://dapr.io/)
3. using both together - CQRS and DAPR - with respective deployment options

Appplication is about web application for citizens, focusing on general/tourist information and admin part, where you can get info about citizen services (water information, electricity, etc.)

## Structure

Application is built into 2 parts:
1. web information 
2. portal citizen admin information

Data will be stored in [SQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) database.

Both this applications are built with an option to be hosted in (with an option to leverage):
1. local system with
    1. web server in front
    2. container hosting with [Docker](https://www.docker.com/) engine
    3. event driven via Redis
    4. storage in SQL database
2. cloud deployment on [Azure](https://azure.com)
    1. [Azure App Service](https://azure.microsoft.com/en-us/services/app-service/)
    2. [Azure Container Instance](https://azure.microsoft.com/en-us/services/container-instances/)
    3. [Azure Kubernetes Service](https://azure.microsoft.com/en-us/services/kubernetes-service/)
    4. [Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/)
    5. [Azure KeyVault](https://docs.microsoft.com/en-us/azure/key-vault/general/overview)
    6. [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview)
    7. [Azure Storage](https://azure.microsoft.com/en-us/services/storage/)
    8. [Azure Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/overview)
    9. [Azure SQL](https://azure.microsoft.com/en-us/services/sql-database/)
    10. [Azure SignalR](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview)
    11. [SendGrid](https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email)

All of this services will be used as a part of building a responsive application with Azure behind the scenes.

## How to run the application

In order to run the application, you will need to have few things installed:
1. [.NET Core](https://dot.net)
2. [Docker](https://docker.com)
3. [SQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) - you can use [SQL in docker](https://hub.docker.com/_/microsoft-mssql-server) as well
4. [Dapr](https://docs.dapr.io/getting-started/install-dapr-selfhost/)

To see check the code, you can use [Visual Studio Code](https://code.microsoft.com) or any IDE that support .NET development ([Visual Studio](https://visualstudio.com), [JetBrains Rider](https://www.jetbrains.com/rider/), ...).

To run the code, you can go into **src** folder, open the solution and run the solution via [dotnet run](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run) commmand.

# Credits

Solution uses:
1. [Spectre.Console](https://github.com/spectreconsole/spectre.console)
2. [FontAwesome](https://fontawesome.com/)
