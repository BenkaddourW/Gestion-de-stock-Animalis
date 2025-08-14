-- Insert 10 sample products with different animal types and categories
INSERT INTO [dbo].[Produits] 
    ([Id], [IdProduit], [Nom], [Description], [AnimalType], [Categorie], [Prix], [Quantite])
VALUES
    (NEWID(), 1001, 'Croquettes Premium Chien', 'Croquettes de haute qualité pour chiens adultes', 'Chien', 'Nourriture', 45.99, 50),
    (NEWID(), 1002, 'Litière Agglomérante Chat', 'Litière ultra absorbante pour chats', 'Chat', 'Hygiène', 22.50, 30),
    (NEWID(), 1003, 'Jouet à mâcher pour Chiot', 'Jouet résistant pour les dents de chiots', 'Chien', 'Jouet', 12.99, 75),
    (NEWID(), 1004, 'Aquarium 100L avec Filtre', 'Aquarium complet pour poissons tropicaux', 'Poisson', 'Habitat', 149.99, 10),
    (NEWID(), 1005, 'Foin de Prairie pour Lapin', 'Foin frais pour rongeurs et lapins', 'Lapin', 'Nourriture', 8.75, 40),
    (NEWID(), 1006, 'Collier Anti-Puces Chien', 'Protection contre les puces pendant 8 semaines', 'Chien', 'Santé', 29.95, 25),
    (NEWID(), 1007, 'Cage pour Oiseaux Medium', 'Cage spacieuse pour perruches et petits oiseaux', 'Oiseau', 'Habitat', 65.00, 15),
    (NEWID(), 1008, 'Nourriture pour Tortue', 'Aliment complet pour tortues terrestres', 'Reptile', 'Nourriture', 15.25, 20),
    (NEWID(), 1009, 'Hamac pour Chinchilla', 'Hamac doux pour petit rongeur', 'Rongeur', 'Accessoire', 18.50, 12),
    (NEWID(), 1010, 'Shampooing pour Chien', 'Shampooing doux au pH adapté pour chiens', 'Chien', 'Hygiène', 14.99, 35);

-- Verify the inserted data
SELECT * FROM [dbo].[Produits];