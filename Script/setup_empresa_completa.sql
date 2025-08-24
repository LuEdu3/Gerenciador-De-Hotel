-- Script para atualizar a estrutura da tabela Empresas
-- Execute este script no MySQL Workbench ou linha de comando

USE gerenciador_hotel;

-- Adicionar novos campos à tabela Empresas
ALTER TABLE Empresas 
ADD COLUMN NomeResumido VARCHAR(50) NULL AFTER Nome,
ADD COLUMN Slogan VARCHAR(500) NULL AFTER LogoUrl,
ADD COLUMN DescricaoSobre TEXT NULL AFTER Slogan,
ADD COLUMN DescricaoBreve VARCHAR(1000) NULL AFTER DescricaoSobre,
ADD COLUMN AnoFundacao INT NULL AFTER DescricaoBreve,
ADD COLUMN Telefone VARCHAR(100) NULL AFTER AnoFundacao,
ADD COLUMN WhatsApp VARCHAR(100) NULL AFTER Telefone,
ADD COLUMN Endereco VARCHAR(200) NULL AFTER Email,
ADD COLUMN Cidade VARCHAR(100) NULL AFTER Endereco,
ADD COLUMN Estado VARCHAR(50) NULL AFTER Cidade,
ADD COLUMN CEP VARCHAR(20) NULL AFTER Estado,
ADD COLUMN Pais VARCHAR(100) NULL AFTER CEP,
ADD COLUMN Website VARCHAR(100) NULL AFTER Pais,
ADD COLUMN Facebook VARCHAR(100) NULL AFTER Website,
ADD COLUMN Instagram VARCHAR(100) NULL AFTER Facebook,
ADD COLUMN Twitter VARCHAR(100) NULL AFTER Instagram,
ADD COLUMN LinkedIn VARCHAR(100) NULL AFTER Twitter,
ADD COLUMN HorarioCheckin VARCHAR(50) NULL AFTER LinkedIn,
ADD COLUMN HorarioCheckout VARCHAR(50) NULL AFTER HorarioCheckin,
ADD COLUMN Ativo BOOLEAN NOT NULL DEFAULT TRUE AFTER HorarioCheckout,
ADD COLUMN DataAtualizacao DATETIME(6) NULL AFTER DataCriacao;

-- Atualizar tabela EmpresaFotos
ALTER TABLE EmpresaFotos 
ADD COLUMN AltText VARCHAR(100) NULL AFTER Descricao,
ADD COLUMN Ordem INT NOT NULL DEFAULT 0 AFTER AltText,
ADD COLUMN Tipo INT NOT NULL DEFAULT 4 AFTER Ordem,
ADD COLUMN Ativo BOOLEAN NOT NULL DEFAULT TRUE AFTER Tipo,
ADD COLUMN DataCriacao DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) AFTER Ativo;

-- Criar tabela EmpresaServicos
CREATE TABLE EmpresaServicos (
    Id INT NOT NULL AUTO_INCREMENT,
    EmpresaId INT NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(500) NULL,
    Icone VARCHAR(50) NULL,
    Ordem INT NOT NULL DEFAULT 0,
    Ativo BOOLEAN NOT NULL DEFAULT TRUE,
    DataCriacao DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (Id),
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id) ON DELETE CASCADE,
    INDEX IX_EmpresaServicos_EmpresaId (EmpresaId)
);

-- Criar tabela EmpresaPremios
CREATE TABLE EmpresaPremios (
    Id INT NOT NULL AUTO_INCREMENT,
    EmpresaId INT NOT NULL,
    Titulo VARCHAR(200) NOT NULL,
    Descricao VARCHAR(500) NULL,
    Ano INT NULL,
    Instituicao VARCHAR(100) NULL,
    Icone VARCHAR(50) NULL,
    Ordem INT NOT NULL DEFAULT 0,
    Ativo BOOLEAN NOT NULL DEFAULT TRUE,
    DataCriacao DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (Id),
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id) ON DELETE CASCADE,
    INDEX IX_EmpresaPremios_EmpresaId (EmpresaId)
);

