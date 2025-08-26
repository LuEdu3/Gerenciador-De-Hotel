# Roadmap - Sistema de Gerenciamento de Hotel (ASP.NET MVC 8.0)

## Tecnologias
- ASP.NET MVC 8.0 + MySQL + Entity Framework Core + Bootstrap

## Status Geral: 87% Concluído

### ✅ MÓDULOS COMPLETOS (100%)
- **Autenticação e Autorização** - Login/logout, níveis de acesso (admin, recepcionista, hóspede)
- **Gestão de Usuários** - CRUD completo com controle de status
- **Gestão de Acomodações** - CRUD, upload de imagens, check-in/out, validações
- **Sistema de Amenidades** - 8 amenidades fixas com associação às acomodações  
- **Gestão de Países** - Base pré-carregada (seed otimizado)
- **Sistema de Reservas** - CRUD completo, "Minhas Reservas", validação de conflitos
- **Relatórios** - Apenas relatório de ocupação
- **Painel Administrativo** - Dashboard básico

### 🔄 MÓDULOS PARCIAIS
- **Interface Responsiva (90%)** - Falta tema escuro

### ❌ MÓDULOS PENDENTES
- **Integração com Pagamentos (0%)** - Não iniciado
- **Recuperação de senha** - Não implementado

---

## Modificações Pendentes


### Para Usuários
**Pedro ✅**
- ✅ "Minhas Reservas" implementado
- ✅ **Clicar na reserva vai direto ao quarto** - botão ainda aponta para detalhes da reserva
  - Alterar: `Views/Reservas/MinhasReservas.cshtml` linha ~154: `asp-action="Details" asp-route-id="@reserva.Id"` → `asp-controller="Acomodacoes" asp-action="Details" asp-route-id="@reserva.AcomodacaoId"`

**Luiz ✅**  
- ✅ Amenidades nos detalhes - implementado com seed automático
- ✅ Camas solteiro/casal separadas - modelo atualizado


- ✅ Acesso às reservas implementado

- ✅ Retirar esse número do quarto na acomodacão.





### Para Administradores  
**Luiz**
- ✅ Upload de imagens funcional (`Controllers/AcomodacoesController.cs` + `wwwroot/imagens`)
- ✅ Visualização de imagens em Edit (`Views/Acomodacoes/Edit.cshtml`)  
- ✅ **Gerenciamento avançado de imagens** - falta UI para reordenar/remover/definir principal no Edit

**Guilherme ✅**
- ✅ Amenidades visíveis nas acomodações

**Iolanda**
- ✅ Capacidade de pessoas em cada quarto.

- ✅ Quantidades de hospedes que suporta ao editar

- ✅ Retirar esse número do quarto na acomodacao

- ✅ Mostrar ao editar a quantidade mínimas de noites

---

## Lembretes

- Mudar bloqueio de acesso ao endpoint 'Reservas/MinhasReservas" para redirecionamento para o index

## Próximos Passos Técnicos

1. **Fix simples**: Alterar link "Ver Detalhes" em MinhasReservas para ir à acomodação
2. **Melhorar gerenciamento de imagens**: Expandir painel em Edit para gerenciar imagens existentes
3. **Horário check-in**: Definir se usar campo separado ou formatar `DataCheckIn` para mostrar hora

## Arquivos Principais
- Controllers: `AcomodacoesController.cs`, `ReservasController.cs`  
- Views: `Acomodacoes/Details.cshtml`, `Reservas/MinhasReservas.cshtml`
- Seed: `Services/SeedDataService.cs`

## Próximos Passos Técnicos

1. **Fix simples**: Alterar link "Ver Detalhes" em MinhasReservas para ir à acomodação
2. **Melhorar gerenciamento de imagens**: Expandir painel em Edit para gerenciar imagens existentes
3. **Horário check-in**: Definir se usar campo separado ou formatar `DataCheckIn` para mostrar hora

## Arquivos Principais
- Controllers: `AcomodacoesController.cs`, `ReservasController.cs`  
- Views: `Acomodacoes/Details.cshtml`, `Reservas/MinhasReservas.cshtml`
- Seed: `Services/SeedDataService.cs`


## Bugs ou Melhoras

1. Endpoint /Reservas/Delete: Ao tentar excluir uma reserva como Administrador, o site não funciona corretamente. Dropshadow bootstrap - Luiz

2. Ao tentar excluir um usuário como Administrador o site buga. Dropshadow bootstrap - Luiz

3. Ao editar ele sobreescreve as regras de capacidade e noites mínimas.

4. Ao tentar excluir uma reserva como usuario o site buga. Dropshadow bootstrap - Luiz

5. **Horário do check-in** - apenas data é exibida, não hora específica

6. Diminuir fonte da descrição

7. **Navbar não fecha no mobile**

Futuramente. Ao tentar resetar a senha de um usuário como Administrador o site buga. Dropshadow bootstrap
