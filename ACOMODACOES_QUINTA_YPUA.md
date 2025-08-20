# Acomodações da Quinta do Ypuã - Seed Data

Este arquivo documenta as acomodações criadas baseadas no site da Pousada Quinta do Ypuã (https://www.quintadoypua.com).

## Acomodações Implementadas

### 1. Domo
- **Descrição**: Uma experiência única em formato geodésico com vista panorâmica da natureza. Ideal para casais que buscam algo diferenciado e intimista.
- **Capacidade**: 1 cama
- **Preço**: R$ 280,00 por noite
- **Mínimo de noites**: 2
- **Status**: Disponível

### 2. Charrua (Bus)
- **Descrição**: Acomodação única em ônibus convertido, oferecendo uma experiência alternativa e sustentável. Perfeita para aventureiros.
- **Capacidade**: 2 camas
- **Preço**: R$ 180,00 por noite
- **Mínimo de noites**: 1
- **Status**: Disponível

### 3. Suíte com Cozinha
- **Descrição**: Suíte completa com cozinha equipada, ideal para estadias mais longas. Oferece conforto e praticidade para famílias ou casais.
- **Capacidade**: 1 cama
- **Preço**: R$ 350,00 por noite
- **Mínimo de noites**: 2
- **Status**: Disponível

### 4. Chalé Família
- **Descrição**: Chalé espaçoso ideal para famílias, com múltiplas camas e área de convivência. Ambiente aconchegante em meio à natureza.
- **Capacidade**: 4 camas
- **Preço**: R$ 450,00 por noite
- **Mínimo de noites**: 2
- **Status**: Disponível

### 5. Cabana
- **Descrição**: Cabana rústica e aconchegante, perfeita para quem busca uma conexão mais próxima com a natureza. Ideal para casais.
- **Capacidade**: 1 cama
- **Preço**: R$ 220,00 por noite
- **Mínimo de noites**: 1
- **Status**: Disponível

### 6. Estacionamento para Overlanders
- **Descrição**: Área especial para veículos de viajantes overlanders, com infraestrutura básica e acesso a banheiros compartilhados.
- **Capacidade**: 2 camas
- **Preço**: R$ 80,00 por noite
- **Mínimo de noites**: 1
- **Status**: Disponível

## Informações da Pousada Original

- **Nome**: Pousada Quinta do Ypuã
- **Localização**: Estrada Ipua, nº 6, Laguna - SC, 88790-000
- **Contato**: (48) 99940-9732
- **Email**: pousadaquintadoypua@gmail.com
- **Redes Sociais**: 
  - Facebook: /pousadaquintadoypua
  - Instagram: @pousadaquintadoypua

## Implementação no Sistema

As acomodações foram implementadas através do `SeedDataService.cs` e são inseridas automaticamente na primeira execução da aplicação, caso não existam acomodações no banco de dados.

### Localização do Código
- **Service**: `Services/SeedDataService.cs` - método `SeedAcomodacoes()`
- **Ativação**: `Program.cs` - chamada do seed durante inicialização
- **Model**: `Models/Acomodacao.cs`

### Estrutura de Dados
Cada acomodação possui os seguintes campos:
- Nome
- Descrição
- Quantidade de Camas
- Preço por Noite
- Mínimo de Noites
- Status (Disponível, Ocupada, Manutenção, Fora de Serviço)
- URL da Imagem Principal
- Ativa (bool)
- Data de Criação/Atualização

## Nota Técnica

Devido às limitações de scraping do site original (proteção contra crawlers), as informações foram adaptadas baseando-se nos nomes das acomodações fornecidos e características típicas de cada tipo de hospedagem.

As URLs das imagens são placeholders e podem ser substituídas por imagens reais quando disponíveis.
