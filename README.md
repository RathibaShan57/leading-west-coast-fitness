# Leading West Coast Fitness Club (Arizona)

Project 2 sample for Testable whitebox analysis.

An Arizona fitness-club platform: members, memberships, class bookings, heat-aware outdoor sessions, and Phoenix-metro tax quotes.

## Connect this branch in Testable

Use branch **`west-coast-fitness-club`**. `main` is intentionally empty.

## Platform-accepted stack (pinned)

These pins match Testable worker runtimes and whitebox registry versions:

| Layer | Technology | Pin |
|---|---|---|
| Runtime | Node | **20** (`.nvmrc`, `.node-version`, `engines.node`: `>=20 <21`) |
| Package manager | npm | **10.9.2** (`packageManager`) |
| Frontend | React | **18.3.1** |
| Bundler | Vite | **5.4.11** |
| Language | TypeScript | **5.4.5** |
| Unit / coverage | Vitest + V8 | **2.0.5** (platform `vitest-coverage` 2.0.0 line) |
| Lint | ESLint | **8.47.0** + sonarjs **0.25.1** + security **3.0.1** |
| Duplication | jscpd | **4.0.5** |
| Backend | ASP.NET Core | **net8.0** |
| SDK | .NET SDK | **8.0.400** (`global.json`, `rollForward: latestFeature`) |
| Database | PostgreSQL | **16.4** via Npgsql EF Core **8.0.8** |
| Tests | xUnit + Coverlet | xUnit **2.6.2**, Coverlet **6.0.0** |
| Analyzers | Roslyn / Sonar / SCS | NetAnalyzers **8.0.0**, SonarAnalyzer.CSharp **9.32.0.97167**, SecurityCodeScan **5.6.7** |

Architecture: **Clean Architecture** — Domain → Application → Infrastructure → Api, with the Vite SPA in `frontend/`.

## Local run

```bash
docker compose up -d
# set ConnectionStrings__FitnessDatabase including a local password
dotnet test WestCoastFitness.sln
cd frontend && npm ci && npm test && npm run build
```

Frontend: http://localhost:5173  
API: http://localhost:5088
