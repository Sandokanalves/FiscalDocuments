# API de Processamento de Documentos Fiscais

## Visão Geral do Projeto

Esta é uma API RESTful construída com ASP.NET Core 9 para o processamento e gerenciamento de documentos fiscais (NFe, CTe, NFSe). A aplicação recebe arquivos XML, extrai dados relevantes, os persiste em um banco de dados e expõe endpoints para consulta e manipulação desses dados.

## Decisões de Arquitetura

- **Arquitetura Limpa (Clean Architecture)**: A solução foi estruturada em camadas (Domain, Application, Infrastructure, Api) para garantir baixo acoplamento, alta coesão e separação de responsabilidades. Isso torna o sistema mais testável, manutenível e flexível a mudanças tecnológicas. A "Regra de Dependência" é rigorosamente seguida, com todas as dependências apontando para o centro (Domain).

- **CQRS (Command Query Responsibility Segregation)**: Utilizamos o padrão CQRS, com o auxílio da biblioteca `MediatR`, para separar as operações de escrita (Commands) das operações de leitura (Queries). Isso simplifica os modelos, otimiza o desempenho para cada tipo de operação e torna a lógica da aplicação mais clara e focada.

- **Domain-Driven Design (DDD)**: A camada de `Domain` foi modelada para ser o coração da aplicação, contendo as entidades de negócio e as regras que são independentes de qualquer framework. A entidade `FiscalDocument` encapsula seus próprios dados e comportamentos (como o método `UpdateTotalAmount`), protegendo sua consistência.

- **SQL Server**: O SQL Server foi escolhido como banco de dados relacional por sua robustez, maturidade e amplo suporte no ecossistema .NET. A utilização do Entity Framework Core como ORM abstrai o acesso aos dados e facilita as migrations.

## Como Rodar a Aplicação (com Docker para Desenvolvimento)

### Pré-requisitos

- .NET 9 SDK
- Docker

### 1. Iniciar o Banco de Dados SQL Server com Docker

Execute o comando abaixo no seu terminal para iniciar uma instância do SQL Server em um contêiner Docker. Substitua `yourStrong(!)Password` pela mesma senha definida no `appsettings.json`.

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Rodar a Aplicação

Navegue até a pasta do projeto da API e execute o comando para iniciar a aplicação.

```bash
cd src/FiscalDocuments.Api
dotnet run
```

### 3. Aplicar as Migrations (Criar o Banco de Dados)

Com a aplicação rodando (ou em um terminal separado), execute o comando a seguir para aplicar as migrations do Entity Framework e criar a estrutura do banco de dados.

```bash
dotnet ef database update --project ../FiscalDocuments.Infrastructure --startup-project .
```

## Como Rodar os Testes

Para executar todos os testes (unitários, de integração e de arquitetura), rode o seguinte comando a partir da raiz do projeto:

```bash
dotnet test
```

## Documentação da API

Após iniciar a aplicação, a documentação completa da API, gerada pelo Swagger, estará disponível em:

**https://localhost:&lt;porta&gt;/swagger**

Substitua `<porta>` pela porta na qual a aplicação está rodando (geralmente definida no arquivo `Properties/launchSettings.json`).

## Melhorias Futuras

- **Processamento Assíncrono de XML**: Para a ingestão de um grande volume de arquivos, o processamento do XML poderia ser movido para uma fila (como RabbitMQ ou Azure Service Bus) e processado por um worker em background. Isso tornaria o endpoint de upload mais rápido e resiliente.
- **Validação Robusta de XML**: Implementar uma biblioteca especializada em documentos fiscais para validar a estrutura e as regras de negócio dos diferentes tipos de XML (NFe, CTe, NFSe).
- **Logging e Monitoramento Avançado**: Integrar uma solução de logging estruturado (como Serilog) e ferramentas de monitoramento de performance (APM) para observar a saúde da aplicação em produção.
- **Autenticação e Autorização**: Proteger os endpoints com um mecanismo de autenticação e autorização, como JWT (JSON Web Tokens).
- **Testes de Carga**: Criar um projeto de teste de carga com ferramentas como k6 ou NBomber para medir o desempenho da API sob estresse.
