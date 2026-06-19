# PlaylistifyAPI

A ASP.NET 8 Web API for managing playlists and songs using a many-to-many relationship.

## Architecture

The project is separated into 3 layers:

* **Domain:** Contains the entities (`Playlist`, `Song`) and business rules.
* **Infrastructure:** Handles EF Core, database access, and repository implementations.
* **API:** Contains controllers, DTOs, and endpoint definitions.

## Why SQL Server?

I used SQL Server because the application is highly relational.

A playlist can contain multiple songs, and a song can belong to multiple playlists, so a relational database fits this use case naturally.

It also gives:

* Data integrity through foreign keys and transactions.
* Efficient querying for many-to-many relationships.
* A normalized schema without duplicating data.

## Database Schema

```
[ Playlists ] 1 --- * [ PlaylistSong ] * --- 1 [ Songs ]
```

### Playlists

* `Id` (Guid, PK)
* `Name` (string, required, max 100)
* `Description` (string, optional)
* `CreatedAt` (DateTime)

### Songs

* `Id` (Guid, PK)
* `Title` (string, required)
* `Artist` (string, required)
* `Duration` (TimeSpan)

### PlaylistSong

* `PlaylistId` (FK)
* `SongId` (FK)
* Composite primary key: (`PlaylistId`, `SongId`)

## Running the project

### Prerequisites

* .NET 8 SDK
* SQL Server

### 1. Restore packages

```bash
dotnet restore
```

### 2. Update the connection string

Inside `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=PlaylistifyDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply migrations

```bash
dotnet ef database update --project PlaylistifyAPI.Infrastructure --startup-project PlaylistifyAPI.API
```

### 4. Run the application

```bash
dotnet run --project PlaylistifyAPI.API
```

Then open Swagger from the URL shown in the terminal.

### 5. Run tests

```bash
dotnet test
```
