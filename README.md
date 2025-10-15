# API de Processamento de Documentos Fiscais

## 1. Visão Geral do Projeto

Esta API RESTful, desenvolvida em ASP.NET Core 9, oferece uma solução robusta e escalável para o recebimento, armazenamento e consulta de documentos fiscais eletrônicos (NFe, CTe, etc.). O projeto foi construído seguindo as melhores práticas de engenharia de software, com foco em qualidade de código, manutenibilidade e testabilidade.

O principal objetivo é fornecer uma retaguarda confiável para sistemas que precisam processar e gerenciar um grande volume de documentos fiscais, garantindo a integridade dos dados, a performance das consultas e a segurança das informações.

---

## 2. Decisões de Arquitetura e Princípios

A espinha dorsal do projeto é a **Arquitetura Limpa (Clean Architecture)**. Essa escolha não foi acidental; ela visa criar um sistema com baixo acoplamento, alta coesão e clara separação de responsabilidades, resultando em um software mais fácil de manter, testar e evoluir.

Os seguintes princípios e padrões foram rigorosamente aplicados:

- **Princípios SOLID**:

  - **S (Single Responsibility Principle)**: Cada classe e método tem uma única responsabilidade. Por exemplo, os _Handlers_ do MediatR cuidam de uma única operação (um Command ou uma Query).
  - **O (Open/Closed Principle)**: A arquitetura é aberta para extensão (novos casos de uso) e fechada para modificação (as camadas centrais não são alteradas).
  - **L (Liskov Substitution Principle)**: As abstrações (interfaces) são implementadas de forma a garantir a substituibilidade sem quebrar o sistema.
  - **I (Interface Segregation Principle)**: Interfaces são pequenas e focadas, como a `IFiscalDocumentRepository`, que define apenas o contrato necessário para a persistência.
  - **D (Dependency Inversion Principle)**: As camadas mais internas (Domínio) não dependem das camadas externas (Infraestrutura). A dependência é invertida através de interfaces, implementadas com a Injeção de Dependência nativa do ASP.NET Core.

- **Domain-Driven Design (DDD)**: O coração da aplicação, a camada de **Domínio**, foi modelada para refletir a linguagem e as regras de negócio do problema.

  - **Agregado**: A entidade `FiscalDocument` atua como a raiz de agregação, garantindo a consistência de seus objetos internos, como a coleção de `ProductItem`.
  - **Value Objects**: Objetos como `Address` foram modelados como imutáveis, representando conceitos que são definidos por seus atributos, não por uma identidade.

- **Padrão CQRS (Command Query Responsibility Segregation)**: A separação entre operações de escrita (Commands) e leitura (Queries) foi implementada com a biblioteca **MediatR**.
  - **Benefícios**: Simplifica a lógica, permite otimizações independentes para leitura e escrita, e torna os casos de uso explícitos e fáceis de encontrar no código.

---

## 3. Tecnologias Utilizadas

- **Framework**: .NET 9
- **API**: ASP.NET Core
- **Banco de Dados**: SQL Server
- **ORM**: Entity Framework Core 9
- **Padrão de Comunicação**: CQRS com MediatR
- **Validação**: FluentValidation
- **Testes**: xUnit para testes unitários, de integração e de arquitetura
- **Assertions**: FluentAssertions para testes mais legíveis
- **Teste de Arquitetura**: NetArchTest

---

## 4. Guia de Execução Local

Siga estes passos para clonar, configurar e executar a aplicação em um ambiente local.

### Pré-requisitos

- **[.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)** instalado.
- Uma instância do **SQL Server** rodando localmente (pode ser a versão Express, Developer ou o LocalDB, que já vem com o Visual Studio).

### Passo a Passo

1.  **Clone o Repositório**

    ```bash
    git clone https://github.com/SEU_USUARIO/FiscalDocuments.git
    cd FiscalDocuments
    ```

2.  **Configure a Connection String**

    - Abra o arquivo `src/FiscalDocuments.Api/appsettings.json`.
    - Localize a seção `ConnectionStrings` e ajuste o valor de `DefaultConnection` para apontar para a sua instância do SQL Server.
    - **Exemplo para SQL Server LocalDB (padrão do Visual Studio):**
      ```json
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FiscalDocumentsDb;Trusted_Connection=True;"
      ```
    - **Exemplo para SQL Server Express:**
      ```json
      "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FiscalDocumentsDb;Trusted_Connection=True;"
      ```

3.  **Aplique as Migrations para Criar o Banco**

    - Abra um terminal na raiz do projeto e execute o comando a seguir. Ele irá criar o banco de dados `FiscalDocumentsDb` com a estrutura de tabelas correta.

    ```bash
    dotnet ef database update --project src/FiscalDocuments.Infrastructure --startup-project src/FiscalDocuments.Api
    ```

4.  **Execute a Aplicação**

    - Navegue até a pasta do projeto da API e inicie a aplicação:

    ```bash
    cd src/FiscalDocuments.Api
    dotnet run
    ```

5.  **Acesse a Documentação da API (Swagger)**
    - Com a aplicação rodando, abra seu navegador e acesse a UI do Swagger para interagir com os endpoints:
    - **https://localhost:PORTA/swagger** (a porta exata será exibida no terminal, geralmente algo como 7171).

---

## 5. Como Executar os Testes

A solução contém uma suíte completa de testes para garantir a qualidade e a corretude do código.

- **Testes Unitários**: Validam a lógica de negócio nos _Handlers_ de forma isolada.
- **Testes de Integração**: Verificam o fluxo completo, desde o endpoint da API até o banco de dados de teste (usando SQLite em memória para garantir o isolamento).
- **Testes de Arquitetura**: Garantem que as dependências entre as camadas estão corretas (ex: a camada de Domínio não depende da Infraestrutura).

Para executar todos os testes, rode o seguinte comando na raiz do projeto:

```bash
dotnet test

```
