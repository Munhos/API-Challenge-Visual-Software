Este projeto é uma Web API ASP.NET Core (.NET 9) desenvolvida como parte do processo seletivo da Visual Software. A aplicação realiza o monitoramento de voos e possui autenticação via JWT, persistência em PostgreSQL via Entity Framework, integração com API externa e testes automatizados.

A aplicação oferece:

Autenticação JWT
Integração com API REST externa via HttpClient
CRUD de voos monitorados internos
Documentação via Swagger
Testes automatizados (unitários e integração)
Orquestração com Docker Compose

Para executar o sistema localmente, certifique-se de ter instalado:

.NET 9 SDK
Docker e Docker Compose

Clone o repositório com o comando: 

- git clone https://github.com/Munhos/API-Challenge-Visual-Software.git
- cd Visual_Software_Challenge

Se for executar diretamente pelo Visual Studio (sem Docker), troque db por localhost na DefaultConnection, ficando assim:
- "DefaultConnection": "Host=localhost;Port=5432;Database=FlightsDb;Username=postgres;Password=postgres"

Para iniciar a aplicação via Docker Compose, execute:

docker-compose up --build

A API estará disponível em: http://localhost:5000

Para rodar os testes, utilize:

 - dotnet test

Isso executará os testes unitários e de integração do projeto. Certifique-se de que o banco de dados esteja em execução via Docker ou localmente.

Para testar os endpoints via Swagger, acesse:

http://localhost:5000/index.html

A documentação permite testar todos os endpoints da API, como:

POST /api/users/newUser (criação de usuário)

POST /api/users/login (login)

GET /api/monitoredflight/getAllExternal (voos externos)

GET/POST/DELETE/PATCH de voos internos

Para testar endpoints que requerem autenticação, realize login, copie o token JWT retornado, clique em "Authorize" no Swagger e insira o token com o prefixo Bearer .

Alguns dos arquivos da estrutura do projeto que é dividido em:

Controllers: MonitoredFlightController.cs, UsersController.cs

Services: FlightsService, UsersService, JwtService

Models: UserModel, MonitoredFlightsModel

Infra: Program.cs, appsettings, Dockerfile, docker-compose.yml

Todos os endpoints estão documentados com Swagger via Swashbuckle.AspNetCore.

A aplicação segue boas práticas de REST, autenticação segura via JWT, persistência com EF Core, testes em C# e containerização com Docker.
