# 📦 Bolsa Família API - AS65A-N15-BACK-END

Este repositório contém o código-fonte da Web API desenvolvida em .NET 8 para o gerenciamento de usuários, seus parentes e informações relacionadas ao programa Bolsa Família. A aplicação implementa autenticação via JWT, utiliza Entity Framework Core para persistência de dados, segue princípios de Domain-Driven Design (DDD) e oferece documentação interativa via Swagger.

* **Link da API em Produção com Swagger:** [https://bolsafamilia-api-c3agdmbpdnhxaufz.brazilsouth-01.azurewebsites.net/swagger/index.html](https://bolsafamilia-api-c3agdmbpdnhxaufz.brazilsouth-01.azurewebsites.net/swagger/index.html)

---

## ✨ Funcionalidades Principais

* **Autenticação:** Sistema de login seguro utilizando JWT para proteger os endpoints.
* **Gerenciamento de Usuários:** Cadastro, consulta, atualização e remoção de usuários (com endpoints específicos para administradores).
* **Gerenciamento de Parentes:** Cadastro, consulta, atualização e remoção de parentes vinculados a um usuário.
* **Cálculo de Renda Familiar:** Funcionalidade para calcular a renda per capita familiar com base nos parentes cadastrados.
* **Administração:** Endpoints exclusivos para administradores gerenciarem configurações gerais do sistema (como valor base da renda per capita e tipos de parentesco permitidos) e usuários.
* **Consultas Dinâmicas:** Endpoints para obter listas de opções (Enums) para campos como Estado Civil, Sexo e Tipos de Parentesco.

---

## 🚀 Tecnologias Utilizadas

* **Framework:** ASP.NET Core 8
* **ORM:** Entity Framework Core 8
* **Banco de Dados:** SQL Server (configurado via Migrations)
* **Autenticação:** JWT Bearer Authentication
* **Documentação API:** Swagger (Swashbuckle)
* **Arquitetura:** Domain-Driven Design (DDD)

---

## ⚙️ Configuração e Execução

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/CaboFernando/AS65A-N15-BACK-END.git](https://github.com/CaboFernando/AS65A-N15-BACK-END.git)
    cd AS65A-N15-BACK-END
    ```
2.  **Configure a String de Conexão:**
    * Abra o arquivo `BolsaFamilia.API/appsettings.json`.
    * Modifique a string de conexão `DefaultConnection` para apontar para sua instância do SQL Server.
    * Para ambiente de desenvolvimento, verifique o `BolsaFamilia.API/appsettings.Development.json`.
3.  **Aplique as Migrations:**
    * Navegue até o diretório `BolsaFamilia.Infra`.
    * Execute o comando: `dotnet ef database update`
4.  **Configure o JWT:**
    * No arquivo `BolsaFamilia.API/appsettings.json`, ajuste as configurações do JWT na seção `Jwt` se necessário:
        ```json
        "Jwt": {
          "Key": "SUA_CHAVE_SECRETA_AQUI",
          "Issuer": "BolsaFamiliaAPI",
          "Audience": "BolsaFamiliaAPIUsers"
        }
        ```
5.  **Execute a Aplicação:**
    * Navegue até o diretório `BolsaFamilia.API`.
    * Execute o comando: `dotnet run`
6.  **Acesse o Swagger:**
    * Abra seu navegador e acesse a URL fornecida (geralmente `http://localhost:5112/swagger` ou `https://localhost:7157/swagger` para desenvolvimento).

---

## 🔐 Autenticação (JWT)

Para acessar os endpoints protegidos, você precisa obter um token JWT.

1.  **Faça o Login:**
    * Utilize o endpoint `POST /api/Auths/login`.
    * Envie o email e a senha no corpo da requisição:
        ```json
        {
          "email": "seu_email@exemplo.com",
          "senha": "sua_senha"
        }
        ```
2.  **Receba o Token:**
    * A API retornará um token JWT se as credenciais forem válidas:
        ```json
        {
          "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
          "idUsuario": 1,
          "isAdm": true
          "message": "Autenticação realizada com sucesso."
        }
        ```
    * Em caso de falha, retornará `401 Unauthorized` com uma mensagem de erro.
3.  **Autorize suas Requisições:**
    * No Swagger, clique no botão "Authorize".
    * Na janela que abrir, cole o token recebido no campo "Value", prefixado por `Bearer ` (com espaço):
        ```
        Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        ```
    * Clique em "Authorize" e depois em "Close".
    * Agora você pode executar os endpoints protegidos.

---

## 📚 Endpoints da API

A seguir, a lista detalhada dos endpoints disponíveis, agrupados por controlador. Para todas as respostas de sucesso, a API retornará um status `200 OK` e um objeto `Response<T>` com `Success: true` e a `Message` apropriada. Em caso de falha, geralmente retornará `400 Bad Request` com `Success: false` e uma `Message` detalhando o erro, a menos que especificado de outra forma.

### 🔑 AuthsController (`/api/Auths`)

* **`POST /login`**
    * **Descrição:** Autentica um usuário com base no email e senha.
    * **Acesso:** Público
    * **Corpo da Requisição (LoginDto):**
        ```json
        {
          "email": "string",
          "senha": "string"
        }
        ```
    * **Retorno (Sucesso):** Objeto com `Token` (string JWT), `IdUsuario` (int), `IsAdmin` (bool) e `Message` (string).
    * **Retorno (Falha):** `401 Unauthorized` com a `Message` "Usuário ou senha inválidos." ou "Dados de login inválidos. Verifique o email e a senha.".

### 👤 UsuariosController (`/api/Usuarios`)

* **`POST /`**
    * **Descrição:** Cadastra um novo usuário. Um parente com o grau de parentesco "Responsável" é automaticamente criado e vinculado ao novo usuário.
    * **Acesso:** Público (`[AllowAnonymous]`)
    * **Corpo da Requisição (UsuarioInputDto):**
        ```json
        {
          "nome": "string",
          "cpf": "string (somente números)",
          "email": "string (formato de email válido)",
          "senha": "string"
        }
        ```
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Usuário cadastrado com sucesso."`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado é inválido.", "Já existe um usuário cadastrado com este CPF.").

