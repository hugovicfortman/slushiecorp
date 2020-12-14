# SlushieCorp

SlushieCorp is a goofy real-time "slushie" vending application built with C# .netcore 3.1 and Signal-R. The corresponding Angular 9 UI source is located in this repository: [slushiecorp-ui](https://github.com/hugovicfortman/slushiecorp-ui)

## How does it work?

Using Signal-R Hubs "SlushieHub" class, the application is able to update all subscribed clients immediately a transaction change occurs, maintaining real time status and operations across several machines.

## How to run?
From your terminal/command prompt, clone the repository using the following command

    git clone https://github.com/hugovicfortman/slushiecorp.git

next, run the following 

    cd slushiecorp
    dotnet run

The project will start to run with the Angular 9 UI prebuilt into its `wwwroot` folder.

## Requirements
- dotnetcore 3.1 sdk
- any database server (MSSQL localdb is used by default)