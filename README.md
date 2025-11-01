🧩 Play Microservices Architecture

This project is built using a microservices architecture with RabbitMQ and MassTransit for asynchronous communication between services.
Each service is designed to be independent, scalable, and maintainable, following clean code and separation of concerns principles.

🚀 Microservices Overview
1️⃣ Play.CatalogService

Databases: MongoDB

Message Broker: RabbitMQ

Responsible for managing catalog items (products).

Publishes the following events:

CatalogCreated

CatalogItemUpdated

CatalogItemDeleted

These events are consumed by other services (e.g., Play.Inventory) to keep data in sync.

2️⃣ Play.Inventory

Databases: MongoDB

Message Broker: RabbitMQ

Listens to events from Play.CatalogService and updates its own local database accordingly.

Stores a subset of catalog data (Id, Name, Price) inside the InventoryItems collection.

This allows inventory-related operations to run without directly querying the catalog service.

Events Consumed:

CatalogCreated

CatalogItemUpdated

CatalogItemDeleted

Events Published:

InventoryCreated — Triggered when user inventory items are added or updated.
This event is consumed by the Play.OrderService.

3️⃣ Play.OrderService

Databases: MongoDB and SQL Server

Message Broker: RabbitMQ

Consumes the InventoryCreated event published by Play.Inventory.

Maintains a UserInventory (MongoDB) for quick access to user items.

During order placement, it uses the data from UserInventory and persists final order details in SQL Server (UserOrders table).

4️⃣ Play.Infra

Contains Docker Compose configurations for running essential containers:

SQL Server

MongoDB

RabbitMQ

This ensures all dependent services can run locally or in any environment with a single command.

5️⃣ Play.Common

Contains shared components and extension methods for:

Common configurations (e.g., MassTransit setup, MongoDB/SQL context)

Reusable and maintainable helper methods

Abstractions for scalable architecture

⚙️ Message Flow Summary
graph TD
    A[Play.CatalogService] -- CatalogCreated/Updated/Deleted --> B[Play.Inventory]
    B -- InventoryCreated --> C[Play.OrderService]
    C --> D[SQL Server (Orders)]
    B --> E[MongoDB (InventoryItems)]
    C --> F[MongoDB (UserInventory)]

🧠 Key Functionalities

Event-Driven Communication:
Microservices communicate asynchronously via RabbitMQ using MassTransit.

Polyglot Persistence:
Combines MongoDB (for flexibility and speed) and SQL Server (for relational data and transactions).

Decoupled Services:
Each service owns its own data, improving scalability and maintainability.

Resilient Data Flow:
Inventory and order data are synchronized via domain events, ensuring consistency across services.

Scalable Infrastructure:
Managed through Docker Compose with separate containers for each dependency.
