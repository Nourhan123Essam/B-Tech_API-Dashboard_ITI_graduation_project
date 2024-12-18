# 🛒 E-Commerce Backend API

## 📖 Overview

Welcome to the **E-Commerce Backend API** repository! This project is divided into two main components:  
1. **Admin Dashboard**: Built using **ASP.NET MVC** for managing administrative functionality.  
2. **Public Website Backend**: Developed as an **ASP.NET Web API** to handle public-facing operations.  

### 📌 Note:
This documentation highlights **only the parts of the project** that I have contributed to. While I played a key role in several areas (outlined below), other contributors worked on additional features not detailed here. I hope this README offers clear insight into the areas I focused on. 😊

---
## 🔗 Related Repository

The frontend for this project is built in **Angular** with responsive UI using **Bootstrap**. Check it out [here](https://github.com/Nourhan123Essam/B-Tech_public_website_ITI_graduation_project).

---

## 🏗️ Project Structure

### 1. **Admin Dashboard**  
The **Admin Dashboard** provides an intuitive interface for administrators to manage orders, view order details, and perform various actions like updating, canceling, or deleting orders.

#### 🔧 My Contributions:
- Implemented **CRUD operations** for the **Order** entity in the **MVC** project.  
- Ensured smooth integration with the Onion Architecture layers for maintainability and scalability.  

---

### 2. **Public Website Backend**  
The **Public Website Backend** is a RESTful API that handles key public-facing functionalities, including order management, cart operations, and payment processing.  

#### 🔧 My Contributions:
1. **Order and OrderItem Management**  
   - Implemented **CRUD operations** for the **Order** and **OrderItem** entities in the `OrderController`.  
     - Each order contains a collection of order items, with foreign key relationships linking the `Order` to its `OrderItems` and the `OrderItems` to their associated products.
     - Ensured smooth handling of the order lifecycle, from creation to updates and deletions.

2. **Cart Operations**  
   - Developed cart management logic within the `OrderController` to enable seamless cart functionality.  
     - The cart is represented as an **Order** with a status of `InCart`.  
     - Users can view their cart contents from any browser, making use of persistent storage.

3. **PayPal Integration**  
   - Integrated **PayPal** payment functionality via a dedicated `PayPalController`.  
     - Streamlined payment workflows by enabling secure order payment processing with PayPal's API.

---

### 🧅 Onion Architecture Breakdown  

This project follows **Onion Architecture** to promote separation of concerns, maintainability, and scalability.

#### 1. **DbContextB** (Database Access)
   - Provides the entry point to interact with the database using **Entity Framework Core**.
   - Contains the **BTechDbContext** that manages the connection to the database and tracks entities like Order, OrderItem, etc.

#### 2. **DTOsB** (Data Transfer Objects)
   - Contains the **DTOs** (Data Transfer Objects) used to transfer data between Services in Application layer and Controllers in MVC, and API layers.
   - Examples include SelectOrderBDTO, AddOrUpdateOrderBDTO.

#### 3. **InfrastructureB** (Database Interaction)
   - Contains the implementation for accessing and interacting with the database.
   - Implements the **Repository Patterns** for CRUD operations.

#### 4. **ModelsB** (Entities)
   - Defines the core **domain models** used throughout the system.
   - Includes entities like Order, OrderItem.

#### 5. **ApplicationB** (Business Logic)
   - Encapsulates the business logic of the application.
   - Implements the service layer interacting with the **InfrastructureB** to fetch or persist data, and uses **DTOsB** to manage data flow between the controllers and the core application.

![Structure](https://github.com/Nourhan123Essam/B-Tech_API-Dashboard_ITI_graduation_project/blob/main/Structure%20on%20Visual%20Studio.png)

---

## ✨ Features  

### 🔹 **Order Management**
 **Finish Order**: Completes the payment process and finalizes the order.
- **Update Order Item Quantity**: Allows the user to update the quantity of an item in their cart.
- **Cancel Order**: Allows the user to cancel an order (if not shipped yet).
- **Delete Order Item**: Deletes an item from the order.
- **Add Order Item to Cart**: Adds an item to the user's cart and associates it with an order if it exists. Otherwise, a new order is created.
- **View Order**: Displays the items currently in the cart.
- **User Orders**: Displays a user's completed orders (excluding those marked as "inCart").

### 🔹 **PayPal Integration**
- **Create Order**: Initiates a PayPal order with the given amount and returns the order ID for processing.
- **Capture Order**: Finalizes the PayPal order after payment is completed.

![API](https://github.com/Nourhan123Essam/B-Tech_API-Dashboard_ITI_graduation_project/blob/main/Order%26Payment%20Ui%20Swagger.png)

---

## 🛠️ Technologies Used  

- **ASP.NET Core Web API** for the backend logic.  
- **ASP.NET MVC** for the admin dashboard.  
- **Entity Framework Core** for database interaction.  
- **PayPal SDK** for secure payment processing.  
- **Onion Architecture** for structured and maintainable code.  
- **AutoMapper** for seamless object mapping.  
- **LINQ** for efficient querying.

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
