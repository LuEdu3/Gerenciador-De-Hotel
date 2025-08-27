# Gerenciador-De-Hotel

## Visão Geral
O **Gerenciador-De-Hotel** é um sistema web completo para gestão de hotéis, pousadas e similares, desenvolvido em ASP.NET Core MVC. O sistema oferece controle de acomodações, reservas, check-in/check-out, gestão de usuários, relatórios, informações da empresa e muito mais, com autenticação e autorização robustas.

## Funcionalidades Principais
- **Gestão de Acomodações:** Cadastro, edição, exclusão e visualização de quartos, com controle de status e amenidades.
- **Reservas:** Criação, visualização, cancelamento e histórico de reservas, incluindo área exclusiva "Minhas Reservas" para hóspedes.
- **Check-in/Check-out:** Controle de fluxo de hóspedes, atualização automática do status das acomodações.
- **Gestão de Usuários:** CRUD completo de usuários, reset de senha, atribuição de roles (Administrador, Recepcionista, Hóspede).
- **Relatórios:** Relatórios de ocupação, financeiros e outros, acessíveis para administradores.
- **Gestão da Empresa:** Cadastro e edição de informações institucionais, fotos, serviços, prêmios e contatos.
- **Autenticação e Autorização:** Login seguro, controle de acesso por perfil, navegação dinâmica conforme o papel do usuário.
- **Interface Responsiva:** Layout moderno, navegação intuitiva e componentes reutilizáveis.

## Estrutura do Projeto
- **Controllers:**
	- `AcomodacoesController`: Gerencia acomodações e amenidades.
	- `ReservasController`: Gestão de reservas, "Minhas Reservas" e cancelamentos.
	- `CheckInController`: Controle de check-in/check-out.
	- `UsuariosController`: Administração de usuários e permissões.
	- `EmpresaController`: Gerenciamento das informações da empresa.
	- `RelatoriosController`: Relatórios administrativos.
	- `AuthController`: Autenticação e login.
	- `HomeController`: Página inicial, dashboard e navegação pública.
- **Models:**
	- `Acomodacao`, `Reserva`, `ApplicationUser`, `Empresa`, `Amenidade`, `Pagamento`, entre outros.
- **Services:**
	- `EmpresaService`: Lógica de negócio para empresa, cache, CRUD, etc.
	- `SeedDataService`: Popular dados iniciais.
- **ViewModels:**
	- Estruturas para facilitar o binding e validação de dados nas views.
- **Views:**
	- Páginas para cada funcionalidade, com navegação adaptada ao perfil do usuário.
- **ViewComponents:**
	- Componentes reutilizáveis, como informações da empresa no header.

## Papéis e Permissões
- **Administrador:** Acesso total ao sistema, incluindo gestão de usuários, acomodações, reservas, relatórios e empresa.
- **Recepcionista:** Gestão operacional de reservas, check-in/check-out e visualização de acomodações.
- **Hóspede:** Área "Minhas Reservas", visualização de acomodações e informações públicas.

## Destaques Técnicos
- Autorização granular por ação e perfil.
- Seeds automáticos para dados iniciais.
- Interface responsiva e moderna.
- Boas práticas de segurança e validação.

## Como Executar
1. **Aplicar migrations:**
	 ```bash
	 dotnet ef database update
	 ```
2. **Executar o projeto:**
	 ```bash
	 dotnet run GerenciadorHotel.csproj
	 ```
3. **Acessar:**
	 - Admin: admin@hotel.com / Admin123!
	 - Recepcionista: recepcionista@hotel.com / Recep123!

## Roadmap e Próximos Passos
Consulte o arquivo `Roadmap.md` para ver bugs corrigidos, melhorias planejadas e próximos recursos.

---