-- Inserir dados padrão para o Sereno Hotel
INSERT INTO Empresas (
    Nome, 
    NomeResumido, 
    Slogan, 
    DescricaoSobre, 
    DescricaoBreve, 
    AnoFundacao, 
    Telefone, 
    WhatsApp, 
    Email, 
    Endereco, 
    Cidade, 
    Estado, 
    CEP, 
    Pais, 
    Website, 
    Facebook, 
    Instagram, 
    HorarioCheckin, 
    HorarioCheckout, 
    Ativo, 
    DataCriacao
) VALUES (
    'Sereno Hotel',
    'Sereno',
    'Onde o luxo encontra a tranquilidade',
    'Fundado em 2005, o Sereno Hotel nasceu da paixão por oferecer experiências memoráveis de hospedagem. Localizado em uma área privilegiada, nosso hotel oferece não apenas acomodações de alto padrão, mas também um ambiente que convida ao relaxamento e reconexão.',
    'O Sereno Hotel oferece uma experiência única de hospedagem, combinando luxo contemporâneo com um serviço personalizado e atenção aos detalhes.',
    2005,
    '+55 (11) 3456-7890',
    '+55 (11) 99876-5432',
    'contato@serenohotel.com.br',
    'Rua das Palmeiras, 123',
    'São Paulo',
    'SP',
    '01234-567',
    'Brasil',
    'https://www.serenohotel.com.br',
    'https://facebook.com/serenohotel',
    'https://instagram.com/serenohotel',
    '14:00',
    '12:00',
    TRUE,
    NOW()
);

-- Inserir serviços padrão
SET @empresa_id = LAST_INSERT_ID();

INSERT INTO EmpresaServicos (EmpresaId, Nome, Descricao, Icone, Ordem, Ativo, DataCriacao) VALUES
(@empresa_id, 'Excelência em Serviço', 'Premiado por 5 anos consecutivos como melhor hotel de luxo da região.', 'fas fa-medal', 1, TRUE, NOW()),
(@empresa_id, 'Gastronomia Refinada', 'Restaurante com chef premiado e menu degustação exclusivo.', 'fas fa-utensils', 2, TRUE, NOW()),
(@empresa_id, 'Localização Privilegiada', 'No coração da cidade, próximo aos principais pontos turísticos.', 'fas fa-map-marker-alt', 3, TRUE, NOW()),
(@empresa_id, 'Suporte 24h', 'Concierge e room service disponíveis 24 horas por dia.', 'fas fa-clock', 4, TRUE, NOW());

-- Inserir prêmios padrão
INSERT INTO EmpresaPremios (EmpresaId, Titulo, Descricao, Ano, Instituicao, Icone, Ordem, Ativo, DataCriacao) VALUES
(@empresa_id, 'Melhor Hotel de Luxo', 'Premiado como melhor hotel de luxo da região', 2023, 'Associação Brasileira de Hotelaria', 'fas fa-trophy', 1, TRUE, NOW()),
(@empresa_id, 'Excelência em Atendimento', 'Reconhecimento pela qualidade no atendimento ao cliente', 2022, 'Instituto Nacional de Turismo', 'fas fa-star', 2, TRUE, NOW()),
(@empresa_id, 'Sustentabilidade Ambiental', 'Certificação em práticas sustentáveis', 2023, 'Green Hotels Brasil', 'fas fa-leaf', 3, TRUE, NOW());

-- Verificar se tudo foi inserido corretamente
SELECT 'Empresa criada com sucesso!' as Status;
SELECT * FROM Empresas WHERE Nome = 'Sereno Hotel';
SELECT COUNT(*) as 'Serviços Inseridos' FROM EmpresaServicos WHERE EmpresaId = @empresa_id;
SELECT COUNT(*) as 'Prêmios Inseridos' FROM EmpresaPremios WHERE EmpresaId = @empresa_id;
