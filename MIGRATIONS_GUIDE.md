# Instruções para Executar as Migrações

## 1. Aplicar as Migrações no Banco MySQL

Certifique-se de que:
- O MySQL Server está rodando
- O banco `gerenciador_hotel` foi criado
- A connection string está correta no `appsettings.json`

Execute o comando:

```bash
dotnet ef database update
```

## 2. Estrutura do Banco Criada

As seguintes tabelas serão criadas:

### Tabelas do Sistema de Autenticação (ASP.NET Identity)
- `AspNetUsers` - Usuários do sistema
- `AspNetRoles` - Perfis/Funções
- `AspNetUserRoles` - Relacionamento usuário/perfil
- `AspNetUserClaims` - Claims dos usuários
- `AspNetRoleClaims` - Claims dos perfis
- `AspNetUserLogins` - Logins externos
- `AspNetUserTokens` - Tokens de usuário

### Tabelas do Sistema de Hotel
- `Paises` - Países dos hóspedes
- `Amenidades` - Amenidades disponíveis
- `Acomodacoes` - Quartos/Suítes do hotel
- `AcomodacaoAmenidades` - Relacionamento acomodação/amenidades
- `ImagensAcomodacao` - Imagens das acomodações
- `Reservas` - Reservas dos hóspedes
- `Pagamentos` - Pagamentos das reservas

## 3. Dados Iniciais (Seed Data)

Após aplicar as migrações, você pode inserir dados iniciais:

### Países Comuns
```sql
INSERT INTO Paises (Nome, Codigo, DataCriacao) VALUES 
('Brasil', 'BR', NOW()),
('Argentina', 'AR', NOW()),
('Chile', 'CL', NOW()),
('Uruguai', 'UY', NOW()),
('Estados Unidos', 'US', NOW()),
('Alemanha', 'DE', NOW()),
('França', 'FR', NOW()),
('Reino Unido', 'GB', NOW());
```

### Amenidades Básicas
```sql
INSERT INTO Amenidades (Nome, Descricao, Ativa, DataCriacao) VALUES 
('Wi-Fi Gratuito', 'Internet sem fio gratuita', 1, NOW()),
('Ar Condicionado', 'Climatização do ambiente', 1, NOW()),
('TV a Cabo', 'Televisão com canais por assinatura', 1, NOW()),
('Frigobar', 'Frigobar completo', 1, NOW()),
('Banheira', 'Banheira de hidromassagem', 1, NOW()),
('Varanda', 'Varanda com vista', 1, NOW()),
('Cofre', 'Cofre digital', 1, NOW()),
('Estacionamento', 'Vaga de estacionamento', 1, NOW());
```

## 4. Comandos Úteis para Migrations

### Criar nova migração
```bash
dotnet ef migrations add NomeDaMigracao
```

### Remover última migração
```bash
dotnet ef migrations remove
```

### Ver SQL da migração
```bash
dotnet ef migrations script
```

### Reverter migração
```bash
dotnet ef database update NomeDaMigracaoAnterior
```

### Ver status das migrações
```bash
dotnet ef migrations list
```

## 5. Verificação

Após aplicar as migrações, você pode verificar no MySQL Workbench se todas as tabelas foram criadas corretamente com os relacionamentos e índices apropriados.