* **`GET /`**
    * **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista todos os usuários cadastrados.
    * **Acesso:** Requer Autenticação e Role `Admin`.
    * **Retorno (Sucesso):** `Response<IEnumerable<UsuarioDto>>` com a lista de usuários.

* **`GET /{id}`**
    * **Descrição:** Lista um usuário cadastrado filtrado por ID.
    * **Acesso:** Requer Autenticação.
    * **Parâmetro de Rota:** `id` (inteiro).
    * **Retorno (Sucesso):** `Response<UsuarioDto>` com o usuário encontrado.
    * **Retorno (Falha):** `Response<UsuarioDto>` com `Success: false` e `Message: "Usuário id: {id} não encontrado."`.

* **`GET /cpf/{cpf}`**
    * **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista um usuário cadastrado filtrado por CPF.
    * **Acesso:** Requer Autenticação e Role `Admin`.
    * **Parâmetro de Rota:** `cpf` (string).
    * **Retorno (Sucesso):** `Response<UsuarioDto>` com o usuário encontrado.
    * **Retorno (Falha):** `Response<UsuarioDto>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado é inválido.", "Usuário com CPF {cpf} não encontrado.").

* **`PUT /{id}`**
    * **Descrição:** Altera um usuário cadastrado filtrado por ID. O usuário logado só pode alterar seus próprios dados.
    * **Acesso:** Requer Autenticação.
    * **Parâmetro de Rota:** `id` (inteiro).
    * **Corpo da Requisição (UsuarioInputDto):**
        ```json
        {
          "nome": "string",
          "cpf": "string (somente números)",
          "email": "string (formato de email válido)",
          "senha": "string"
        }
        ```
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Usuário atualizado com sucesso."`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "Só é possível alterar o próprio cadastro.", "CPF informado é inválido.").

* **`PUT /AlterarSenha`**
    * **Descrição:** Valida por CPF e Email e altera a senha do usuário informado.
    * **Acesso:** Público (`[AllowAnonymous]`)
    * **Corpo da Requisição (PasswordInputDto):**
        ```json
        {
          "cpf": "string",
          "email": "string",
          "novaSenha": "string"
        }
        ```
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Senha atualizada com sucesso."`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado não foi encontrado.", "CPF e e-mail não conferem.").

* **`DELETE /{id}`**
    * **Descrição:** [SOMENTE PARA USUÁRIO ADM] Remove um usuário cadastrado filtrado por ID.
    * **Acesso:** Requer Autenticação e Role `Admin`.
    * **Parâmetro de Rota:** `id` (inteiro).
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Usuário removido com sucesso."`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "Usuário não encontrado para remoção.").

### 👪 ParentesController (`/api/Parentes`)

* **`POST /`**
    * **Descrição:** Cadastra um parente, vinculado ao usuário logado.
    * **Acesso:** Requer Autenticação.
    * **Corpo da Requisição (ParenteInputDto):**
        ```json
        {
          "nome": "string",
          "cpf": "string (somente números)",
          "grauParentesco": "string (deve existir na lista de tipos permitidos)",
          "sexo": "integer (0: NaoInformado, 1: Masculino, 2: Feminino, 3: Outro)",
          "estadoCivil": "integer (0: NaoInformado, 1: Solteiro, 2: Casado, 3: Divorciado, 4: Viuvo, 5: UniaoEstavel)",
          "ocupacao": "string",
          "telefone": "string",
          "renda": "number (decimal)"
        }
        ```
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Parente cadastrado com sucesso!"`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado é inválido.", "Já existe um parente cadastrado com este CPF para o usuário logado.").

* **`GET /`**
    * **Descrição:** Lista todos os parentes cadastrados pelo usuário logado.
    * **Acesso:** Requer Autenticação.
    * **Retorno (Sucesso):** `Response<IEnumerable<ParenteDto>>` com a lista de parentes.

