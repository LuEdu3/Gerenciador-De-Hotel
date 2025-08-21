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
- Adicionar Acomodações - Luiz
- Adicionar filtros: Capacidade, Tipo de acomodação (Chalé, cabana, suíte, estacionamento e domo)
- Adicionar no banco mais capacidades de fotos - Iolanda

---

## Futuras Modificações (solicitadas)

Abaixo estão as modificações que você listou, o estado atual com base na inspeção do código e sugestões rápidas do que precisa ser feito para cada item.

### Como usuário

- Clicar na reserva vai direto pro quarto selecionado na página de reservas
	- Status: Pendente (prioridade alta)
	- Observação: Na view `Views/Reservas/MinhasReservas.cshtml` o botão atualmente direciona para os detalhes da reserva (`Reservas/Details/{reserva.Id}`), o que pode abrir uma tela genérica (ex.: sempre para o tipo "domo"). O comportamento desejado é abrir a página da acomodação associada à reserva.
	- Próximo passo: alterar o link do botão para apontar para a acomodação. Exemplo de alteração sugerida na view:

		```razor
		<a asp-controller="Acomodacoes" asp-action="Details" asp-route-id="@reserva.AcomodacaoId" class="btn btn-outline-primary btn-sm">
				<i class="bi bi-eye"></i> Ver Acomodação
		</a>
		```

		Ou substituir o botão atual `asp-action="Details" asp-route-id="@reserva.Id"` por esta versão que usa `AcomodacaoId`.
		Testar como hóspede e administrador após a alteração; garantir que a rota `Acomodacoes/Details/{id}` retorna `NotFound` (404) quando a acomodação não existir. - Pedro

- Mostrar amenidades nos detalhes da acomodação como usuário
	- Status: Implementado
	- Onde: `Views/Acomodacoes/Details.cshtml` já renderiza `Model.AcomodacaoAmenidades` com imagens e nomes.

- Mostrar camas de solteiros e de casal nos detalhes da acomodação
	- Status: Não implementado
	- Observação: O modelo `Acomodacao` atualmente só tem `QuantidadeCamas` (total). Não há informações separadas para camas de solteiro e casal.
	- Próximo passo: estender `Models/Acomodacao.cs` com campos como `QuantidadeCamasSolteiro` e `QuantidadeCamasCasal`, atualizar validações e views (Create/Edit/Details) e ajustar seed/migrations.

- Horário do check-in
	- Status: Parcial / Não apresentado ao usuário
	- Observação: `Reserva` usa `DataCheckIn`/`DataCheckOut` (DateTime) mas as views exibem apenas a data (`dd/MM/yyyy`). Não existe um campo dedicado de "horário do check-in" visível ao usuário.
	- Próximo passo: se desejar um campo separado, criar `HorarioCheckIn` em `Reserva` ou formatar `DataCheckIn` para exibir hora nas views; atualizar formulários e validações conforme necessário.

### Como Administrador

- Mostrar diferenças de cama de solteiro e cama de casal
	- Status: Não implementado
	- Observação: depende de modelagem (ver item acima). Sem campos separados não é possível apresentar diferenças.
	- Próximo passo: adicionar campos no model e novo UI no painel admin para exibir/filtrar por tipo de cama.

- Quantidades de cama deve ser maior ou igual a quantidades de cama de casal
	- Status: Não implementado
	- Observação: regra de validação depende da existência de `QuantidadeCamasCasal`. Após adicionar campos, aplicar DataAnnotation/validação customizada no `Acomodacao` e no servidor (Create/Edit POST).

- Escolher ficheiros deve ficar salvo os arquivos que ja foram upados
	- Status: Parcialmente implementado
	- Observação: O back-end já salva uploads em `wwwroot/imagens` e persiste `ImagemAcomodacao` (veja `Controllers/AcomodacoesController.cs`), e `Views/Acomodacoes/Details.cshtml` exibe as imagens. Entretanto o input `<input type="file">` não pode ser pre-populado por motivos de segurança do navegador. O que falta é uma UI de gerenciamento de imagens no Edit (listar arquivos já upados, permitir remover/definir principal) — assim o usuário verá e poderá reutilizar os arquivos já enviados.
	- Próximo passo: implementar painel de imagens em `Views/Acomodacoes/Edit.cshtml` que lista `Model.Imagens` com botões (remover, definir principal, mover ordem) e preservar essas alterações no POST.

- Ao visualizar a acomodação tem que aparecer as amenidades
	- Status: Implementado
	- Onde: `Views/Acomodacoes/Details.cshtml` já exibe as amenidades associadas.

---

## Mapeamento rápido das alterações no código (onde olhar)

- Exibir amenidades e fotos: `Views/Acomodacoes/Details.cshtml`
- Upload e persistência de imagens: `Controllers/AcomodacoesController.cs` (Create/Edit POST) e modelo `Models/ImagemAcomodacao.cs`
- Lista de reservas do usuário (link): `Views/Reservas/MinhasReservas.cshtml`
- Validação de reservas (datas e conflitos): `Controllers/ReservasController.cs`

## Próximos passos sugeridos (técnicos)

1. Para "clicar na reserva vai direto pro quarto": alterar `Views/Reservas/MinhasReservas.cshtml` e atualizar botão/link. Testar com usuário hóspede.
2. Para camas separadas: adicionar `QuantidadeCamasSolteiro` e `QuantidadeCamasCasal` em `Models/Acomodacao.cs`, criar migração EF Core, atualizar formulários Create/Edit e `Views/Acomodacoes/Details.cshtml` para exibir ambos os valores.
3. Implementar painel de gerenciamento de imagens em Edit (listar imagens já salvas, criar endpoints para remover/definir principal, e ajustar POST Edit para respeitar alterações de ordem/atributos).
4. Definir formato de exibição do horário de check-in (usar `DataCheckIn` ou campo separado) e atualizar views de reservas e detalhes.

Se quiser, eu já posso: (A) aplicar a mudança simples em `MinhasReservas.cshtml` para que o botão "Ver Detalhes" direcione para a acomodação; (B) adicionar os campos de cama ao model + migration inicial; ou (C) implementar o painel de imagens (essa é a maior tarefa entre as opções). Diga qual prefere que eu faça primeiro.


