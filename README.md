# Hospisim - Sistema de Gerenciamento Hospitalar

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Status](https://img.shields.io/badge/status-Em%20Desenvolvimento-yellow.svg)

Hospisim é um sistema de gerenciamento hospitalar completo, desenvolvido como um projeto full-stack utilizando ASP.NET Core. A aplicação abrange desde o cadastro e gerenciamento de pacientes e profissionais até o registro detalhado de atendimentos, internações e prescrições, com o inicio de uma interface web para interação e uma API RESTful para comunicação de dados.

---

## Tabela de Conteúdos
1. [Sobre o Projeto](#1-sobre-o-projeto)
2. [Principais Funcionalidades](#2-principais-funcionalidades)
3. [Regras de Negócio Implementadas](#3-regras-de-negócio-implementadas)
4. [Arquitetura e Tecnologias](#4-arquitetura-e-tecnologias)
5. [Estrutura da API RESTful](#5-estrutura-da-api-restful)
    - [O Padrão DTO](#o-padrão-dto)
    - [Variação nos Endpoints: Por que alguns Controllers são diferentes?](#variação-nos-endpoints)
6. [Banco de Dados](#6-banco-de-dados)
7. [Como Executar o Projeto](#7-como-executar-o-projeto)
8. [Acessar a Aplicação Web](#8-acessar-a-aplicação-web)

---

## 1. Sobre o Projeto
O Hospisim foi concebido para ser uma solução robusta e moderna para a administração de dados em um ambiente hospitalar. Ele centraliza informações críticas, permitindo que a equipe gerencie o ciclo de vida completo de um paciente dentro da instituição, desde seu cadastro inicial até a alta hospitalar, passando por todos os atendimentos, exames e prescrições.

O projeto foi construído com duas frentes principais:
* **API RESTful:** Um backend desacoplado que expõe os dados de forma segura e padronizada, permitindo que a aplicação web (ou futuras aplicações) consuma e manipule as informações.
* **Aplicação Web (MVC):** Foi iniciado um interface visual limpa e profissional para que os usuários do sistema (administradores, recepcionistas, etc.) possam interagir com os dados de forma intuitiva.

## 2. Principais Funcionalidades
- **Gestão de Pacientes:** CRUD completo para o cadastro, edição, visualização de detalhes e exclusão de pacientes.
- **Gestão de Profissionais:** CRUD completo para profissionais de saúde, incluindo associação com especialidades e controle de status (ativo/inativo).
- **Gestão de Especialidades:** CRUD para as especialidades médicas.
- **Registro de Prontuários:** Criação automática de prontuário no primeiro atendimento do paciente.
- **Registro de Atendimentos:** Criação de atendimentos vinculados a um paciente, profissional e prontuário.
- **Registro de Prescrições e Exames:** Funcionalidades para adicionar prescrições e exames a um atendimento existente.
- **Controle de Internações e Altas:** Sistema para registrar a internação de um paciente e, posteriormente, sua alta hospitalar, atualizando os status de forma transacional.

## 3. Regras de Negócio Implementadas
Além do CRUD básico, diversas regras de negócio importantes foram implementadas para garantir a integridade e a lógica do sistema:

- **Prontuário Único por Paciente:** O sistema valida para que um paciente não possa ter mais de um prontuário criado.
- **Criação Automática de Prontuário:** Se um paciente não tiver um prontuário, ele é criado automaticamente no seu primeiro atendimento.
- **Exclusão Segura:**
    - Não é permitido excluir uma **Especialidade** se houver profissionais vinculados a ela.
    - Não é permitido excluir um **Prontuário** se ele contiver atendimentos registrados.
    - A exclusão de pacientes e profissionais na interface web é feita através de um **modal de confirmação** para evitar ações acidentais.
- **Lógica de Alta Hospitalar:** Ao registrar uma alta, o status da internação correspondente é automaticamente atualizado para "Alta concedida" dentro de uma transação, garantindo que as duas operações ocorram juntas ou nenhuma delas.
- **Validação de Entidades:** Antes de criar registros que dependem de outros (como um Atendimento que depende de um Paciente), o sistema valida se as entidades referenciadas existem no banco de dados.

## 4. Arquitetura e Tecnologias
O projeto foi desenvolvido com tecnologias modernas do ecossistema .NET, seguindo boas práticas de arquitetura.

- **Backend:** ASP.NET Core 8, C#
- **Acesso a Dados:** Entity Framework Core 8 (usando a abordagem Code-First)
- **Banco de Dados:** SQL Server
- **Frontend (MVC):** Razor Pages, HTML5, CSS3, JavaScript (vanilla)
- **API:** Padrão RESTful com uso de DTOs (Data Transfer Objects)
- **Injeção de Dependência:** Utilizada para injetar serviços como o `DbContext` nos controllers.

## 5. Estrutura da API RESTful

A API foi projetada para ser o "cérebro" do sistema, expondo os dados de forma segura e padronizada.

### O Padrão DTO
Uma decisão arquitetônica chave foi o uso de **DTOs (Data Transfer Objects)**. Em vez de expor os modelos do Entity Framework diretamente, criamos classes específicas para cada operação da API. Isso traz enormes vantagens:
1.  **Segurança:** Evita vulnerabilidades de "over-posting", pois a API só aceita os campos definidos no DTO de criação/atualização.
2.  **Contrato Claro:** O Swagger e os consumidores da API sabem exatamente quais campos esperar ou enviar, sem a "sujeira" das propriedades de navegação do EF Core.
3.  **Sem Referências Cíclicas:** Resolve os erros de serialização JSON que ocorrem devido a relacionamentos de ida e volta (ex: `Paciente` -> `Prontuario` -> `Paciente`).
4.  **Flexibilidade:** Permite que o modelo do banco de dados evolua sem quebrar a API pública.

### Variação nos Endpoints
> *Por que alguns controllers (como o de Atendimento) não têm um `GetAll()`, enquanto outros (como Pacientes) têm um CRUD completo?*

Isso é uma decisão de design baseada na lógica de negócio e na forma como os recursos se relacionam.

- **Controllers Completos (ex: `PacientesController`, `ProfissionaisApiController`):**
  Representam **recursos primários** do sistema. Faz todo o sentido você querer "listar todos os pacientes" ou "buscar um profissional específico". Eles são os pontos de entrada principais e, por isso, possuem todos os métodos CRUD (GET All, GET by Id, POST, PUT, DELETE).

- **Controllers Parciais (ex: `AtendimentosApiController`, `PrescricoesApiController`):**
  Representam **recursos secundários ou contextuais**. Geralmente, não faz sentido listar "todas as prescrições do hospital de uma só vez". Uma prescrição só existe no contexto de um **atendimento**. Por isso:
  - O `POST` é essencial para **criar** o recurso.
  - O `GET by Id` é útil para ver os **detalhes** de um recurso específico.
  - O `PUT` e o `DELETE` servem para **modificar ou remover** um recurso específico.
  - Um `GetAll()` global não foi implementado porque a lista desses itens é mais útil quando visualizada dentro do seu "pai" (ex: ver a lista de prescrições e exames na tela de detalhes do atendimento).

## 6. Banco de Dados

- **Abordagem Code-First:** O banco de dados é gerado e gerenciado a partir das classes de modelo C# (`Paciente.cs`, etc.).
- **Migrations:** O versionamento do banco de dados é feito com o Entity Framework Migrations. Qualquer alteração no modelo gera uma nova *migration*, que pode ser aplicada para atualizar o banco de forma segura.
- **Seed de Dados:** O arquivo `HospisimDbContext.cs` contém um bloco `OnModelCreating` com o método `HasData` para popular o banco com um conjunto inicial de dados (pacientes, especialidades, profissionais, etc.), facilitando os testes e o desenvolvimento.
- **Relacionamentos e Comportamento de Exclusão:** As regras de relacionamento (1:1, 1:N) e o comportamento de exclusão em cascata foram cuidadosamente configurados para evitar ciclos e garantir a integridade dos dados.

## 7. Como Executar o Projeto

1.  **Pré-requisitos:**
    * SDK do .NET 8 (ou superior).
    * SQL Server (Express, Developer ou outra edição).
    * Visual Studio 2022 ou um editor de sua preferência.

2.  **Passos:**
    * Clone este repositório: `git clone [(https://github.com/KethelenParra/Hospisim.Api.git)]`
    * Abra o arquivo `appsettings.json` e configure a `ConnectionString` para apontar para o seu servidor SQL Server.
    * Abra o **Package Manager Console** no Visual Studio.
    * Execute o comando `Update-database` para criar o banco de dados e aplicar todas as migrations.
    * Rode a aplicação (pressionando F5 no Visual Studio ou com o comando `dotnet run`).
    * A aplicação estará disponível em `https://localhost:7202`.

## 8. Acessar a Aplicação Web

 - Depois de executar o projeto, na URL alterar a rota `https://localhost:7202/swagger/index.html` para `https://localhost:7202/Home`.
 - Na aplicação web tem por enquanto apenas a Home, CRUD paciente e profissional da saúde.
