# DevelopersApplication

## Description
This is a project to manage the experts in programming languages, programming languages, and careers associated with it. 
A person who is interested in coding can see all the programming languages, leading experts of the field, and the careers related to the language. 
An admin can update, delete, and add the different languages, experts, and the careers.
A programmer can also enter their information into the website.
This project also uploads images for the coders
## Running this project
* Make sure there is an App_Data folder in the project (Right click solution > View in File Explorer)
* Tools > Nuget Package Manager > Package Manage Console > Update-Database
* Check that the database is created using (View > SQL Server Object Explorer > MSSQLLocalDb > ..)
* Run the website and navigate to different links
* Make sure to utilize jsondata/coder.json to formulate data you wish to send as part of the POST requests. {id} should be replaced with the coder's primary key ID. The port number may not always be the same if you run into any issues
 https://localhost:44384/coder/list

## Running the Views for List, Details, New
Click on the appropriate links to view the list of coders ,languages and careers and all the details

## ScreenShots of the project
![Homepage](/Images/homepage.png)
![List of coders](/Images/coderlist.png)
![Coder Details](/Images/coderdetails.png)
