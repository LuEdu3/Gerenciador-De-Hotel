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

### 5. Reservas ✅ **IMPLEMENTADO**
- ✅ Criar/editar/excluir reservas
- ✅ Dados do hóspede (nome, sobrenome, email, telefone)
- ✅ Pedidos especiais
- ✅ Associação de reserva à acomodação e país
- ✅ "Minhas Reservas" para hóspedes
- ✅ Cancelamento de reservas por hóspedes
- ✅ Visualização de histórico de reservas
- ✅ Estados visuais por status da reserva
- ✅ Validação de disponibilidade de quartos
- ✅ Cálculo automático de valor total
- ✅ Associação automática de reservas ao usuário logado

**⚠️ PENDÊNCIAS IMPORTANTES:**
- 🔒 **Segurança**: Restringir acesso de hóspedes ao endpoint `/Reservas` (mantendo acesso apenas ao `/Reservas/MinhasReservas`)
- 🚫 **Validação de Conflitos**: Implementar verificação para impedir reservas sobrepostas do mesmo quarto (só permitir se reserva anterior foi cancelada)

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

## 🔧 Melhorias Prioritárias Identificadas

### 🔒 Segurança e Autorização
1. **Restrição de Acesso a Endpoints**
   - **Problema**: Hóspedes podem acessar `/Reservas` (listagem geral)
   - **Solução**: Adicionar `[Authorize(Roles = "Administrador,Recepcionista")]` na action `Index`
   - **Manter**: Acesso livre ao `/Reservas/MinhasReservas` para hóspedes

### 📅 Validação de Reservas
2. **Conflitos de Reservas**
   - **Problema**: Sistema permite reservas sobrepostas para a mesma acomodação
   - **Impacto**: Duas pessoas podem reservar o mesmo quarto no mesmo período
   - **Solução Necessária**: 
     - Verificar conflitos de datas antes de confirmar reserva
     - Considerar apenas reservas ativas (não canceladas)
     - Implementar bloqueio durante o processo de reserva

### 🎯 Regras de Negócio
3. **Estados de Reserva**
   - **Implementado**: Sistema de cancelamento para hóspedes
   - **Funcional**: Administradores e recepcionistas podem gerenciar todas as reservas
   - **Validação**: Reservas canceladas liberam automaticamente a acomodação

---

## Progresso

- [x] Estrutura inicial do projeto ASP.NET MVC 8.0
- [x] Configuração do MySQL e Entity Framework
- [x] Criação dos Models com Data Annotations
- [x] Migrations criadas para o banco de dados
- [x] Implementação da autenticação nativa
- [x] Cadastro e gestão de usuários e níveis de acesso
- [x] Controllers com autorização por roles
- [x] Dashboard com controle de acesso
- [x] Cadastro e gestão de acomodações (CRUD completo com views modernas)
- [x] Sistema de reservas completo (com "Minhas Reservas" para hóspedes)
- [ ] **PRIORIDADE**: Restrição de acesso de hóspedes ao endpoint `/Reservas`
- [ ] **PRIORIDADE**: Validação de conflitos de reservas por acomodação/período
- [ ] Cadastro e associação de amenidades
- [ ] Gestão de países
- [ ] Check-in e check-out
- [ ] Relatórios
- [ ] Painel administrativo
- [ ] Interface responsiva com Bootstrap
- [ ] Integração com pagamentos

### Progresso Atual: 67% concluído

> O progresso é calculado automaticamente com base nas tarefas marcadas como concluídas.

---

Este roadmap será atualizado conforme avançarmos no desenvolvimento.
