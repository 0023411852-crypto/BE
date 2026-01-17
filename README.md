# School Management API

RESTful API for managing Classes and Students using Clean Architecture and ASP.NET Core.

## 🏗️ Architecture

This project follows **Clean Architecture** (Layered Architecture) principles:

```
Domain/                  # Domain Layer
├── Entities/           # Business entities (Class, Student)
└── Interfaces/         # Repository interfaces

Application/            # Application Layer
└── Services/          # Business logic services

Infrastructure/         # Infrastructure Layer
├── Data/              # EF Core DbContext
└── Repositories/      # Repository implementations

Controllers/            # Presentation Layer
├── ClassesController  # Classes API endpoints
└── StudentsController # Students API endpoints
```

## 📊 Database Schema

**Classes Table:**
- Id (int, Primary Key)
- Name (string)
- Description (string)

**Students Table:**
- Id (int, Primary Key)
- FullName (string)
- Email (string)
- DateOfBirth (DateTime)
- ClassId (int, Foreign Key → Classes.Id)

## 🚀 Technologies

- **Framework:** ASP.NET Core 8.0
- **Database:** EF Core In-Memory Database
- **API Documentation:** Swagger/OpenAPI
- **Architecture:** Clean Architecture
- **Pattern:** Repository Pattern with Dependency Injection

## 📡 API Endpoints

### Classes API (`/api/classes`)
- `GET /api/classes` - Get all classes
- `GET /api/classes/{id}` - Get class by ID
- `POST /api/classes` - Create new class

### Students API (`/api/students`)
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

## ⚙️ Setup & Run

### Prerequisites
- .NET 8.0 SDK

### Installation

1. Clone the repository:
```bash
git clone https://github.com/YOUR_USERNAME/school-management-api.git
cd school-management-api
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

5. Open your browser and navigate to:
```
http://localhost:5000
```

The Swagger UI will automatically open for API testing.

## 🧪 Testing with Postman

A Postman Collection is included in the repository:
- **File:** `School_Management_API.postman_collection.json`
- **Import** this file into Postman
- **Base URL:** `http://localhost:5000`
- **Test all CRUD operations** for Classes and Students

## 📋 Project Features

✅ Clean Architecture with proper layer separation  
✅ Repository Pattern for data access  
✅ Dependency Injection throughout  
✅ EF Core In-Memory Database (no SQL Server required)  
✅ RESTful API design  
✅ Swagger documentation  
✅ Sample data seeding  
✅ Full CRUD operations  
✅ Foreign key relationships  

## 🎯 Architecture Principles

- **Separation of Concerns:** Each layer has distinct responsibilities
- **Dependency Inversion:** Controllers depend on Services, Services depend on Repository interfaces
- **No Direct DbContext Access:** Controllers never access database directly
- **Interface-Based Programming:** All dependencies use interfaces
- **Single Responsibility:** Each class has one clear purpose

## 📝 Sample Data

The application automatically seeds sample data on startup:

**Classes:**
- Computer Science
- Mathematics
- Physics

**Students:**
- John Doe (Computer Science)
- Jane Smith (Computer Science)
- Bob Johnson (Mathematics)

## 🔍 Code Flow

```
Client Request
    ↓
Controller (Presentation Layer)
    ↓
Service (Application Layer - Business Logic)
    ↓
Repository (Infrastructure Layer - Data Access)
    ↓
DbContext (EF Core)
    ↓
In-Memory Database
```

## 📄 License

This project is for educational purposes.

## 👨‍💻 Author

Backend Assignment - Clean Architecture Implementation

---

**Built with ❤️ using ASP.NET Core and Clean Architecture**