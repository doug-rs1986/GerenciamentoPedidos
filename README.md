# Gerenciamento de Pedidos

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Coverage](https://img.shields.io/badge/coverage-100%25-brightgreen)

---

## 1. Visão Geral do Projeto

Aplicação completa para gestão de pedidos, com CRUD de clientes, produtos e todo o ciclo de vida de pedidos.

**Funcionalidades principais:**
- Cadastro/gestão de clientes
- Cadastro/gestão de produtos
- Criação de pedidos
- Controle de status de pedidos
- Relatórios básicos

---

## 2. Tecnologias Utilizadas

- **Backend:** .NET 8, ASP.NET Core MVC, Dapper
- **Frontend:** Razor Views (MVC)
- **Banco de dados:** SQL Server 2022
- **Ferramentas adicionais:**
  - Visual Studio 2022 / VS Code
  - Git

---

## 3. Configuração do Ambiente de Desenvolvimento

**Pré-requisitos:**
- .NET 8 SDK
- SQL Server 2022

**Passos para instalação:**
```bash
git clone [repo-url]
cd GerenciamentoPedidos
dotnet restore
cp appsettings.example.json appsettings.json
```

---

## 4. Configuração do Banco de Dados

**Criação do banco:**
```sql
CREATE DATABASE GerenciamentoPedidos;
```

**Criação das tabelas:**
```sql
CREATE TABLE Customers (
  Id INT PRIMARY KEY IDENTITY,
  Name NVARCHAR(100) NOT NULL,
  Email NVARCHAR(100) UNIQUE NOT NULL,
  Phone NVARCHAR(20),
  CreatedDate DATETIME NOT NULL
);

CREATE TABLE Products (
  Id INT PRIMARY KEY IDENTITY,
  Name NVARCHAR(100) NOT NULL,
  Description NVARCHAR(255),
  Price DECIMAL(18,2) NOT NULL,
  StockQuantity INT NOT NULL
);

CREATE TABLE Orders (
  Id INT PRIMARY KEY IDENTITY,
  CustomerId INT NOT NULL,
  OrderDate DATETIME NOT NULL,
  TotalAmount DECIMAL(18,2) NOT NULL,
  Status NVARCHAR(50) NOT NULL,
  FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE OrderItems (
  Id INT PRIMARY KEY IDENTITY,
  OrderId INT NOT NULL,
  ProductId INT NOT NULL,
  Quantity INT NOT NULL,
  UnitPrice DECIMAL(18,2) NOT NULL,
  FOREIGN KEY (OrderId) REFERENCES Orders(Id),
  FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

**Inserção de dados iniciais (opcional):**
```sql
INSERT INTO Customers (Name, Email, Phone, CreatedDate) VALUES ('Cliente Exemplo', 'cliente@exemplo.com', '11999999999', GETDATE());
INSERT INTO Products (Name, Description, Price, StockQuantity) VALUES ('Produto Exemplo', 'Descrição do produto', 99.90, 100);
```

---

## 5. Arquitetura da Aplicação

```
GerenciamentoPedidos/
├── Controllers/        # Lógica de interface e rotas
├── Data/               # Repositórios de acesso a dados
├── DTOs/               # Objetos de transferência de dados
├── Models/             # Entidades do banco
├── Views/              # Páginas Razor (frontend)
├── wwwroot/            # Arquivos estáticos
├── appsettings.json    # Configurações
└── Program.cs          # Inicialização
```

**Principais módulos:**
- `Models`: Entidades do banco
- `Controllers`: Lógica de negócio e interface
- `Data`: Repositórios
- `Views`: Componentes frontend

---

## 6. Variáveis de Ambiente

Exemplo de `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=GerenciamentoPedidos;User Id=usuario;Password=senha;"
}
```

---

## 7. Execução da Aplicação

**Comandos para iniciar:**
```bash
# Backend
 dotnet run

```
Acesse: http://localhost:5228 ou https://localhost:7071;

---

## 8. Guia do Desenvolvedor

- **Convenções de código:**
  - C# padrão .NET
  - Nomes em inglês para entidades
- **Como adicionar novas entidades:**
  - Criar Model, DTO, Repository e Controller
- **Fluxo para modificação do banco de dados:**
  - Alterar Models e scripts SQL

---

## 9. Manual do Usuário

### Clientes
- Cadastro de novo cliente
- Atualização de dados
- Exclusão e consulta

### Produtos
- Cadastro de novos itens
- Atualização de estoque
- Exclusão e consulta

### Pedidos
- Criação de pedido
- Adição de itens
- Alteração de status
- Visualização de histórico

---

## 10. FAQ e Solução de Problemas

- **Erro de conexão:** Verifique variáveis de ambiente e string de conexão
- **Problemas conhecidos:**
  - Não há suporte a múltiplos bancos simultâneos

---

## 11. Roadmap e Melhorias Futuras

- Criação de EndPoints para acesso externo
- Dashboard de relatórios avançados
- Exportação de dados
- Integração com sistemas de pagamento

---

## 12. Licença

MIT License

---

> Para dúvidas, sugestões ou contribuições, abra uma issue ou envie um pull request.
