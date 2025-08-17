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

### 4. Amenidades ✅ **IMPLEMENTADO**
- ✅ Sistema de amenidades fixas (8 amenidades pré-definidas)
- ✅ Associação de amenidades às acomodações
- ✅ Integração com formulários de acomodação

### 5. Países ✅ **IMPLEMENTADO**
- ✅ Base de dados de países pré-carregada no sistema
- ✅ Associação automática de país às reservas
- ✅ Sistema de seed otimizado (preserva dados existentes)

### 6. Reservas ✅ **IMPLEMENTADO**
- ✅ Criar/editar/excluir reservas
- ✅ Dados do hóspede (nome, sobrenome, email, telefone)
- ✅ Pedidos especiais
- ✅ Associação de reserva à acomodação e país
- ✅ "Minhas Reservas" para hóspedes
- ✅ Cancelamento de reservas por hóspedes
- ✅ Visualização de histórico de reservas
- ✅ Estados visuais por status da reserva
- ✅ Validação de disponibilidade de quartos (bloqueio de conflitos)
- ✅ Cálculo automático de valor total
- ✅ Associação automática de reservas ao usuário logado
- ✅ Check-in/check-out com atualização automática do status da acomodação
- ✅ Autorização: hóspedes restritos às próprias reservas

**✅ FUNCIONALIDADES COMPLETAS:**
- ✅ **Segurança**: Acesso de hóspedes restrito ao endpoint `/Reservas` (mantendo acesso apenas ao `/Reservas/MinhasReservas`)
- ✅ **Validação de Conflitos**: Verificação implementada para impedir reservas sobrepostas do mesmo quarto (ignora reservas canceladas)
- ✅ **Check-in/Check-out**: Sistema completo com atualização automática do status da acomodação
- ✅ **Autorização por Perfil**: Hóspedes só podem ver/editar/cancelar suas próprias reservas

### 7. Relatórios
- ✅Relatório de ocupação
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
- [x] Implementação da autenticação nativa
- [x] Cadastro e gestão de usuários e níveis de acesso
- [x] Controllers com autorização por roles
- [x] Dashboard com controle de acesso
- [x] Cadastro e gestão de acomodações (CRUD completo com views modernas)
- [x] Sistema de amenidades fixas e associação com acomodações
- [x] Gestão de países (sistema de seed otimizado e base pré-carregada)
- [x] Sistema de reservas completo (com "Minhas Reservas" para hóspedes)
- [x] Restrição de acesso de hóspedes ao endpoint `/Reservas` (segurança implementada)
- [x] Validação de conflitos de reservas por acomodação/período (bloqueio de sobreposição)
- [x] Check-in e check-out (com atualização automática do status da acomodação)
- [ ] Relatórios financeiros e de hóspedes
- [ ] Painel administrativo avançado
- [ ] Interface responsiva com Bootstrap (melhorias adicionais)
- [ ] Integração com pagamentos

### Progresso Atual: 83% concluído

> O progresso é calculado automaticamente com base nas tarefas marcadas como concluídas.

---

Este roadmap será atualizado conforme avançarmos no desenvolvimento.
