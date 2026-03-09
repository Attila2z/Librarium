# Librarium


### Prerequisites
- .NET 8
- Docker

### Setup PostgreSQL with docker

```bash
docker run -d \
  --name librarium-postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=Librarium \
  -p 5432:5432 \
  postgres:12
```

Wait a few moments for the database to initialize before running the next command, otherwise you might get some errors.

### Run App + Migrations

```bash
cd src/Librarium.Data && dotnet ef database update && cd ../Librarium.Api && dotnet run
```

API URL: `http://localhost:5255`

## API Endpoints

- `GET /api/books`
- `GET /api/members`
- `POST /api/loans`
- `GET /api/loans/{memberId}`
