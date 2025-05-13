# 📦 BolsaFamilia API

Este repositório tem como objetivo, armazenamento do código fonte da Web API que será consumido pelo front-end no projeto da disciplina de AS65A - Certificadora De Competência Identitária N15 (2025_01).

API desenvolvida em .NET Core para gerenciamento de usuários do programa Bolsa Família, com autenticação via JWT e documentação automática via Swagger.

---
## 🔐 Autenticação

### Como autenticar?

1. Realize o login com o endpoint:

```
POST /api/Auth/login
```

**Body (JSON):**

```json
{
  "cpf": "12345678900",
  "senha": "suaSenha"
}
```

2. O retorno será um **token JWT**. Exemplo:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

3. No Swagger (ou nos headers das requisições), clique em **Authorize** e insira o token no formato:

```
Bearer {seu_token}
```

---

## 📚 Endpoints Disponíveis

### 🔐 Auth

#### `POST /api/Auth/login`

Autentica o usuário com Email e senha, e retorna um token JWT válido para as demais requisições.

---

### 👤 Usuario

> Todos os endpoints abaixo **exigem autenticação JWT**.

#### `GET /api/Usuario`

Retorna uma lista de todos os usuários cadastrados.

#### `POST /api/Usuario`

Cadastra um novo usuário.

**Body (exemplo):**

```json
{
  "nome": "João da Silva",
  "cpf": "12345678900",
  "Email": "teste@gmail.com",
  "senha": "senha123"
}
```

#### `PUT /api/Usuario`

Atualiza os dados de um usuário existente.

**Body (exemplo):**

```json
{
  "nome": "João da Silva",
  "cpf": "12345678900",
  "Email": "teste@gmail.com",
  "senha": "senha123"
}
```

#### `DELETE /api/Usuario`

Remove um usuário com base no corpo da requisição.

**Body (exemplo):**

```json
{
  "cpf": 1
}
```

#### `GET /api/Usuario/get-by-id/{id}`

Consulta um usuário específico pelo seu ID.

**Exemplo:**
`GET /api/Usuario/get-by-id/1`

#### `GET /api/Usuario/get-by-cpf/{cpf}`

Consulta um usuário específico pelo CPF.

**Exemplo:**
`GET /api/Usuario/get-by-cpf/12345678900`

---

## ⚙️ Configurações JWT

No `appsettings.json`, defina:

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
* DDD

---


Os membros do grupo são:

## - André Faria De Souza RA 2101106
## - Beatriz Aparecida Banaki De Campos RA 2210533
## - Carlos Fernando Dos Santos RA 1692984
## - Rafael De Palma Francisco RA 2465248
## - Sarah Kelly Almeida RA 1842293
