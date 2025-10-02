# LuxuryPropertiesUsa
API to simulate an administration of properties in USA

**Developed with**:
- DDD Architecture
- Design Patterns (MediatR[CQRS], UnitOfWork, Repository)
- Best Practices based on some SOLID Principles
- Security with JWT
- UnitTest with nUnit and Moq
- Entity Framework (BD Sql Server)

**To keep in mind:**
- The DB that is used is local with Sql Server "**SQLEXPRESS**" and we worked with the user **developer**. The Database scripts are in the folder: **"FirstSteps/1. ScriptsDb/"**. 
  - **1-createUserForDb.sql** creates the user in the database.
  - **2-scriptsDb.sql** creates the tables and initial data.
- The folder **"FirstSteps/2. DataForTests/"** contains any examples for test the endpoints.
- All methods are comented for a clarity read.
- For security, to use the developed microservices a token must be generated first **(The Authentication method GET is used for these)**, then passed through the header using authentication bearer.
- This development is using the properties of the environment variables **(appsettings.json)**, so that in a future deployment a YML file is created and in it configure the secrets that are created in the cloud, for example that file should contain at least the db connection string:
~~~
    ...
    envFrom:
        - secretRef:
           name: main-db
~~~
  ... With this, for security the **"(appsettings.json)"** file should no longer contain such environment variables.
- The images must reach the API in BASE64 format, likewise in the query it is returned in the same format. In the Database it saves an Array of bytes. For functional tests you can use the WebSite "https://www.base64-image.de/" and upload an image to generate the Base64 format, note that the value to use must not contain the initial part "**data:image/jpeg;base64,**". For greater ease you can use the examples provided.
- The method that lists the properties was developed thinking of a view with a table with a search field, the indicated columns with the option of ordering click and also with a pagination.
- Unit tests were developed with NUnit to the Controllers, Handlers and Repositories.
