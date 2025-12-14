## 🧠 Overview

This project is the backend service for a blog post application. It exposes RESTful APIs to create, read, update, and delete blog posts. The backend is built to be scalable, maintainable, and easy to integrate with any frontend client.

Whether you’re building a blog dashboard, mobile app, or static frontend, this backend provides the foundations to manage blog content efficiently.

---

## 🚀 Features

- ✨ **CRUD operations** for blog posts  
- 🔐 Authentication support (JWT, API keys, etc.)  
- 📦 Structured with best practices for backend architecture  
- 📡 Clean and RESTful API design  
- 🛠️ Easily extensible and ready for production use

---

## 🧩 Tech Stack

| Technology | Role |
|------------|------|
| **C#** | Core backend logic |
| **ASP.NET Core** | REST API framework |
| **Entity Framework** | Database ORM |
| **PostgreSQL** | Data storage |

---

## 📁 Repository Structure
blog_post_app_backend/
├── BlogPostApp/ # Backend application source code
│ ├── Controllers/ # API endpoint handlers
│ ├── Models/ # Data models and entities
│ ├── Services/ # Business logic services
│ ├── Data/ # Database context & migrations
│ └── Program.cs # Entry point
├── .gitignore
├── README.md
└── appsettings.json # App configuration

---

## 🧰 Getting Started

### 🔹 Prerequisites

Make sure you have installed:

- [.NET SDK (9.0+)](https://dotnet.microsoft.com/download)
- A database (PostgreSQL)

---

### ▶️ Run Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/ARASIF1-6/blog_post_app_backend.git
   cd blog_post_app_backend

## Restore dependencies
dotnet restore

## Run the app
dotnet run --project BlogPostApp

## Visit your API:
http://localhost:5000/api/posts

## API Endpoints
| Method | Endpoint      | Description             |
| ------ | ------------- | ----------------------- |
| GET    | `/posts`      | Get all blog posts      |
| GET    | `/posts/{id}` | Get a specific post     |
| POST   | `/posts`      | Create a new post       |
| PUT    | `/posts/{id}` | Update an existing post |
| DELETE | `/posts/{id}` | Delete a post           |



