# Roadmap - Sistema de Gerenciamento de Hotel (ASP.NET MVC 8.0)

Tecnologias: ASP.NET MVC 8.0 + MySQL + Entity Framework Core + Bootstrap

Status Geral: 87% concluído

## Progresso
- ✅ Módulos completos: Autenticação/Autorização; Usuários; Acomodações (imagens e validações); Amenidades; Países; Reservas (inclui Minhas Reservas); Relatórios (ocupação); Painel Admin.
- 🔄 Parcial: Interface responsiva (90%) — faltam tema escuro e navbar fechar no mobile.
- ❌ Pendentes: Integração com Pagamentos (0%); Recuperação de senha.

## Concluídos (com responsáveis)
- Pedro: Minhas Reservas.
- Luiz: Amenidades nos detalhes; Camas casal/solteiro separadas; Acesso às reservas; Remover número do quarto; Upload e visualização de imagens; Rotas DeleteConfirmed; Regras de capacidade e noites mínimas no Edit; Exibição de horário de check-in; Amenidades salvando.
- Guilherme: Amenidades visíveis nas acomodações.
- Iolanda: Capacidade por quarto; Mostrar mínimo de noites no Edit; Remover número do quarto.
- Victor: Design e filtros.

## Bugs corrigidos
- Exclusão de reserva (Admin/Usuário) — fluxo e rotas corrigidos.
- Exclusão de usuário (Admin) — corrigido.
- Edit respeita capacidade e noites mínimas — corrigido.
- Links DeleteConfirmed inexistentes — rotas compatíveis adicionadas.
- Amenidades não salvavam — seed/associações corrigidos.
- Exibição de horário de check-in — corrigido.

## Próximos passos
1) Trocar link “Ver Detalhes” em MinhasReservas para abrir a acomodação.
2) Gerenciamento avançado de imagens no Edit (reordenar/remover/definir principal).
3) UI: tema escuro; navbar fechar no mobile; diminuir fonte da descrição.
4) Redirecionar acesso não autorizado de Reservas/MinhasReservas para Index.
5) Implementar Integração com Pagamentos e Recuperação de senha.

## Arquivos principais
- Controllers: `AcomodacoesController.cs`, `ReservasController.cs`
- Views: `Acomodacoes/Details.cshtml`, `Reservas/MinhasReservas.cshtml`
- Seed: `Services/SeedDataService.cs`
