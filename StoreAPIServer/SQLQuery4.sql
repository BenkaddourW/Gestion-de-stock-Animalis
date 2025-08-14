INSERT INTO [dbo].[Utilisateurs] 
    ([Prenom], [Nom], [Email], [Telephone], [MotDePasse], [TypeAnimal], [NomAnimal], [AccepteConditions], [InscritNewsletter])
VALUES
    ('Jean', 'Dupont', 'jean.dupont@email.com', '0612345678', 'motdepasse123', 'Chien', 'Rex', 1, 1),
    ('Marie', 'Martin', 'marie.martin@email.com', '0698765432', 'password456', 'Chat', 'Misty', 1, 0),
    ('Pierre', 'Durand', 'pierre.durand@email.com', '0687654321', 'secure789', 'Poisson', 'Bubble', 1, 1),
    ('Sophie', 'Leroy', 'sophie.leroy@email.com', '0676543210', 'sophie2023', 'Oiseau', 'Tweety', 1, 0);