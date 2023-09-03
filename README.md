This project should have installed packages:    
**Package name | version***    
a) Microsoft.EntityFrameworkCore | **6.0.15**    
b) Microsoft.EntityFrameworkCore.SqlServer | **6.0.15**    
c) Microsoft.EntityFrameworkCore.Tools | **6.0.15**    
d) Swashbuckle.AspNetCore | **6.2.3**    
e) Microsoft.AspNetCore.Authentication.JwtBearer | **6.0.15** 

*Project will work if you install packages in specific version. Otherwise project won't work.

How to run REST API?
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

**1)** You need to set up database, so:

**1.1)** Make sure you have installed **SQL Server Management Studio (SSMS)**   
--> https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

**1.2)** Open CMD in administrative rights

**1.3)** In terminal just type command: sqllocaldb create **"YourDatabaseServerName"**
![How to run](https://user-images.githubusercontent.com/90453529/226385105-f98e299e-32e4-403c-bc3b-2bd6f6b32ea7.png)

**1.4)** After that open SQL Server Managment Studio (SSMS), you will see window "Connect to Server". In this window you need to fill fields like that:     
-Server type: Database Engine (it is usually set as a default),     
-Server name: (LocalDB)\**YourDatabaseServerName** (YourDatabaseName is just database when you created in point 3),     
-Authentication: Windows Authentiocation (it is usually set as a default).    
![How to run2](https://user-images.githubusercontent.com/90453529/226385140-e9a56777-d7b0-4d6e-bb03-7ce706328b29.png)

_After filling up those fields just click "Connect" button._

**1.5)** Next step is creating database. Move your cursor on fold "Databases" in "Object Explorer". Right click on it and select "New Database".    
![How to run3](https://user-images.githubusercontent.com/90453529/226386718-3bb5658c-0d7a-4a0a-9f97-86b280f988c0.png)

**1.6)** You will see window with database property. In field "Database name" type your database name, if you want you can change location of databse. Then click "Ok" button.    
![How to run4](https://user-images.githubusercontent.com/90453529/226386765-1e1c3b69-c0ce-4299-92e3-607231f3d0f5.png)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

**2)** You need to connect to database via connection string in Visual Studio, so:

**2.1)** Make sure you have installed **Visual Studio Community 2022**    
 --> https://visualstudio.microsoft.com/vs/

**2.2)** Open REST API project in Visual Studio   

**2.3)** In search field in top of the visual studio type: "Server Explorer". When you click on "Server Explorer", on the left side should appear fold.
![How to run5](https://user-images.githubusercontent.com/90453529/226387108-d2a82721-031d-4de7-a936-565c1ac5abe7.png)

**2.4)** Next you need to click "Connect to Database"    
![How to run6](https://user-images.githubusercontent.com/90453529/226387164-cfc5d7cc-7106-489b-bcc4-f322e23d9326.png)

**2.5)** In new window you need select database source. In this case select "Microsoft SQL Server" then click button "Continue"   
![How to run7](https://user-images.githubusercontent.com/90453529/226387208-1cf66d5f-73a7-47ac-a47a-83719fb9a970.png)

**2.6)** You will see new window with "Add Connection" and fields:   
-Data source: Microsoft SQL Server (SqlClient) (leave it with default value),   
-Server name: (LocalDB)\YourDatabaseServerName (exactly the same server name from point 1.4),    
-Authentication: Windows Authentication (leave it with default value),    
-Select or enret a database name: YourDatabaseName (exactly the same server name from point 1.6),     
![How to run8](https://user-images.githubusercontent.com/90453529/226387302-14e7182c-b179-4ba0-8167-feec5ae54f59.png)

**_If you don't know what is your database server name just open SQL Server Management Studio, connect to your database and in "Object Explorer" right click on your database. Select "properties", then "View connection properties"_
![How to run-additional](https://user-images.githubusercontent.com/90453529/226387351-6a7a7f64-9b5f-4461-b6fc-01384b5f9f20.png)

**2.7)** After that you will see under "Data Connections", a new connection. When you click on it in property in right pane, go and find position "Connection String". Save value of this property to notepad or somewhere else.
![How to run9](https://user-images.githubusercontent.com/90453529/226387438-d3df8834-3408-4dfd-881b-9b830f55f8aa.png)

**2.8)** Now, right click on project structure in "Solution Explorer", then select "Manage User Secrets".
![How to run10](https://user-images.githubusercontent.com/90453529/226387488-60a49a65-f6c9-46dd-97d5-9cc0fe444124.png)

**2.9)** Finally, copy content of "appsettings.json", then paste into "secrets.json". In field "Default" paste your saver connection string from point 2.7. **Don't forget to fill up all empty fields**.   
![image](https://github.com/ZiumC/Obsluga_Restauracji_REST_API/assets/90453529/030b7bc0-3037-4041-9270-f2d90678bb5e)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

**3)** Running project    

**3.1)** In search field in top of the visual studio type: "Package Manager Console". When you click on first position Package Console would appear.
![How to run12](https://user-images.githubusercontent.com/90453529/226389524-f3d1a09f-97b6-4083-94a8-08310c3760d2.png)

**3.2)** In Package Console just type: "update-database". If you are connected to your database, this command should generate all tables and insert data into new database.    
![How to run13](https://user-images.githubusercontent.com/90453529/226390347-86494ce7-a755-4812-8c75-34b422605caa.png)



