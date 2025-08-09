# Configuração do MySQL para o Gerenciador de Hotel

## Pré-requisitos

1. **MySQL Server** instalado e rodando
2. **MySQL Workbench** (opcional, para interface gráfica)

## Configuração

### 1. Criar o Banco de Dados

Conecte-se ao MySQL e execute:

```sql
CREATE DATABASE gerenciador_hotel CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 2. Configurar a Connection String

O arquivo `appsettings.json` já está configurado com:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=gerenciador_hotel;Uid=root;Pwd=;"
  }
}
```

**Ajuste os parâmetros conforme sua configuração:**
- `Server`: endereço do servidor MySQL (localhost se local)
- `Database`: nome do banco (gerenciador_hotel)
- `Uid`: usuário do MySQL (root ou outro)
- `Pwd`: senha do usuário (vazio se sem senha)

### 3. Executar as Migrações

```bash
dotnet ef database update
```

### 4. Testar a Conexão

```bash
dotnet run GerenciadorHotel.csproj
```

## Estrutura do Banco

O sistema criará automaticamente as tabelas:
- `AspNetUsers` - Usuários do sistema
- `AspNetRoles` - Perfis/níveis de acesso
- `AspNetUserRoles` - Relacionamento usuários/perfis
- Outras tabelas do Identity

## Próximos Passos

Após a configuração inicial, será necessário:
1. Criar models específicos do hotel (Acomodação, Reserva, etc.)
2. Adicionar novas migrações conforme necessário
3. Implementar seeders para dados iniciais

## Troubleshooting

### Erro de Conexão
- Verifique se o MySQL está rodando
- Confirme usuário e senha na connection string
- Teste a conexão via MySQL Workbench

### Erro de Charset
- Certifique-se que o banco foi criado com UTF8MB4
- Ajuste a connection string se necessário: `...;charset=utf8mb4;`
