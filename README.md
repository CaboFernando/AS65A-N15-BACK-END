
# 📦 BolsaFamilia API

Este repositório contém o código-fonte da Web API desenvolvida em .NET Core 8 para gerenciamento de usuários e seus respectivos membros familiares (parentes) no contexto do programa Bolsa Família. A aplicação utiliza autenticação via JWT, Entity Framework Core, arquitetura DDD, e documentação via Swagger.

---

## 🔐 Autenticação

### Como autenticar?

1. Realize o login com o endpoint:

```

POST /api/Auths/login

````

**Body (JSON):**

```json
{
  "email": "teste@gmail.com",
  "senha": "suaSenha"
}
````

2. O retorno será um **token JWT**. Exemplo:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

3. No Swagger (ou nas requisições via Postman/curl), clique em **Authorize** e insira o token no formato:

```
Bearer {seu_token}
```

---

## 📚 Endpoints Disponíveis

### 🔐 Auth

#### `POST /api/Auths/login`

Autentica o usuário com email e senha, retornando um token JWT válido.

---

### 👤 Usuario

> Todos os endpoints abaixo **exigem autenticação JWT**, exceto o cadastro.

#### `GET /api/Usuarios`

Retorna uma lista de todos os usuários cadastrados.

#### `GET /api/Usuarios/{id}`

Consulta um usuário específico pelo seu **ID**.

#### `GET /api/Usuarios/cpf/{cpf}`

Consulta um usuário específico pelo seu **CPF**.

#### `POST /api/Usuarios`

Cadastra um novo usuário.

**Exemplo de Body:**

```json
{
  "nome": "João da Silva",
  "cpf": "12345678900",
  "email": "joao@gmail.com",
  "senha": "senha123"
}
```

#### `PUT /api/Usuarios`

Atualiza os dados de um usuário existente.

#### `DELETE /api/Usuarios/{cpf}`

Remove um usuário com base no CPF informado na rota.

---

### 👪 Parente

> Todos os endpoints exigem autenticação JWT. Os dados cadastrados ficam vinculados ao usuário autenticado.

#### `GET /api/Parentes`

Retorna todos os parentes cadastrados pelo usuário autenticado.

#### `GET /api/Parentes/cpf/{cpf}`

Consulta um parente específico pelo CPF.

#### `POST /api/Parentes`

Cadastra um novo parente vinculado ao usuário autenticado.

**Exemplo de Body:**

```json
{
  "nome": "Maria da Silva",
  "cpf": "98765432100",
  "grauParentesco": "Mae",
  "sexo": 2, //<-- Feminino
  "estadoCivil": 2, //<--Casado
  "ocupacao": "Professora",
  "telefone": "(11) 91234-5678",
  "renda": 2300.00
}
```

#### `PUT /api/Parentes`

Atualiza os dados de um parente (identificado pelo CPF no body).

#### `DELETE /api/Parentes/{cpf}`

Remove um parente com base no CPF informado.

#### `GET /api/Parentes/renda`

Vai retornar informações sobre a renda dos parentes vinculados ao usuário logado, apresentando a informação se ele pode ou não ser elegível ao programa do Bolsa Família

---

## 📘 Como enviar valores de enum (`sexo` e `estadoCivil`)

Os campos `sexo` e `estadoCivil` devem ser enviados como **inteiro** no JSON, correspondendo exatamente aos nomes abaixo:

### 🎭 Sexo

| Valor (string) | Código         |
| -------------- | -------------- |
| "Masculino"    | 1              |
| "Feminino"     | 2              |
| "Outro"        | 3              |

### 💍 Estado Civil

| Valor (string) | Código         |
| -------------- | -------------- |
| "Solteiro"     | 1              |
| "Casado"       | 2              |
| "Divorciado"   | 3              |
| "Viuvo"        | 4              |
| "UniaoEstavel" | 5              |

> ⚠️ **Atenção:** os valores devem ser enviados com **código exato** no corpo da requisição. Exemplo:

```json
{
  "estadoCivil": 1,
  "sexo": 1
}
```

---

## ⚙️ Configurações JWT

No `appsettings.json`, configure as informações do JWT:

```json
"Jwt": {
  "Key": "uF2/xAf8Pwo6MP2xL6EkDqKDPqxlmujMbTyF2cmZxRc=",
  "Issuer": "MinhaApiSegura",
  "Audience": "MinhaApiClientes"
}
```

---

## 🚀 Tecnologias Usadas

* ASP.NET Core 8
* Entity Framework Core
* Swagger (Swashbuckle)
* JWT Bearer Authentication
* SQL Server
* DDD (Domain-Driven Design)

---

## 👥 Membros do Grupo

* André Faria De Souza - RA 2101106
* Beatriz Aparecida Banaki De Campos - RA 2210533
* Carlos Fernando Dos Santos - RA 1692984
* Rafael De Palma Francisco - RA 2465248
* Sarah Kelly Almeida - RA 1842293

```

