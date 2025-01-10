# GerPros_Backend_API

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/GerPros_Backend_API) version 8.0.6.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

or use docker instead:
```cmd
## release
docker-compose up --build
```

```cmd
## development
docker-compose -f docker-compose.dev.yaml up --build
```



```bash

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::8.0.6
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```

## Docker

- Image build
```bash
docker image build -t apalan600/gerpros-backend-api:latest .
```

- Image push
```bash
docker push apalan600/gerpros-backend-api:latest
```

## AWS ECR

```bash
docker build -t gerpros/api .
```


```bash
docker tag gerpros/api:latest 058264288018.dkr.ecr.ap-northeast-1.amazonaws.com/gerpros/api:latest
```

```bash
## USE AWS CLI to login first
## or use AWS Tookit instead
docker push 058264288018.dkr.ecr.ap-northeast-1.amazonaws.com/gerpros/api:latest
```

## AWS CloudFront

Create a key group(using OpenSSL)
OpenSSL would be installed when installing Git Bash 
- private key
```git bash
openssl genrsa -out private_key.pem 2048
```

- public key
```git bash
openssl rsa -pubout -in private_key.pem -out public_key.pem
```

## Database
- quick test database login using psql 
```shell
& "C:\Program Files\PostgreSQL\17\bin\psql.exe" -h localhost -p 54310 -U TestUser -d GerPros_Backend_APITestDb
```

- local Db
```bash
& "C:\Program Files\PostgreSQL\17\bin\psql.exe" -h localhost -U TestUser -d GerPros_Backend_APITestDb -W
```

## RDS

EC2 instance connect to RDS
```bash
docker run -it --rm \
  -e PGPASSWORD= {PGPASSWORD} \
  postgres:latest \
  psql -h gerpros-database.cd6yu8cwg31s.ap-northeast-1.rds.amazonaws.com \
       -p 5432 \
       -d "GerprosDatabase" \
       -U postgres
```

## Migration

from root folder '\GerPros\GerPros-Backend-API> '
```bash
dotnet ef migrations add "UpdatePostImageUrlMaxLength" --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations

```

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

- To drop all tables in the database, run the following script in the psql:
```postgresql
DO $$ 
DECLARE 
    r RECORD; 
BEGIN 
    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) 
    LOOP 
        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE'; 
    END LOOP; 
END $$;

```

## Help
To learn more about the template go to the [project website](https://github.com/jasontaylordev/CleanArchitecture). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.