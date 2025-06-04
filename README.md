# 📦 Bolsa Família API - AS65A-N15-BACK-END

Este repositório contém o código-fonte da Web API desenvolvida em .NET 8 para o gerenciamento de usuários, seus parentes e informações relacionadas ao programa Bolsa Família. A aplicação implementa autenticação via JWT, utiliza Entity Framework Core para persistência de dados, segue princípios de Domain-Driven Design (DDD) e oferece documentação interativa via Swagger.

---

## ✨ Funcionalidades Principais

*   **Autenticação:** Sistema de login seguro utilizando JWT para proteger os endpoints.
*   **Gerenciamento de Usuários:** Cadastro, consulta, atualização e remoção de usuários (com endpoints específicos para administradores).
*   **Gerenciamento de Parentes:** Cadastro, consulta, atualização e remoção de parentes vinculados a um usuário.
*   **Cálculo de Renda Familiar:** Funcionalidade para calcular a renda per capita familiar com base nos parentes cadastrados.
*   **Administração:** Endpoints exclusivos para administradores gerenciarem configurações gerais do sistema (como valor base da renda per capita e tipos de parentesco permitidos) e usuários.
*   **Consultas Dinâmicas:** Endpoints para obter listas de opções (Enums) para campos como Estado Civil, Sexo e Tipos de Parentesco.

---

## 🚀 Tecnologias Utilizadas

*   **Framework:** ASP.NET Core 8
*   **ORM:** Entity Framework Core 8
*   **Banco de Dados:** SQL Server (configurado via Migrations)
*   **Autenticação:** JWT Bearer Authentication
*   **Documentação API:** Swagger (Swashbuckle)
*   **Arquitetura:** Domain-Driven Design (DDD)

---

