# MyPAS (My Patient Accounting System)

A full-stack patient accounting system built with C#, .NET, Entity Framework, SQL Server, and React.  
This project helps users manage patient records, services, and payments in a clear and efficient way.

---

## ✨ Features

- 🔍 View, add, update, and delete patient records
- 💳 Track patient services and payments
- 📊 Planned: Dynamically calculate service and payment totals
- 📁 Planned: Import/export patient data from files
- 📧 Planned: Email support for patient records
- ☁️ Future: Azure deployment (App Service, SQL, Storage)

---

## 🛠️ Tech Stack

| Frontend     | Backend            | Database      | Tools & Services     |
|--------------|--------------------|---------------|-----------------------|
| React        | ASP.NET Core       | SQL Server    | Entity Framework Core |
| Tailwind CSS | RESTful API        | Localhost DB  | GitHub, Visual Studio |
| React Router |                    |               | Azure (Planned)       |

---

## 🖥️ Running the App Locally

### 🔧 Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full install)
- [Node.js & npm](https://nodejs.org/)
- Visual Studio or VS Code

---

### 🚀 Backend Setup

```bash
cd server
dotnet restore
dotnet ef database update
dotnet run
