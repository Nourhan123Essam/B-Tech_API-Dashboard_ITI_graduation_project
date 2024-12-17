# E-Commerce Backend API

## Overview

This repository contains the backend for the E-Commerce project, divided into two main parts:

1. **Admin Dashboard**: Built using **ASP.NET MVC** for admin functionality.
2. **Public Website Backend**: Built as an **ASP.NET Web API** for handling public user interactions.

### Note:
The description below covers **only the parts of the project** that I have worked on. Other parts of the project not detailed here are handled by other contributors and might not be included in this specific backend repository section.

---

## Project Structure

### 1. **Admin Dashboard (AdminDashboardB)**

The **AdminDashboardB** is an **ASP.NET MVC** project providing administrative interfaces for managing orders, viewing order details, and controlling various order-related actions such as canceling or updating orders.

- **Controllers**: Handles user requests and performs business logic related to order management.
- **Views**: Provides an interactive user interface for managing orders.
- **Models**: Represents the data structures used for displaying data in the views.

### 2. **Public Website Backend (B-Tech.API)**

The **B-Tech.API** is an **ASP.NET Web API** project that handles public-facing operations like creating orders, updating cart items, and processing payments (e.g., PayPal integration).

- **Controllers**: Responsible for receiving HTTP requests and returning data in response, leveraging the business logic in the Application layer.
- **Models**: Defines the entities for the e-commerce application (e.g., `Order`, `OrderItem`, etc.).
- **DTOs**: The Data Transfer Objects used for sending and receiving data via API requests.

### **Onion Architecture Breakdown**

This project follows **Onion Architecture** to promote separation of concerns, maintainability, and scalability.

#### 1. **DbContextB** (Database Access)
   - Provides the entry point to interact with the database using **Entity Framework Core**.
   - Contains the **DbContext** that manages the connection to the database and tracks entities like `Order`, `OrderItem`, etc.

#### 2. **DTOsB** (Data Transfer Objects)
   - Contains the **DTOs** (Data Transfer Objects) used to transfer data between layers and over the API.
   - Examples include `OrderDTO`, `OrderItemDTO`.

#### 3. **InfrastructureB** (Database Interaction)
   - Contains the implementation for accessing and interacting with the database.
   - Implements the **Repository Patterns** for CRUD operations.

#### 4. **ModelsB** (Entities)
   - Defines the core **domain models** used throughout the system.
   - Includes entities like `Order`, `OrderItem`.

#### 5. **ApplicationB** (Business Logic)
   - Encapsulates the business logic of the application.
   - Implements the service layer interacting with the **InfrastructureB** to fetch or persist data, and uses **DTOsB** to manage data flow between the controllers and the core application.

### How the Components Interact

- **Controllers** in **AdminDashboardB** and **B-Tech.API** communicate with the **ApplicationB** layer to perform actions on the database, using the appropriate **DTOsB** for input and output.
- The **ApplicationB** layer handles business logic and works with the **InfrastructureB** to interact with the database via **DbContextB**.
- The **DbContextB** and **InfrastructureB** are agnostic to the UI layer, ensuring that the core logic remains independent of any presentation layer.

---

## Features

### Order Management and Cart Operations
- **Finish Order**: Completes the payment process and finalizes the order.
- **Update Order Item Quantity**: Allows the user to update the quantity of an item in their cart.
- **Cancel Order**: Allows the user to cancel an order (if not shipped yet).
- **Delete Order Item**: Deletes an item from the order.
- **Add Order Item to Cart**: Adds an item to the user's cart and associates it with an order if it exists. Otherwise, a new order is created.
- **View Order**: Displays the items currently in the cart.
- **User Orders**: Displays a user's completed orders (excluding those marked as "inCart").

### PayPal Integration
- **Create Order**: Initiates a PayPal order with the given amount and returns the order ID for processing.
- **Capture Order**: Finalizes the PayPal order after payment is completed.

![API](https://github.com/Nourhan123Essam/B-Tech_API-Dashboard_ITI_graduation_project/blob/main/Order%26Payment%20Ui%20Swagger.png)
---

## Technologies Used
- **ASP.NET Core Web API** for building the backend API.
- **ASP.NET MVC** for the admin dashboard interface.
- **Entity Framework Core** for database interaction and ORM.
- **AutoMapper** for object-to-object mapping (e.g., mapping between domain models and DTOs).
- **LINQ** for querying the database.
- **PayPal SDK** for payment processing.
- **Dependency Injection** for managing services and dependencies.
- **Onion Architecture** for clear separation of concerns.

---

## 💡 **How to Run**

### Steps:
1. Clone the repository:  
   
    ```bash
     git clone https://github.com/Nourhan123Essam/B-Tech_API-Dashboard_ITI_graduation_project.git

2. Update the appsettings.json with your SQL Server connection string in this format:
  in the appsetting.json put your database name, and server name in the DefaultConnection:
    
    ```appsetting.json
    "ConnectionStrings": {
    "DefaultConnection": "Server=your-server-name;Database=your-database-name;Trusted_Connection=True;TrustServerCertificate=True"
    }
3.Apply Migrations
 - Open the terminal and navigate to the project folder where the .csproj file is located.
 - Run the following command to create the database and apply migrations:
    ```bash
      dotnet ef database update
  
4. Open the the folder using Visual Studio then run the project

## 📬 Let's Connect
- [LinkedIn](https://www.linkedin.com/in/nourhan-essam123/)  
- [LeetCode](https://leetcode.com/u/norhan123/)  
- [GitHub](https://github.com/Nourhan123Essam)
- [Gmail](nourhan.essam.makhlouf@gmail.com)
---
