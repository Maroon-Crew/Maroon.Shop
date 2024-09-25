# How to auto generate API clients

The API clients (Maroon.Shop.Api.Client) were generated using the Open API Kiota tool, using the following command line.

### Prerequisites

> :warning: Before you can use the Kiota client, you will need to install it.  

If you're using Visual Studio, you can do this from the "Developer PowerShell" window by running the following command line.

``` powershell
dotnet tool install --global Microsoft.OpenApi.Kiota
```

Alternatively, you can use one of the options from the [Kiota installation documentation](https://learn.microsoft.com/en-us/openapi/kiota/install?tabs=bash).

## Generating the clients

> :warning: Ensure your Web API is running before you generate clients.

You can generate the clients by running the following command line.

``` powershell
kiota generate -l CSharp -c MaroonClient -n Maroon.Shop.Api.Client -d https://localhost:7282/swagger/v1/swagger.json -o ./ --exclude-backward-compatible
```

Note the parameters

- `-l CSharp` generate C# clients
- `-c MaroonClient` the name of the main client class
- `-n Maroon.Shop.Api.Client` the namespace for the generate classes
- `-d https://localhost:7282/swagger/v1/swagger.json` the API feed to generate the classes from
- `-o ./` outputs the classes in the current directory (you will need to CD into the target directory, or change this directory to the desired directory)
- `--exclude-backward-compatible` this avoids emitting deprecated methods that maintain compatibility from older versions of Kiota

For a full reference of how to use the command line, see the [Kiota usage documentation](https://learn.microsoft.com/en-us/openapi/kiota/using).