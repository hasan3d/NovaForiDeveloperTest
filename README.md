# NovaFori Developer Test - Todolist App

![Todolist App Demo](https://github.com/hasan3d/NovaForiDeveloperTest/blob/master/TodoListAppDemo.gif)

## How long did you spend on your solution?
### Answer: I spent three and a half hours approximately.
## How do you build and run your solution?
#### Answer: I have separated the FrontEnd and BackEnd sections of the solution into two different folders. The FrontEnd folder has a README file which explains how to build and run the project. The Backend folder has a README file describing how to build and run the project. You must run the BackEnd Web API project first, then run the FrontEnd project. The FrontEnd project has ```.env``` file, which contains the BackEnd Web API URL. i.e       ```VUE_APP_TODOLIST_API_URL=https://localhost:7116```. 
## What technical and functional assumptions did you make when implementing your solution?
#### Answer: I carefully analysed the technical requirements and user stories before writing any code. With the technical specifications in mind, I have built a RESTful API using ASP.NET Core 6 and added the appropriate Unit Test. I am also persisting the data in a SQL database. I created the FrontEnd solution with Vue.js and Typescript. By analysing the user stories, I created the TodoList app, where a user can see a list of all to-dos. Users can create a new task that gets added to the to-do list as a pending status. The user can change the to-do list existing task status from pending to completed or completed to pending.
## Briefly explain your technical design and why you think it is the best approach to this problem.
#### Answer: I designed the Backend solution with the Repository and Unit Of Work Design Pattern. With the Repository design pattern, the Backend solution is cleaner, and It helps reduces duplicate queries and de-couples the application from the Data Access Layer. As per the Repository pattern, the Backend solution contains the Domain project, which holds Entities and Interfaces, including the IGenericRepository and DataAccess.EFCore project uses Entity Framework Core code first approach to interact with the database. I am using the Unit Of Work Design Pattern to commit the data to the database. As the to-do list application would require performing CRUD operations, it is a good candidate for the Repository Design Pattern. For the FrontEnd, I created the solution with the Vue CLI. I used Vue 3 with the Composition API to write components. The solution also uses Typescript.


