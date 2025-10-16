# API de Processamento de Documentos Fiscais

## 1. Visão Geral do Projeto

Esta API RESTful, desenvolvida em ASP.NET Core 9, oferece uma solução robusta e escalável para o recebimento, armazenamento e consulta de documentos fiscais eletrônicos (NFe, CTe, etc.). O projeto foi construído seguindo as melhores práticas de engenharia de software, com foco em qualidade de código, manutenibilidade e testabilidade.

O principal objetivo é fornecer uma retaguarda confiável para sistemas que precisam processar e gerenciar um grande volume de documentos fiscais, garantindo a integridade dos dados, a performance das consultas e a segurança das informações.

---

## 2. Decisões de Arquitetura e Princípios

A espinha dorsal do projeto é a **Arquitetura Limpa (Clean Architecture)**. Essa escolha não foi acidental; ela visa criar um sistema com baixo acoplamento, alta coesão e clara separação de responsabilidades.

### Estrutura da Solução (Múltiplos Projetos)

A solução é organizada em múltiplos projetos (`.csproj`), onde cada projeto representa uma camada da Arquitetura Limpa (`Domain`, `Application`, `Infrastructure`, etc.). Esta é uma prática fundamental para:

- **Garantir o Isolamento**: O compilador impede referências indevidas (ex: a camada de `Domain` não pode depender da `Infrastructure`).
- **Controlar Dependências**: A direção das dependências aponta sempre para o centro (`Domain`), protegendo as regras de negócio de mudanças em tecnologias externas.
- **Facilitar a Manutenção**: A separação clara de responsabilidades torna o código mais fácil de entender, manter e testar.

### Princípios e Padrões Aplicados

- **Princípios SOLID**:

  - **S (Single Responsibility Principle)**: Cada classe tem uma única responsabilidade.
  - **O (Open/Closed Principle)**: A arquitetura é aberta para extensão e fechada para modificação.
  - **L (Liskov Substitution Principle)**: As abstrações são implementadas de forma a garantir a substituibilidade.
  - **I (Interface Segregation Principle)**: Interfaces são pequenas e focadas, como `IFiscalDocumentRepository`.
  - **D (Dependency Inversion Principle)**: As dependências são invertidas através de interfaces, com o auxílio da Injeção de Dependência.

- **Domain-Driven Design (DDD)**: O coração da aplicação, a camada de **Domínio**, foi modelada para refletir as regras de negócio.

  - **Agregado**: A entidade `FiscalDocument` atua como a raiz de agregação.
  - **Value Objects**: Objetos como `Address` são modelados como imutáveis.

- **Padrão CQRS (Command Query Responsibility Segregation)**: A aplicação combina DDD com o padrão CQRS, utilizando a biblioteca **MediatR**.
  - **Commands**: Representam operações de escrita (Create, Update, Delete) e encapsulam toda a lógica para executar uma ação.
  - **Queries**: Representam operações de leitura e são otimizadas para retornar DTOs (Data Transfer Objects).
  - **Benefícios**: Esta combinação simplifica a lógica, permite otimizações independentes para leitura e escrita, e torna os casos de uso explícitos e fáceis de encontrar no código.

---

## 3. Tecnologias Utilizadas

- **Framework**: .NET 9
- **API**: ASP.NET Core
- **Banco de Dados**: SQL Server
- **ORM**: Entity Framework Core 9
- **Padrão de Comunicação**: CQRS com MediatR
- **Validação**: FluentValidation
- **Testes**: xUnit
- **Assertions**: FluentAssertions
- **Teste de Arquitetura**: NetArchTest

---

## 4. Guia de Execução Local

### Pré-requisitos

- **.NET 9 SDK** instalado.
- Uma instância do **SQL Server** rodando localmente (Express, Developer ou LocalDB).

### Passo a Passo

1.  **Clone o Repositório**
    ```bash
    git clone https://github.com/SEU_USUARIO/FiscalDocuments.git
    cd FiscalDocuments
    ```
2.  **Configure a Connection String**
    - Abra o arquivo `src/FiscalDocuments.Api/appsettings.json` e ajuste a `DefaultConnection`.
3.  **Aplique as Migrations**
    ```bash
    dotnet ef database update --project src/FiscalDocuments.Infrastructure --startup-project src/FiscalDocuments.Api
    ```
4.  **Execute a Aplicação**
    ```bash
    cd src/FiscalDocuments.Api
    dotnet run
    ```
5.  **Acesse a Documentação (Swagger)**
    - Abra o navegador em `https://localhost:PORTA/swagger`.

---

## 5. Como Executar os Testes

Para executar a suíte completa de testes (unitários, integração e arquitetura), rode o comando na raiz do projeto:

```bash
dotnet test
```

---

## 6. Melhorias Futuras

Embora a solução atual seja robusta, os seguintes pontos poderiam ser implementados para evoluir o projeto:

- **Autenticação e Autorização**: Implementar segurança nos endpoints utilizando JWT (JSON Web Tokens) e políticas de autorização.
- **Processamento Assíncrono com Filas**: Para o upload de arquivos, utilizar um sistema de mensageria (como RabbitMQ ou Azure Service Bus) para processar os documentos em background, tornando a API mais resiliente e escalável.
- **Logging e Monitoramento Avançado**: Integrar uma ferramenta de logging estruturado (como Serilog com um sink para Seq ou Datadog) e monitoramento de performance (APM) para observar a saúde da aplicação em produção.
- **Testes de Carga**: Adicionar um projeto de testes de carga (usando k6 ou NBomber) para validar a performance da API sob estresse e identificar gargalos.
- **Resiliência e Padrões de Retentativa**: Implementar políticas de resiliência (como Retry e Circuit Breaker com Polly) nas chamadas a serviços externos, se aplicável.
