# Roadmap - Sistema de Gerenciamento de Hotel (ASP.NET MVC 8.0)

## Tecnologias
- ASP.NET MVC 8.0
- Autenticação nativa ASP.NET
- Data Annotations (validação e metadados)
- MySQL (banco de dados)
- Entity Framework Core (ORM)
- Bootstrap (UI responsiva)


## Funcionalidades Sugeridas (Baseadas no Diagrama)

### 1. Autenticação e Autorização [✅ 100%]
- ✅ Login/Logout
- ✅ Cadastro de usuários (admin, recepcionista, hóspede)
- ❌ Recuperação de senha
- ✅ Níveis de acesso (admin, recepcionista, etc)

### 2. Gestão de Usuários [✅ 100%]
- ✅ Cadastro/edição/exclusão de funcionários
- ✅ Controle de status do funcionário
- ✅ Visualização de dados pessoais e nível de acesso

### 3. Gestão de Acomodações [✅ 100%]
- ✅ Cadastro/edição/exclusão de acomodações
- ✅ Descrição, quantidade de camas, preço, imagens
- ✅ Check-in/check-out por acomodação
- ✅ Mínimo de noites
- ✅ Status da acomodação

### 4. Amenidades [✅ 100%]
- ✅ Sistema de amenidades fixas (8 amenidades pré-definidas)
- ✅ Associação de amenidades às acomodações
- ✅ Integração com formulários de acomodação

### 5. Países [✅ 100%]
- ✅ Base de dados de países pré-carregada no sistema
- ✅ Associação automática de país às reservas
- ✅ Sistema de seed otimizado (preserva dados existentes)

### 6. Reservas [✅ 100%]
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

### 7. Relatórios [🔄 33%]
- ✅ Relatório de ocupação
- ❌ Relatório financeiro
- ❌ Relatório de hóspedes

### 8. Painel Administrativo [🔄 50%]
- ✅ Dashboard com estatísticas
- ✅ Gerenciamento de usuários

### 9. Interface Responsiva [✅ 90%]
- ✅ Layout em Bootstrap
- ❌ Temas claros/escuros

### 10. Integração com Pagamentos [❌ 0%]
- ❌ Registro de pagamentos
- ❌ Integração com gateways (futuro)

---

## Progresso Detalhado

**✅ MÓDULOS COMPLETOS (100%):**
- [x] **Autenticação e Autorização** - Sistema completo com níveis de acesso (falta apenas recuperação de senha)
- [x] **Gestão de Usuários** - CRUD completo com autorização e controle de status
- [x] **Gestão de Acomodações** - CRUD completo com views modernas, validações e check-in/out
- [x] **Sistema de Amenidades** - Amenidades fixas e associação com acomodações
- [x] **Gestão de Países** - Base pré-carregada com sistema de seed otimizado
- [x] **Sistema de Reservas** - CRUD completo, "Minhas Reservas", validação de conflitos, autorização

**🔄 MÓDULOS EM ANDAMENTO:**
- [🔄] **Relatórios (33%)** - Apenas relatório de ocupação implementado
- [🔄] **Painel Administrativo (50%)** - Dashboard básico e gerenciamento de usuários
- [🔄] **Interface Responsiva (90%)** - Bootstrap implementado, falta tema escuro

**❌ MÓDULOS PENDENTES:**
- [❌] **Integração com Pagamentos (0%)** - Não iniciado

### Progresso Geral: 87% concluído

**Detalhamento por funcionalidade:**
- Estrutura do projeto: ✅ 100%
- Configuração MySQL/EF: ✅ 100%
- Models e Migrations: ✅ 100%
- Autenticação ASP.NET: ✅ 95% (falta recuperação de senha)
- Sistema de Usuários: ✅ 100%
- Sistema de Acomodações: ✅ 100%
- Sistema de Reservas: ✅ 100%
- Amenidades e Países: ✅ 100%
- Segurança e Autorização: ✅ 100%
- Validações de Negócio: ✅ 100%
- Relatórios: 🔄 33%
- Interface UI/UX: ✅ 90%
- Pagamentos: ❌ 0%

> O progresso é calculado automaticamente com base nas tarefas marcadas como concluídas.

---

Este roadmap será atualizado conforme avançarmos no desenvolvimento.
  
---

## Próximos Passos - Banco de Dados

- Dados da empresa (Nome, logo, contato, email e galeria para fotos) - Guilherme
- Adicionar Acomodações
- Adicionar filtros: Capacidade, Tipo de acomodação (Chalé, cabana, suíte, estacionamento e domo)
- Adicionar no banco mais capacidades de fotos