## ⚙️ Configuração e Execução

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/CaboFernando/AS65A-N15-BACK-END.git
    cd AS65A-N15-BACK-END
    ```
2.  **Configure a String de Conexão:**
    *   Abra o arquivo `BolsaFamilia.API/appsettings.json`.
    *   Modifique a string de conexão `DefaultConnection` para apontar para sua instância do SQL Server.
3.  **Aplique as Migrations:**
    *   Navegue até o diretório `BolsaFamilia.Infra`.
    *   Execute o comando: `dotnet ef database update`
4.  **Configure o JWT:**
    *   No arquivo `BolsaFamilia.API/appsettings.json`, ajuste as configurações do JWT na seção `Jwt` se necessário:
        ```json
        "Jwt": {
          "Key": "SUA_CHAVE_SECRETA_AQUI",
          "Issuer": "BolsaFamiliaAPI",
          "Audience": "BolsaFamiliaClient"
        }
        ```
5.  **Execute a Aplicação:**
    *   Navegue até o diretório `BolsaFamilia.API`.
    *   Execute o comando: `dotnet run`
6.  **Acesse o Swagger:**
    *   Abra seu navegador e acesse a URL fornecida (geralmente `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`).

---

## 🔐 Autenticação (JWT)

Para acessar os endpoints protegidos, você precisa obter um token JWT.

1.  **Faça o Login:**
    *   Utilize o endpoint `POST /api/Auths/login`.
    *   Envie o email e a senha no corpo da requisição:
        ```json
        {
          "email": "seu_email@exemplo.com",
          "senha": "sua_senha"
        }
        ```
2.  **Receba o Token:**
    *   A API retornará um token JWT se as credenciais forem válidas:
        ```json
        {
          "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        }
        ```
3.  **Autorize suas Requisições:**
    *   No Swagger, clique no botão "Authorize".
    *   Na janela que abrir, cole o token recebido no campo "Value", prefixado por `Bearer ` (com espaço):
        ```
        Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        ```
    *   Clique em "Authorize" e depois em "Close".
    *   Agora você pode executar os endpoints protegidos.

---

## 📚 Endpoints da API

A seguir, a lista detalhada dos endpoints disponíveis, agrupados por controlador.

### 🔑 AuthsController (`/api/Auths`)

*   **`POST /login`**
    *   **Descrição:** Autentica um usuário com base no email e senha.
    *   **Acesso:** Público
    *   **Corpo da Requisição (LoginDto):**
        ```json
        {
          "email": "string",
          "senha": "string"
        }
        ```
    *   **Retorno (Sucesso):** Token JWT.
    *   **Retorno (Falha):** `401 Unauthorized`.

### 👤 UsuariosController (`/api/Usuarios`)

*   **`POST /`**
    *   **Descrição:** Cadastra um novo usuário.
    *   **Acesso:** Público (`[AllowAnonymous]`)
    *   **Corpo da Requisição (UsuarioInputDto):**
        ```json
        {
          "nome": "string",
          "cpf": "string (somente números)",
          "email": "string (formato de email válido)",
          "senha": "string"
        }
        ```
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`GET /`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista todos os usuários cadastrados.
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Retorno (Sucesso):** Lista de `UsuarioDto`.

*   **`GET /{id}`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista um usuário cadastrado filtrado por ID.
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Parâmetro de Rota:** `id` (inteiro).
    *   **Retorno (Sucesso):** `UsuarioDto`.
    *   **Retorno (Falha):** `404 Not Found`.

*   **`GET /cpf/{cpf}`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista um usuário cadastrado filtrado por CPF.
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Parâmetro de Rota:** `cpf` (string).
    *   **Retorno (Sucesso):** `UsuarioDto`.
    *   **Retorno (Falha):** `404 Not Found`.

*   **`PUT /{id}`**
    *   **Descrição:** Altera um usuário cadastrado filtrado por ID. O usuário só pode alterar seus próprios dados (verificar implementação do serviço).
    *   **Acesso:** Requer Autenticação.
    *   **Parâmetro de Rota:** `id` (inteiro).
    *   **Corpo da Requisição (UsuarioInputDto):** (Mesmo do POST)
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`PUT /AlterarSenha`**
    *   **Descrição:** Valida por CPF e Email e altera a senha do usuário informado.
    *   **Acesso:** Público (`[AllowAnonymous]`)
    *   **Corpo da Requisição (PasswordInputDto):**
        ```json
        {
          "cpf": "string",
          "email": "string",
          "novaSenha": "string"
        }
        ```
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`DELETE /{id}`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Remove um usuário cadastrado filtrado por ID.
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Parâmetro de Rota:** `id` (inteiro).
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

### 👪 ParentesController (`/api/Parentes`)

*   **`POST /`**
    *   **Descrição:** Cadastra um parente, vinculado ao usuário logado.
    *   **Acesso:** Requer Autenticação.
    *   **Corpo da Requisição (ParenteInputDto):**
        ```json
        {
          "nome": "string",
          "cpf": "string (somente números)",
          "grauParentesco": "string (deve existir na lista de tipos permitidos)",
          "sexo": "integer (1: Masculino, 2: Feminino, 3: Outro)",
          "estadoCivil": "integer (1: Solteiro, 2: Casado, 3: Divorciado, 4: Viuvo, 5: UniaoEstavel)",
          "ocupacao": "string",
          "telefone": "string",
          "renda": "number (decimal)"
        }
        ```
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`GET /`**
    *   **Descrição:** Lista todos os parentes cadastrados pelo usuário logado.
    *   **Acesso:** Requer Autenticação.
    *   **Retorno (Sucesso):** Lista de `ParenteDto`.

*   **`GET /cpf/{cpf}`**
    *   **Descrição:** Lista o parente cadastrado pelo usuário logado, filtrado pelo CPF.
    *   **Acesso:** Requer Autenticação.
    *   **Parâmetro de Rota:** `cpf` (string).
    *   **Retorno (Sucesso):** `ParenteDto`.
    *   **Retorno (Falha):** `404 Not Found`.

*   **`PUT /{id}`**
    *   **Descrição:** Atualiza um parente cadastrado pelo usuário logado, filtrado pelo ID.
    *   **Acesso:** Requer Autenticação.
    *   **Parâmetro de Rota:** `id` (inteiro).
    *   **Corpo da Requisição (ParenteInputDto):** (Mesmo do POST)
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`DELETE /{id}`**
    *   **Descrição:** Remove um parente cadastrado pelo usuário logado, filtrado pelo ID.
    *   **Acesso:** Requer Autenticação.
    *   **Parâmetro de Rota:** `id` (inteiro).
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

*   **`GET /renda`**
    *   **Descrição:** Calcula a renda familiar com base nos parentes cadastrados pelo usuário logado.
    *   **Acesso:** Requer Autenticação.
    *   **Retorno (Sucesso):** Retorna uma mensagem informando se o grupo familiar é ou não é elegível para o programa Bolsa Família.

### ⚙️ AdminController (`/api/Admin`)

*   **`GET /`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista todas as informações gerais (configurações).
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Retorno (Sucesso):** `InfoGeraisDto`.
    *   **Retorno (Falha):** `404 Not Found` (se não houver registro inicial).

*   **`PUT /{id}`**
    *   **Descrição:** [SOMENTE PARA USUÁRIO ADM] Altera as informações gerais.
    *   **Acesso:** Requer Autenticação e Role `Admin`.
    *   **Parâmetro de Rota:** `id` (inteiro, geralmente 1).
    *   **Corpo da Requisição (InfoGeraisInputDto):**
        ```json
        {
          "valorBaseRendaPerCapita": "number (decimal)",
          "tiposParentescoPermitidos": "string (separados por vírgula, ex: Pai,Mae,Filho)"
        }
        ```
    *   **Retorno (Sucesso):** `200 OK` com mensagem.
    *   **Retorno (Falha):** `400 Bad Request`.

### 📋 DropDownsController (`/api/DropDowns`)

*   **`GET /estados-civis`**
    *   **Descrição:** Retorna a lista de estados civis disponíveis (Enum `EstadoCivil`).
    *   **Acesso:** Público.
    *   **Retorno:** Lista de objetos `{ value: int, name: string }`.

*   **`GET /generos`**
    *   **Descrição:** Retorna a lista de gêneros disponíveis (Enum `Sexo`).
    *   **Acesso:** Público.
    *   **Retorno:** Lista de objetos `{ value: int, name: string }`.

*   **`GET /tipos-parentesco`**
    *   **Descrição:** Retorna a lista de tipos de parentesco permitidos, conforme configurado pelo Admin.
    *   **Acesso:** Público.
    *   **Retorno:** Lista de strings.

---

## 📘 Como usar Enums e Tipos de Parentesco

### 🎭 Sexo (`Sexo` Enum)

| Nome        | Valor (Inteiro) |
|-------------|-----------------|
|`NaoDefinido`| 0               |
|`Masculino`  | 1               |
|`Feminino`   | 2               |
|`Outro`      | 3               |

### 💍 Estado Civil (`EstadoCivil` Enum)

| Nome          | Valor (Inteiro) |
|---------------|-----------------|
|`NaoDefinido`  | 0               |
|`Solteiro`     | 1               |
|`Casado`       | 2               |
|`Divorciado`   | 3               |
|`Viuvo`        | 4               |
|`UniaoEstavel` | 5               |

### 👨‍👩‍👧‍👦 Tipos de Parentesco

O campo `grauParentesco` no cadastro/atualização de parentes deve ser uma `string` que corresponda exatamente a um dos tipos definidos pelo administrador no endpoint `PUT /api/Admin/{id}` (campo `tiposParentescoPermitidos`). Use o endpoint `GET /api/DropDowns/tipos-parentesco` para obter a lista atualizada.

**Exemplo de envio no JSON:**

```json
{
  "sexo": 2, // Feminino
  "estadoCivil": 1, // Solteiro
  "grauParentesco": "Filho" // Deve estar na lista de tipos permitidos
}
```

---

## 👥 Membros do Grupo (AS65A-N15)

*   André Faria De Souza - RA 2101106
*   Beatriz Aparecida Banaki De Campos - RA 2210533
*   Carlos Fernando Dos Santos - RA 1692984
*   Rafael De Palma Francisco - RA 2465248
*   Sarah Kelly Almeida - RA 1842293

