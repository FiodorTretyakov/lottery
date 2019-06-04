# Lottery

Lottery REST Api allows create, amend and check the results of the lottery tickets.

## Specification

REST API interface allows:

* POST `ticket` creates ticket;
* GET `ticket` get all tickets;
* GET `ticket/{id}` get the ticket by id;
* PUT `ticket/{id}` change ticket by id;
* PUT `status/{id}` check the ticket result;

## Configuration and Usage

Solution wrote on .NET Core 2.2. To run and debug it locally you need:

* download and install .NET Core SDK 2.2: <https://dotnet.microsoft.com/download>, it is cross-platform, and for the Mac you can install the latest one version, the solution doesn't require Visual Studio for Mac;
* install self-signed certificate for .NET Core by running `dotnet dev-certs https --trust` from base directory;
* if you want use console, you should:
  * to run: run console command `dotnet run -p Lottery/Lottery.csproj` from the base folder;
  * to test: run console command `dotnet test /p:CollectCoverage=true /p:Exclude="[*]Lottery.Migrations.*"`, it will run tests and show the code coverage;
* if you want sue vscode, you should:
  * download and install it from: <https://code.visualstudio.com/Download>;
  * to run: Go to Launch -> Debug Run configuration;
  * to debug: Go to Tasks -> Run test watch;

## Technical decisions

* .NET Core, because of it is cross-platform, powerful and high performing.
* The Web Api based application is ideally suits to the task: REST Api;
* The Visual Studio code, because of it is lightweight, cross-platform, powerful IDE, with a lot of plugins where I can find everything I need;
* Restlet Client to test REST Api. It has very intuitive UI and works directly from browser (supports offline mode);
* git, because of it is easy work with versions, approaches, allow trace history;
* efcore, because of it is the quickest native way to create scaffolded database with minimum code;

## Design decisions and trade-offs

* Relationship database, because of I want provide consistency and validation at the lowest level - database.
* Sqlite, because of it is simple to use and because of SqlServer doesn't works out-of-the-box on Mac and Linux, and as it is not production version, only Demo, concurrency issue is not a problem. For the production, I'd better use PostgreSql.
* The attributic approach to models: I use attributes for DB mapping and for the Web Api serialization.
* Models I have: **Ticket** that contains set of **Line**. I decided to don't create Number model (to much complexity) and use JSON serialization for the DB, but for the model, it deserializes from property.
* All my validation rules throw exceptions in models, but several checks I had to move to controllers to don't overload the models;
* Ticket can be displayed 2 ways: raw lines with numbers or computed result. I use serialization properties for that: `ShouldSerializeResult` and `ShouldSerializeNumbers`, that work out-of-the-box;
* For test I use 2 ways:
  * Unit tests for models in folder `ModelTest`;
  * Integration test that emulate my Api calls in `ApiTest` allow me to test the solution end-to-end. I use real Sqlite database, because of InMemory doesn't work with relations, in-memory Sqlite should hold the connection, so I just need to cleanup the data after each test.
