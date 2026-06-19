# How I Used AI & Architectural Decisions

Just a quick breakdown of where I used AI during this task

## 1. Architecture decisions vs YAGNI

* For a project this small (2 tables), using a full Repository + Service layer is arguably overkill and goes against YAGNI. Injecting `DbContext` directly into controllers would've been the simpler approach.

* For that discussed both approaches with AI and ended up going with a 3-layer Clean Architecture (`Domain`, `Infrastructure`, `API`) because this assessment is more focused on SOLID principles, testability, and scalability rather than building the smallest possible solution, So I went for the sweet spot to show clean Architecture while still not overengineering it.

* I also kept it practical by creating a lightweight `GenericRepository<T>` and a dedicated `IPlaylistRepository`. This avoided repeating CRUD logic while still giving me a clean place to handle EF Core eager loading (`.Include(p => p.Songs)`) for the many-to-many relationship.
* Additionally, by abstracting the data layer behind interfaces (IPlaylistRepository), I was able to write fast, isolated Unit Tests using mock repositories without touching a database. It also allowed me to cleanly swap out the real SQL provider for an InMemoryDatabase provider to run robust Integration Tests effortlessly (REPO PATTERN POWER).

## 2. Removing unnecessary AI boilerplate

AI tends to over-engineer things sometimes.

Some generated repository code included:

* `Expression<Func<T, bool>>` leaking into places where it wasn't needed.
* `CancellationToken = default` on every method.
* Async wrappers everywhere.

I manually simplified a lot of that.

For example, making `Update` and `Delete` asynchronous in an EF Core repository doesn't make much sense since they only modify the entity tracking state in memory. The database isn't touched until `SaveChanges` is called.

I changed those back to synchronous methods, removed unnecessary abstractions, and kept the code simpler and easier to read.

## 3. What I actually used AI for

AI usage was limited to a few areas:

* 📝 Documentation boilerplate: generating the initial markdown structure for the database schema documentation.

* 🐛 Routing bug fix: helping diagnose a `404 NotFound` integration test failure caused by a route mismatch (`/api/playlist` vs `/api/playlists`).

* 🧪 Test scaffolding: generating the baseline setup for `.NET`'s `WebApplicationFactory` to replace the production database provider with an `InMemoryDatabase` during tests.
