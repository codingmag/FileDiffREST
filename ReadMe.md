Choices and Assumptions Made:
* Solution structure was designed in a way so that optimizing lousely coupling therefore maximizing testability and scalability.
* For optimizing testability, Unity as an IoC container and Entity Framework as an implementation of UoW and Repository patterns were used.
* For increasing control over model files and DB schema, making it easier for DB generation and population and therefore helping continous integration code-first approach was followed in EF.
* For following the best practice as providing async methods in REST APIs thus having non-blocking thread calls and prodiving consumers of the API a fluent UI experience Web API2 with async method implementations was used.
* In order to eliminate deadlocks in the execution and following the best practice as sticking to the async pattern in the rest of the application as well as the front-end, async methods were used in both service layer and EF.
* Altough there was no significant requirement for creating mapping code, AutoMapper was used assuming the application might be a product of the company and therefore has a potential for growth.
* For mocking the DBContext and services Moq was used.
* LocalDB was used for storing the data during testing. 

Setup & Testing
* Depending on the environment, DB connection strings may need to be modified in FileDiffREST.Web and FileDiffREST.Web.Tests projects in order for both the web application and the integration tests work correctly. (Please note that app.config in FileDiffREST.Web.Tests was configured so that the DB is dropped and re-created in each run while the DB creation behaviour was left as default in the web project.)
* Solution contains unit tests that do not require to be run in custom order and integration tests that are ordered under FileDiffControllerIntegrationTests.orderedtest.
* IMPORTANT! It is normal behaviour that 3 of the integration tests fail while running the tests of the solution as the integration tests need to be run in the specified order. That order is the same as the usage case stated in the assignment description that was sent to me as an e-mail. In order to run the ordered integrated tests, launch "Test Explorer", right click on filediffcontrollerintegrationtests and select "Run selected tests".
* Web api of the solution was further tested via SoapUI and it was confirmed that the given test case works (Please note that DB is not re-generated in each run, therefore differences may occur in the second run of a scenario to be manually executed over Soap UI). 
