# OrganisationStructure
Structure of organisation

## Prerequisites
Before you begin, make sure your development environment includes dotnet core SDK v3.0+.

Used NuGet packages:
"jsTree" Version="3.1.1.2" 
"Microsoft.AspNetCore.Authentication.Negotiate" Version="6.0.11"
"Microsoft.EntityFrameworkCore" Version="7.0.2" 
"Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.2"
"Microsoft.EntityFrameworkCore.Tools" Version="7.0.2"
"Newtonsoft.Json" Version="13.0.2" 
"NonFactors.Grid.Mvc6" Version="7.1.0" 


## Build
Restore Data base from backup file "OrganisationHierarchy.bak", the backup is in folder Utills in project.
Create user in SQL server: "Web_api"/"password".
Then use that in ConnectionStrings, where change please Data source, login and password.
After running, there would be 3 navigation buttons, First - "Home"(To return on the main page), "Second" - Organisation(Tree hierarchy of employees), 
Third - "Employee"(Which you can use to see table of employees, and do some action with them).

