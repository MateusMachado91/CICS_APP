# PYB CICS - Sistema de Solicitações

## 📋 Sobre o Projeto

Este projeto é uma migração modernizada do sistema legado em Visual Basic 6 para C#/.NET/Blazor Server. O sistema gerencia solicitações para tabelas CICS (Customer Information Control System) do BANRISUL, permitindo o controle e acompanhamento de mudanças nos ambientes CICS.

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture**, organizado em camadas:

### 📁 Estrutura do Projeto

```
├── PYBWeb.Domain/              # Camada de Domínio
│   ├── Entities/               # Entidades do negócio
│   ├── Enums/                  # Enumerações
│   └── Interfaces/             # Interfaces de repositório
├── PYBWeb.Application/         # Camada de Aplicação (futura)
├── PYBWeb.Infrastructure/      # Camada de Infraestrutura
│   ├── Data/                   # DbContext e configurações EF
│   └── Repositories/           # Implementações dos repositórios
├── PYBWeb.Web/                 # Interface Web Blazor Server
│   ├── Components/             # Componentes Blazor
│   └── Pages/                  # Páginas da aplicação
└── PYBWeb.Tests/               # Testes unitários
```

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework principal (compatível com .NET 8 e superior)
- **Blazor Server** - Interface web interativa
- **Entity Framework Core** - ORM para acesso a dados
- **SQLite** - Banco de dados
- **Bootstrap 5** - Framework CSS para UI responsiva
- **Bootstrap Icons** - Ícones

## 📊 Funcionalidades

### ✅ Implementadas
- **Dashboard** com estatísticas das solicitações
- **Listagem de Solicitações** com filtros por status, tipo de tabela e busca por texto
- **Escolha do Ambientes CICS**
- **Tipos de Tabela CICS**:
  - **DCT** - Destination Control Table
  - **FCT** - File Control Table  
  - **PCT** - Program Control Table
  - **PPT** - Processing Program Table

### 🔄 Status das Solicitações
- Pendente
- Em Análise
- Aprovada
- Rejeitada
- Implementada
- Cancelada

### 📋 Próximas Funcionalidades
- Criação e edição de solicitações
- Gerenciamento de usuários
- Sistema de autenticação e autorização
- Histórico de alterações
- Anexos de arquivos
- Relatórios

## 🛠️ Configuração do Ambiente

### Pré-requisitos
- .NET 8.0 SDK ou superior
- SQL Server ou SQL Server LocalDB
- Visual Studio Code (recomendado)

### ⚠️ Instalação do .NET 8

Se você ainda não tem o .NET 8 instalado:

1. **Baixe o .NET 8 SDK**: [Download .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Instale a versão SDK** (que inclui o Runtime)
3. **Verifique a instalação**:
```bash
dotnet --version
dotnet --list-runtimes
```

### 📦 Compatibilidade
- ✅ **.NET 8.0** - Versão alvo
- ✅ **.NET 9.0** - Compatível
- ✅ **Entity Framework Core 8.0**

### 🔧 Configuração do Banco de Dados

1. **Atualize a string de conexão** no arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PYBWebDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```

2. **Execute as migrações** para criar o banco:
```bash
dotnet ef migrations add InitialCreate --project PYBWeb.Infrastructure --startup-project PYBWeb.Web
dotnet ef database update --project PYBWeb.Infrastructure --startup-project PYBWeb.Web
```

### ▶️ Executando o Projeto

1. **Compile o projeto**:
```bash
dotnet build
```

2. **Execute a aplicação**:
```bash
dotnet run --project PYBWeb.Web
```

3. **Acesse no navegador**: `https://localhost:5001` ou `http://localhost:5000`

### 📋 Tasks do VS Code

O projeto inclui tasks configuradas:
- **build** - Compila a solution
- **run blazor** - Executa a aplicação Blazor Server

## 📚 Entidades Principais

### 🏢 Ambiente
Representa os ambientes CICS (Desenvolvimento, Teste, Homologação, Produção)

### 📄 Solicitacao
Solicitação principal contendo informações sobre mudanças nas tabelas CICS

### 📝 ItemSolicitacao
Itens específicos dentro de uma solicitação (entradas das tabelas CICS)

### 👤 Usuario
Usuários do sistema com diferentes níveis de permissão

### 📋 HistoricoSolicitacao
Registro de todas as alterações de status das solicitações

### 📎 AnexoSolicitacao
Arquivos anexados às solicitações

## 🔄 Migração do VB6

Esta aplicação moderniza o sistema legado VB6 mantendo:
- ✅ **Funcionalidades originais** do sistema CICS
- ✅ **Estrutura de dados** similar ao sistema antigo
- ✅ **Fluxo de trabalho** conhecido pelos usuários
- ✅ **Interface moderna** e responsiva
- ✅ **Arquitetura escalável** para futuras expansões

## 🧪 Dados de Teste

O sistema inclui dados iniciais (seed data):
- 4 ambientes CICS padrão
- Usuário administrador padrão (login: admin)

## 📈 Próximos Passos

1. **Autenticação e Autorização**
2. **CRUD completo de Solicitações**
3. **Gestão de Usuários**
4. **Sistema de Aprovação**
5. **Relatórios e Dashboards**
6. **API REST** para integrações
7. **Testes automatizados**

## 🤝 Contribuição

Para contribuir com o projeto:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é propriedade do BANRISUL e está sujeito às políticas internas da instituição.

---

**Desenvolvido para BANRISUL** - Sistema de Gerenciamento de Solicitações CICS
