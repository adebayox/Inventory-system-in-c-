# Inventory System built using C#/.Net

# API Collection Description

This API collection provides endpoints for managing the movement of laptops in an inventory system. 
It offers various operations such as retrieving, adding, updating, and deleting laptops, 
as well as generating reports.

Each Assigned laptop has a model of serial number, category, assigned to, previous owner.

The endpoints are as follows:

---

## API Endpoints

- **GET /api/Laptop/GetAll**: Retrieve all laptops.
- **DELETE /api/Laptop/{id}**: Delete a specific laptop by ID.
- **GET /api/Laptop/{id}**: Retrieve details of a specific laptop by ID.
- **POST /api/Laptop/assign-laptop**: Create a new laptop.
- **PUT /api/Laptop/edit-assigned-laptop**: Update an existing laptop.
- **GET /api/Laptop/GetReport?startDate&endDate**: Retrieves a report containing information.
- **GET /api/Laptop/GenerateReport?startDate&endDate**: Generates and downloads a report in Excel format

    

---

Please refer to the individual API endpoints for more detailed information on their usage, request parameters, and response formats.

## GET http://localhost:5210/api/Laptop/GetAll
```
[http://localhost:5149/api/Appointment/GetAll](http://localhost:5210/api/Laptop/GetAll)
```

Description: Retrieves a list of all laptops..

Method: GET

Returns: A list of all laptops.

## POST http://localhost:5210/api/Laptop/assign-laptop
```
[http://localhost:5149/api/Appointment](http://localhost:5210/api/Laptop/assign-laptop)
```

Description: Assigns a laptop to a user.

Method: POST

Request Body: Contains the necessary data to assign the laptop (e.g., user information, laptop details).
```
{
  "serialNumber": "n134",
  "laptopModel": "smod3",
  "category": "Claims",
  "assigned_To": "busola",
  "previous_Owner": "odalo",
  "createdAt": "2023-05-24T08:58:45.725Z"
}
```

## GET http://localhost:5210/api/Laptop/{id}
```
http://localhost:5210/api/Laptop/{id}
```

Description: Retrieves detailed information about a specific laptop based on its serial number.

Method: GET

Route Parameter: serialnumber (string) - The serial number of the laptop to retrieve.

## PUT http://localhost:5210/api/Laptop/edit-assigned-laptop
```
[http://localhost:5149/api/Appointment](http://localhost:5210/api/Laptop/edit-assigned-laptop)
```

Description: Updates the details of an assigned laptop.

Method: PUT

Request Body: Contains the updated information for the assigned laptop.
```
{
  "serialNumber": "n134",
  "laptopModel": "smod3",
  "category": "Claims",
  "assigned_To": "busola",
  "previous_Owner": "yinka",
  "createdAt": "2023-05-24T08:58:45.725Z"
}
```

## DELETE http://localhost:5210/api/Laptop/{id}
```
http://localhost:5210/api/Laptop/{id}
```

Description: Deletes a specific laptop based on its serial number.

Method: DELETE

## GET http://localhost:5210/api/Laptop/GetReport?startDate&endDate
```
http://localhost:5210/api/Laptop/GetReport?startDate&endDate
```
Description: Retrieves a report containing information about all laptops.

Method: GET
```
Query Params
startDate
2023/1/01

endDate
2023/12/31
```

## GET http://localhost:5210/api/Laptop/GenerateReport?startDate&endDate
```
http://localhost:5210/api/Laptop/GenerateReport?startDate&endDate
```
Description: Generates and downloads a report in Excel format, containing information about all laptops.

Method: GET
```
Query Params
startDate
2023/1/01

endDate
2023/12/31
```
