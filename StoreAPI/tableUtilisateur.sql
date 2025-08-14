CREATE TABLE Utilisateur (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Prenom NVARCHAR(50) NOT NULL,
    Nom NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telephone NVARCHAR(20),
    MotDePasse NVARCHAR(255) NOT NULL,
    TypeAnimal NVARCHAR(30),
    NomAnimal NVARCHAR(50),
    AccepteConditions BIT NOT NULL,
    InscriptionNewsletter BIT NOT NULL,
    DateInscription DATETIME DEFAULT GETDATE()
);
