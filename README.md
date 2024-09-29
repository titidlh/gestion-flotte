# gestion-flotte
projet scolaire c#
=======
# Application de Gestion de Flotte de Véhicules

## Introduction
Cette application permet de gérer une flotte de véhicules et de planifier des trajets pour différents chauffeurs. Elle inclut les fonctionnalités suivantes :
- Gestion des véhicules (ajout, modification, suppression)
- Gestion des chauffeurs (ajout, modification, suppression)
- Création de trajets entre des points de départ et d'arrivée
- Calcul et affichage des statistiques sur la flotte (kilométrage total, consommation de carburant, chauffeurs les plus actifs)

## Technologies utilisées
- **Langage** : C#
- **Framework** : .NET 6+
- **Base de données** : SQLite (via Entity Framework Core)
- **Architecture** : Console Application

## Prérequis
- .NET Core 6.0 ou supérieur
- SQLite

## Instructions d'installation
1. Clonez le projet dans un répertoire local :
   ```bash
   git clone https://github.com/titidlh/gestion-flotte.git


#Accédez au répertoire du projet :

cd gestion-flotte

# Restaurez les dépendances du projet :

dotnet restore

#Construisez le projet :

dotnet build

# Exécutez le projet :

    dotnet run

# Utilisation

# L'application propose un menu interactif dans le terminal pour naviguer entre les différentes fonctionnalités. Voici un aperçu des options disponibles :

    1. Ajouter un véhicule : Ajouter un nouveau véhicule à la flotte.
    2. Modifier un véhicule : Modifier les informations d'un véhicule existant.
    3. Supprimer un véhicule : Retirer un véhicule de la flotte.
    4. Afficher les véhicules : Afficher tous les véhicules présents dans la flotte.
    5. Ajouter un chauffeur : Ajouter un chauffeur avec son permis et son ancienneté.
    6. Modifier un chauffeur : Modifier les informations d'un chauffeur existant.
    7. Supprimer un chauffeur : Retirer un chauffeur de la liste.
    8. Afficher les chauffeurs : Afficher tous les chauffeurs enregistrés.
    9. Créer un trajet : Planifier un nouveau trajet pour un chauffeur et un véhicule.
    10. Afficher les statistiques : Afficher les statistiques sur les trajets, la consommation, etc.
    11. Planifier automatiquement un trajet : Sélectionner automatiquement un véhicule et un chauffeur disponibles pour un trajet.

# Tests

Le projet inclut un ensemble de tests unitaires pour valider les fonctionnalités principales. Pour exécuter les tests, utilisez les commandes suivantes :

#    Ajoutez un projet de tests si nécessaire :

dotnet new xunit -n GestionFlotte.Tests

# Ajoutez une référence au projet principal :

dotnet add GestionFlotte.Tests/GestionFlotte.Tests.csproj reference GestionFlotte/GestionFlotte.csproj

# Exécutez les tests :

    dotnet test

# Structure du projet

    Classes/ : Contient les classes principales (Vehicule, Chauffeur, Trajet, etc.)
    Database/ : Gestion de la base de données et des opérations CRUD.
    Utilitaires/ : Classes utilitaires pour les statistiques.
    Program.cs : Point d'entrée de l'application.
    GestionFlotte.csproj : Fichier de projet C#.

# Bugs connus et améliorations

    Le projet nécessite une validation d'entrée utilisateur plus robuste.
    Ajout d'une interface graphique pour améliorer l'expérience utilisateur (en cours).


# Structure du Projet

gestion-flotte/
├── bin/                           # Répertoire de build généré automatiquement
├── Classes/                       # Contient les classes principales
│   ├── Camion.cs                  # Classe pour les camions
│   ├── Chauffeur.cs               # Classe pour les chauffeurs
│   ├── Exceptions.cs              # Gestion des exceptions spécifiques
│   ├── IMaintenable.cs            # Interface pour la maintenance des véhicules
│   ├── Moto.cs                    # Classe pour les motos
│   ├── Trajet.cs                  # Classe pour les trajets
│   ├── Vehicule.cs                # Classe de base pour les véhicules
│   └── Voiture.cs                 # Classe pour les voitures
├── Database/                      # Gestion de la base de données et des opérations CRUD
│   └── Database.cs
├── Utilitaires/                   # Classes utilitaires pour les statistiques
│   └── Statistiques.cs
├── Program.cs                     # Point d'entrée de l'application
├── GestionFlotte.csproj           # Fichier de projet C#
├── README.md                      # Documentation de l'application
└── flotte.db                      # Base de données SQLite




# Auteur

    Nom : titidlh
    
