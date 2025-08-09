# Roadmap - Sistema de Gerenciamento de Hotel (ASP.NET MVC 8.0)

## Tecnologias
- ASP.NET MVC 8.0
- Autenticação nativa ASP.NET
- Data Annotations (validação e metadados)
- MySQL (banco de dados)
- Entity Framework Core (ORM)
- Bootstrap (UI responsiva)


## Funcionalidades Sugeridas (Baseadas no Diagrama)

### 1. Autenticação e Autorização
- Login/Logout
- Cadastro de usuários (admin, recepcionista, hóspede)
- Recuperação de senha
- Níveis de acesso (admin, recepcionista, etc)

### 2. Gestão de Usuários
- Cadastro/edição/exclusão de funcionários
- Controle de status do funcionário
- Visualização de dados pessoais e nível de acesso

### 3. Gestão de Acomodações
- Cadastro/edição/exclusão de acomodações
- Descrição, quantidade de camas, preço, imagens
- Check-in/check-out por acomodação
- Mínimo de noites
- Status da acomodação

### 4. Amenidades
- Cadastro/edição/exclusão de amenidades
- Associação de amenidades às acomodações
- Imagens de amenidades

### 5. Reservas
- Criar/editar/excluir reservas
- Dados do hóspede (nome, sobrenome, email, telefone)
- Pedidos especiais
- Associação de reserva à acomodação e país
- Visualizar reservas por período
- Cancelamento de reservas

### 6. Países
- Cadastro/edição/exclusão de países
- Associação de país ao hóspede/reserva

### 7. Relatórios
- Relatório de ocupação
- Relatório financeiro
- Relatório de hóspedes

### 8. Painel Administrativo
- Dashboard com estatísticas
- Gerenciamento de usuários

### 9. Interface Responsiva
- Layout em Bootstrap
- Temas claros/escuros

### 10. Integração com Pagamentos
- Registro de pagamentos
- Integração com gateways (futuro)

---

## Progresso

- [x] Estrutura inicial do projeto ASP.NET MVC 8.0
- [x] Configuração do MySQL e Entity Framework
- [x] Criação dos Models com Data Annotations
- [x] Migrations criadas para o banco de dados
- [ ] Implementação da autenticação nativa
- [ ] Cadastro e gestão de usuários e níveis de acesso
- [ ] Cadastro e gestão de acomodações
- [ ] Cadastro e associação de amenidades
- [ ] Sistema de reservas (com país do hóspede)
- [ ] Gestão de países
- [ ] Check-in e check-out
- [ ] Relatórios
- [ ] Painel administrativo
- [ ] Interface responsiva com Bootstrap
- [ ] Integração com pagamentos

### Progresso Atual: 33% concluído

> O progresso é calculado automaticamente com base nas tarefas marcadas como concluídas.

---

Este roadmap será atualizado conforme avançarmos no desenvolvimento.
