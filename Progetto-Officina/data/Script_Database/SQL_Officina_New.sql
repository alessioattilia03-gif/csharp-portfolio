-- CREATING THE ADMIN TABLE WITH ACCESS RIGHTS
CREATE TABLE Utente (
    UtenteID INT PRIMARY KEY IDENTITY(1,1), -- primary key
    CodicePub CHAR(36) NOT NULL DEFAULT NEWID() UNIQUE, -- a unique global identifier is created. useful for external references without exposure
    Username VARCHAR(100) NOT NULL UNIQUE, 
    PasswordHash VARCHAR(MAX) NOT NULL, -- login id
    Ruolo VARCHAR(50) NOT NULL
);

CREATE TABLE Cliente (
    ClienteID INT PRIMARY KEY IDENTITY(1,1), -- primary key
    CodicePub CHAR(36) NOT NULL DEFAULT NEWID() UNIQUE, -- a unique global identifier is created. useful for external references without exposure
    Nome VARCHAR(100) NOT NULL,
    Cognome VARCHAR(100) NOT NULL,
    Telefono VARCHAR(30),
    Email VARCHAR(250) UNIQUE, -- unique email addresses to avoid duplicates
    Indirizzo VARCHAR(500)
);

CREATE TABLE Veicolo (
    VeicoloID INT PRIMARY KEY IDENTITY(1,1), -- primary key
    CodicePub CHAR(36) NOT NULL DEFAULT NEWID() UNIQUE, -- a unique global identifier is created. useful for external references without exposure
    Targa VARCHAR(20) NOT NULL UNIQUE, 
    Marca VARCHAR(100) NOT NULL,
    Modello VARCHAR(100) NOT NULL,
    Anno INT,
    ClienteID INT NOT NULL, -- foreign key to cliente
    CONSTRAINT FK_Veicolo_Cliente FOREIGN KEY (ClienteID) 
        REFERENCES Cliente(ClienteID) ON DELETE CASCADE -- if i delete a customer, the db deletes all vehicles associated with the customer
);

CREATE TABLE Intervento (
    InterventoID INT PRIMARY KEY IDENTITY(1,1), -- primary key
    CodicePub CHAR(36) NOT NULL DEFAULT NEWID() UNIQUE, -- a unique global identifier is created. useful for external references without exposure
    Descrizione NVARCHAR(MAX) NOT NULL, 
    DataIngresso DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, -- current date and time at the moment of record creation
    DataFine DATETIME, 
    Prezzo DECIMAL(10,2) CHECK (Prezzo >= 0), 
    Stato VARCHAR(50) CHECK (Stato IN ('in corso', 'completato', 'da fare')), 
    VeicoloID INT NOT NULL, -- foreign key to veicolo
    CONSTRAINT FK_Intervento_Veicolo FOREIGN KEY (VeicoloID) 
        REFERENCES Veicolo(VeicoloID) ON DELETE CASCADE -- if i delete a vehicle, the db deletes all interventions associated with the vehicle
);

INSERT INTO Cliente (Nome, Cognome, Telefono, Email) VALUES
('Luca', 'Bianchi', '3331111111', 'luca.bianchi@email.com'),
('Giulia', 'Verdi', '3332222222', 'giulia.verdi@email.com'),
('Marco', 'Neri', '3333333333', 'marco.neri@email.com'),
('Anna', 'Gialli', '3334444444', 'anna.gialli@email.com'),
('Paolo', 'Blu', '3335555555', 'paolo.blu@email.com'),
('Sara', 'Rosa', '3336666666', 'sara.rosa@email.com'),
('Davide', 'Ferrari', '3337777777', 'davide.ferrari@email.com'),
('Elisa', 'Romano', '3338888888', 'elisa.romano@email.com'),
('Francesco', 'Greco', '3339999999', 'francesco.greco@email.com'),
('Chiara', 'Conti', '3330000000', 'chiara.conti@email.com');

INSERT INTO Utente (Username, PasswordHash, Ruolo) VALUES
('admin', 'hash_admin_123', 'admin'),
('meccanico1', 'hash_mec1_123', 'meccanico'),
('meccanico2', 'hash_mec2_123', 'meccanico'),
('meccanico3', 'hash_mec3_123', 'meccanico'),
('meccanico4', 'hash_mec4_123', 'meccanico');

INSERT INTO Veicolo (Targa, Marca, Modello, Anno, ClienteID) VALUES
('BB123CC', 'Volkswagen', 'Golf', 2021, 2),
('CC234DD', 'Ford', 'Focus', 2020, 3),
('DD345EE', 'BMW', 'Serie 1', 2023, 4),
('EE456FF', 'Audi', 'A3', 2022, 5),
('FF567GG', 'Mercedes', 'Classe A', 2021, 6),
('GG678HH', 'Toyota', 'Yaris', 2019, 7),
('HH789II', 'Hyundai', 'i20', 2020, 8),
('II890JJ', 'Kia', 'Rio', 2021, 9),
('JJ901KK', 'Peugeot', '208', 2022, 10),
('KK012LL', 'Renault', 'Clio', 2023, 1);

INSERT INTO Intervento (Descrizione, Prezzo, Stato, VeicoloID) VALUES
('cambio olio e filtri', 120.00, 'completato', 1),
('sostituzione pastiglie freni', 180.00, 'completato', 2),
('revisione completa', 250.00, 'in corso', 3),
('cambio gomme (4)', 450.00, 'da fare', 4),
('sostituzione batteria', 130.00, 'completato', 5),
('tagliando completo', 220.00, 'in corso', 6),
('riparazione sospensioni', 400.00, 'da fare', 7),
('diagnosi elettronica', 70.00, 'completato', 8),
('sostituzione frizione', 650.00, 'in corso', 9),
('riparazione impianto frenante', 300.00, 'da fare', 10);


	select *from Cliente
    select *from Utente
    select *from Veicolo
    select *from Intervento

	--drop table Cliente
	--drop table Utente

	--select * from Utente
	--EXEC sp_rename 'Utente.codice', 'CodicePub', 'COLUMN';

 --   ALTER TABLE Utente
 --   ADD CONSTRAINT CK_Utente_Ruolo
 --   CHECK (Ruolo IN ('admin', 'meccanico'));

-- ALTER TABLE Utente 
-- ADD Email NVARCHAR(255) NULL,
--    Telefono NVARCHAR(20) NULL;


--SELECT name, is_identity 
--FROM sys.columns 
--WHERE object_id = OBJECT_ID('Utente') AND name = 'UtenteId';

--SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
--FROM INFORMATION_SCHEMA.COLUMNS
--WHERE TABLE_NAME = 'Utente'; 