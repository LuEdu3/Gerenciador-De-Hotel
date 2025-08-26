# Configuração do Railway - Gerenciador de Hotel

## 1. Variáveis de Ambiente Necessárias

Baseado na connection string do MySQL que você mostrou, configure as seguintes variáveis no Railway:

### Opção 1: Usando MYSQL_URL (Recomendado)
```
MYSQL_URL=mysql://root:zxdGCGxLDnGKJFkiereMhzuENaKyFrxn@mysql.railway.internal:3306/railway
```

### Opção 2: Variáveis Individuais (Fallback)
```
MYSQLHOST=mysql.railway.internal
MYSQLDATABASE=railway
MYSQLUSER=root
MYSQLPASSWORD=zxdGCGxLDnGKJFkiereMhzuENaKyFrxn
MYSQLPORT=3306
```

## 2. Como Configurar no Railway

1. **Acesse seu projeto no Railway**
2. **Vá na aba "Variables"**
3. **Adicione a variável MYSQL_URL** com o valor mostrado acima
4. **Salve as alterações**

## 3. Verificações

- ✅ O código já foi atualizado para ler essas variáveis
- ✅ As migrations são aplicadas automaticamente
- ✅ O SeedDataService criará a empresa padrão
- ✅ A aplicação está configurada para HTTPS e proxy

## 4. Após Deploy

1. Acesse sua aplicação na URL fornecida pelo Railway
2. Use as credenciais padrão:
   - **Email**: admin@empresa.com
   - **Senha**: Admin123!

## 5. Estrutura do Banco

O sistema criará automaticamente:
- Todas as tabelas necessárias
- Uma empresa padrão
- Um usuário administrador
- Dados iniciais para países e amenidades

## 6. Troubleshooting

Se houver problemas de conexão:
1. Verifique se a variável MYSQL_URL está configurada corretamente
2. Confirme que o serviço MySQL está ativo no Railway
3. Verifique os logs do deploy para erros específicos

## 7. Connection String gerada

Com a MYSQL_URL configurada, a aplicação gerará automaticamente:
```
Server=mysql.railway.internal;Port=3306;Database=railway;Uid=root;Pwd=zxdGCGxLDnGKJFkiereMhzuENaKyFrxn;SslMode=Preferred;
```
