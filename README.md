# tokenapp
Tech stack - .Net core API, React, Mobx, Axios, semantic ui, Formik, yup validation, xunit, Moq, Sqlite

This is a simple web app for managing the api tokens.

Application Layers
-------------------
Tokenapp (react client)

API --> Domain
        Persistence
        
Migrations
-----------
Application come with sqlite db, no need to apply migrations if using sqlite
To change to SQL Server

1. remove existing migrations by running dotnet ef migrations remove -p Persistence -s API 
2. Change the ApplicationServiceExtensions.cs to use SQL server
        services.AddDbContext<DataContext>(options =>
                options.UseServer(configuration.GetConnectionString("DefaultConnection"))
            );
3. Change the connection string in appsetting.development.jsin
4. Add new migrations dotnet ef migrations add sqliteinitial -p Persistence -s API  
        
Build --> ensure nuget packages are updated
----------
1. cd token app --> npm start will start client in browser. alternatevely npm build will build and send the output to api/wwwroot folder
2. cd API --> dotnet run will build the API project
        