* **`GET /cpf/{cpf}`**
    * **Descrição:** Lista o parente cadastrado pelo usuário logado, filtrado pelo CPF.
    * **Acesso:** Requer Autenticação.
    * **Parâmetro de Rota:** `cpf` (string).
    * **Retorno (Sucesso):** `Response<ParenteDto>` com o parente encontrado.
    * **Retorno (Falha):** `Response<ParenteDto>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado é inválido.", "Parente com CPF {cpf} não encontrado para o usuário logado.").

* **`PUT /{id}`**
    * **Descrição:** Atualiza um parente cadastrado pelo usuário logado, filtrado pelo ID.
    * **Acesso:** Requer Autenticação.
    * **Parâmetro de Rota:** `id` (inteiro).
    * **Corpo da Requisição (ParenteInputDto):** (Mesmo do POST)
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Parente atualizado com sucesso!"`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "CPF informado é inválido.", "Parente não encontrado para o usuário logado.").

* **`DELETE /{id}`**
    * **Descrição:** Remove um parente cadastrado pelo usuário logado, filtrado pelo ID.
    * **Acesso:** Requer Autenticação.
    * **Parâmetro de Rota:** `id` (inteiro).
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Parente removido com sucesso!"`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "Parente não encontrado para o usuário logado.").

* **`GET /renda`**
    * **Descrição:** Calcula a renda familiar com base nos parentes cadastrados pelo usuário logado e verifica a elegibilidade para o Bolsa Família.
    * **Acesso:** Requer Autenticação.
    * **Retorno (Sucesso):** `Response<string>` com `Data: null` e uma `Message` informando se o grupo familiar é elegível ou não (ex: "De acordo com o cálculo dos parentes cadastrado para o usuário logado, o grupo familiar é SIM elegível para o programa Bolsa Família").

### ⚙️ AdminController (`/api/Admin`)

* **`GET /`**
    * **Descrição:** [SOMENTE PARA USUÁRIO ADM] Lista todas as informações gerais (configurações do sistema).
    * **Acesso:** Requer Autenticação e Role `Admin`.
    * **Retorno (Sucesso):** `Response<InfoGeraisDto>` com as informações gerais.
    * **Retorno (Falha):** `Response<InfoGeraisDto>` com `Success: false` e `Message: "Configurações gerais não encontradas. É necessário criar o registro inicial."`.

* **`PUT /{id}`**
    * **Descrição:** [SOMENTE PARA USUÁRIO ADM] Altera as informações gerais.
    * **Acesso:** Requer Autenticação e Role `Admin`.
    * **Parâmetro de Rota:** `id` (inteiro, geralmente 1).
    * **Corpo da Requisição (InfoGeraisInputDto):**
        ```json
        {
          "valorBaseRendaPerCapita": "number (decimal)",
          "tiposParentescoPermitidos": "string (separados por vírgula, ex: Pai,Mae,Filho)"
        }
        ```
    * **Retorno (Sucesso):** `Response<bool>` com `Data: true` e `Message: "Informações gerais atualizadas com sucesso!"`.
    * **Retorno (Falha):** `Response<bool>` com `Success: false` e `Message` indicando o erro (ex: "Configurações gerais não encontradas para atualização.").

### 📋 DropDownsController (`/api/DropDowns`)

* **`GET /estados-civis`**
    * **Descrição:** Retorna a lista de estados civis disponíveis (Enum `EstadoCivil`).
    * **Acesso:** Público.
    * **Retorno (Sucesso):** `Response<List<object>>` onde cada objeto tem `value` (int) e `name` (string).

* **`GET /generos`**
    * **Descrição:** Retorna a lista de gêneros disponíveis (Enum `Sexo`).
    * **Acesso:** Público.
    * **Retorno (Sucesso):** `Response<List<object>>` onde cada objeto tem `value` (int) e `name` (string).

* **`GET /tipos-parentesco`**
    * **Descrição:** Retorna a lista de tipos de parentesco permitidos, conforme configurado pelo Admin.
    * **Acesso:** Público.
    * **Retorno (Sucesso):** `Response<List<string>>` com a lista de strings de tipos de parentesco.

---

## 📘 Como usar Enums e Tipos de Parentesco

### 🎭 Sexo (`Sexo` Enum)

| Nome          | Valor (Inteiro) |
|---------------|-----------------|
|`NaoInformado` | 0               |
|`Masculino`    | 1               |
|`Feminino`     | 2               |
|`Outro`        | 3               |

### 💍 Estado Civil (`EstadoCivil` Enum)

| Nome           | Valor (Inteiro) |
|----------------|-----------------|
|`NaoInformado`  | 0               |
|`Solteiro`      | 1               |
|`Casado`        | 2               |
|`Divorciado`    | 3               |
|`Viuvo`         | 4               |
|`UniaoEstavel`  | 5               |

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

