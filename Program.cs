using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFlotte.Classes;
using GestionFlotte.Database;
using GestionFlotte.Utilitaires;

class Program
{
    static Database db = new Database("flotte.db");
    static List<Vehicule> flotte = new List<Vehicule>();
    static List<Chauffeur> chauffeurs = new List<Chauffeur>();

    static async Task Main(string[] args)
    {
        db.CreerTables();

        flotte = db.RecupererVehicules();
        chauffeurs = db.RecupererChauffeurs();

        if (flotte.Count == 0)
        {
            AjouterVehicule(new Voiture("AB-123-CD", "Peugeot", "208", 50000, 0.06));
            AjouterVehicule(new Camion("CD-789-FG", "Mercedes", "Actros", 120000, 0.12));
            AjouterVehicule(new Moto("EF-456-HI", "Yamaha", "MT-07", 90000, 0.05));
        }

        if (chauffeurs.Count == 0)
        {
            AjouterChauffeur(new Chauffeur("Jean Dupont", "B", 5));
            AjouterChauffeur(new Chauffeur("Marie Martin", "C", 10));
        }

        chauffeurs[0].AjouterVehiculeAutorise(flotte[0]);
        chauffeurs[1].AjouterVehiculeAutorise(flotte[1]);

        // Vérifier si l'argument --test est fourni
    if (args.Length > 0 && args[0] == "--test")
    {
        // Exécuter le test automatique de l'option 11
        Console.WriteLine("Mode test activé : exécution automatique de l'option 11.");
        await PlanifierTrajet();
        Console.WriteLine("Test terminé.");
    }
    else
    {
        await AfficherMenuPrincipal();
    }

        await AfficherMenuPrincipal();
        db.FermerConnexion();
    }

    static void AjouterVehicule(Vehicule vehicule)
    {
        flotte.Add(vehicule);
        db.AjouterVehicule(vehicule.Immatriculation, vehicule.Marque, vehicule.Modele, vehicule.Type, vehicule.Kilometrage, vehicule.ConsommationParKm);
        Console.WriteLine($"Véhicule {vehicule.Marque} {vehicule.Modele} ajouté à la flotte.");
    }

    static void ModifierVehicule(string immatriculation, int nouveauKilometrage)
    {
        var vehicule = flotte.Find(v => v.Immatriculation == immatriculation);
        if (vehicule != null)
        {
            vehicule.Kilometrage = nouveauKilometrage;
            db.ModifierVehicule(immatriculation, nouveauKilometrage);
            Console.WriteLine($"Le kilométrage du véhicule {vehicule.Immatriculation} a été mis à jour à {nouveauKilometrage} km.");
        }
        else
        {
            Console.WriteLine("Véhicule introuvable.");
        }
    }

    static void SupprimerVehicule(string immatriculation)
    {
        var vehicule = flotte.Find(v => v.Immatriculation == immatriculation);
        if (vehicule != null)
        {
            flotte.Remove(vehicule);
            db.SupprimerVehicule(immatriculation);
            Console.WriteLine($"Véhicule {immatriculation} supprimé.");
        }
        else
        {
            Console.WriteLine("Véhicule introuvable.");
        }
    }

    static void AjouterChauffeur(Chauffeur chauffeur)
    {
        chauffeurs.Add(chauffeur);
        db.AjouterChauffeur(chauffeur.Nom, chauffeur.Permis, chauffeur.Anciennete);
        Console.WriteLine($"Chauffeur {chauffeur.Nom} ajouté.");
    }

    static void ModifierChauffeur(string nom, int nouvelleAnciennete)
    {
        var chauffeur = chauffeurs.Find(c => c.Nom == nom);
        if (chauffeur != null)
        {
            chauffeur.Anciennete = nouvelleAnciennete;
            db.ModifierChauffeur(nom, nouvelleAnciennete);
            Console.WriteLine($"Le chauffeur {nom} a une nouvelle ancienneté de {nouvelleAnciennete} ans.");
        }
        else
        {
            Console.WriteLine("Chauffeur introuvable.");
        }
    }

    static void SupprimerChauffeur(string nom)
    {
        var chauffeur = chauffeurs.Find(c => c.Nom == nom);
        if (chauffeur != null)
        {
            chauffeurs.Remove(chauffeur);
            db.SupprimerChauffeur(nom);
            Console.WriteLine($"Chauffeur {nom} supprimé.");
        }
        else
        {
            Console.WriteLine("Chauffeur introuvable.");
        }
    }

    static void AfficherVehicules()
    {
        Console.WriteLine("\nListe des véhicules disponibles dans la flotte :");
        foreach (var vehicule in flotte)
        {
            Console.WriteLine($"Type: {vehicule.Type}, Marque: {vehicule.Marque}, Modèle: {vehicule.Modele}, Immatriculation: {vehicule.Immatriculation}, Kilométrage: {vehicule.Kilometrage} km");
        }
    }

    static void AfficherChauffeurs()
    {
        Console.WriteLine("\nListe des chauffeurs :");
        foreach (var chauffeur in chauffeurs)
        {
            Console.WriteLine($"Nom: {chauffeur.Nom}, Permis: {chauffeur.Permis}, Ancienneté: {chauffeur.Anciennete} ans");
        }
    }
static async Task CreerTrajetMenu()
{
    Console.WriteLine("\n=== Créer un trajet ===");

    string depart = "";
    string arrivee = "";
    double distance = 0;
    TimeSpan dureeEstimee = TimeSpan.Zero;
    Vehicule vehicule = null;
    Chauffeur chauffeur = null;

    // Étape 1 : Point de départ
    while (true)
    {
        Console.Write("Point de départ : ");
        depart = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(depart))
            break;
        Console.WriteLine("Erreur : Le point de départ ne peut pas être vide. Veuillez réessayer.");
    }

    // Étape 2 : Point d'arrivée
    while (true)
    {
        Console.Write("Point d'arrivée : ");
        arrivee = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(arrivee))
            break;
        Console.WriteLine("Erreur : Le point d'arrivée ne peut pas être vide. Veuillez réessayer.");
    }

    // Étape 3 : Distance
    while (true)
    {
        Console.Write("Distance (en km) : ");
        if (double.TryParse(Console.ReadLine(), out distance) && distance > 0)
            break;
        Console.WriteLine("Erreur : Veuillez entrer une distance valide (nombre positif). Veuillez réessayer.");
    }

    // Étape 4 : Durée estimée
    while (true)
    {
        Console.Write("Durée estimée (hh:mm:ss) : ");
        if (TimeSpan.TryParse(Console.ReadLine(), out dureeEstimee))
            break;
        Console.WriteLine("Erreur : Veuillez entrer une durée valide au format hh:mm:ss. Veuillez réessayer.");
    }

    // Étape 5 : Sélection du véhicule
    while (true)
    {
        Console.WriteLine("\nListe des véhicules disponibles :");
        AfficherVehicules();
        Console.Write("Immatriculation du véhicule choisi : ");
        string immatriculation = Console.ReadLine();
        vehicule = flotte.Find(v => v.Immatriculation == immatriculation);
        if (vehicule != null)
            break;
        Console.WriteLine("Erreur : Véhicule introuvable. Veuillez entrer une immatriculation valide.");
    }

    // Étape 6 : Sélection du chauffeur
    while (true)
    {
        Console.WriteLine("\nListe des chauffeurs disponibles :");
        AfficherChauffeurs();
        Console.Write("Nom du chauffeur choisi : ");
        string nomChauffeur = Console.ReadLine();
        chauffeur = chauffeurs.Find(c => c.Nom == nomChauffeur);
        if (chauffeur != null)
        {
            // Vérifier si le chauffeur est autorisé à conduire ce type de véhicule
            if (chauffeur.VehiculesAutorises.Contains(vehicule))
                break;
            else
                Console.WriteLine($"Erreur : Le chauffeur {chauffeur.Nom} n'est pas autorisé à conduire le véhicule {vehicule.Immatriculation}. Veuillez sélectionner un autre chauffeur.");
        }
        else
        {
            Console.WriteLine("Erreur : Chauffeur introuvable. Veuillez entrer un nom de chauffeur valide.");
        }
    }

    // Étape 7 : Création et enregistrement du trajet
    try
    {
        Trajet trajet = new Trajet(depart, arrivee, distance, dureeEstimee, vehicule, chauffeur);
        chauffeur.AjouterTrajet(trajet);
        vehicule.AjouterTrajet(trajet);

        // Ajouter le trajet à la base de données
        db.AjouterTrajet(depart, arrivee, distance, dureeEstimee.ToString(), vehicule.Immatriculation, chauffeur.Nom);
        Console.WriteLine("Trajet créé avec succès.");

        // Simuler le trajet de manière asynchrone
        await trajet.SimulerTrajetAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la création du trajet : {ex.Message}");
    }
}


    static void AfficherStatistiques()
    {
        Statistiques.AfficherKilometrageTotal(flotte);
        Statistiques.AfficherConsommationTotale(flotte);
        Statistiques.AfficherChauffeursLesPlusActifs(chauffeurs);
    }

    static async Task AfficherMenuPrincipal()
    {
        int choix = 0;
        do
        {
            Console.WriteLine("\n=== Menu Principal ===");
            Console.WriteLine("1. Ajouter un véhicule");
            Console.WriteLine("2. Modifier un véhicule");
            Console.WriteLine("3. Supprimer un véhicule");
            Console.WriteLine("4. Afficher les véhicules");
            Console.WriteLine("5. Ajouter un chauffeur");
            Console.WriteLine("6. Modifier un chauffeur");
            Console.WriteLine("7. Supprimer un chauffeur");
            Console.WriteLine("8. Afficher les chauffeurs");
            Console.WriteLine("9. Créer un trajet");
            Console.WriteLine("10. Afficher les statistiques");
            Console.WriteLine("11. Planifier automatiquement un trajet");
            Console.WriteLine("12. Quitter");
            Console.Write("Choisissez une option : ");

            if (int.TryParse(Console.ReadLine(), out choix))
            {
                switch (choix)
                {
                    case 1: AjouterVehiculeMenu(); break;
                    case 2: ModifierVehiculeMenu(); break;
                    case 3: SupprimerVehiculeMenu(); break;
                    case 4: AfficherVehicules(); break;
                    case 5: AjouterChauffeurMenu(); break;
                    case 6: ModifierChauffeurMenu(); break;
                    case 7: SupprimerChauffeurMenu(); break;
                    case 8: AfficherChauffeurs(); break;
                    case 9: await CreerTrajetMenu(); break;
                    case 10: AfficherStatistiques(); break;
                    case 11: await PlanifierTrajet(); break;
                    case 12: Console.WriteLine("Merci d'avoir utilisé le programme. À bientôt !"); break;
                    default: Console.WriteLine("Option invalide. Veuillez choisir à nouveau."); break;
                }
            }
            else
            {
                Console.WriteLine("Entrée invalide. Veuillez entrer un nombre.");
            }
        } while (choix != 12);
    }

    // Méthode pour ajouter un véhicule
static void AjouterVehiculeMenu()
{
    Console.WriteLine("\n=== Ajouter un véhicule ===");
    Console.Write("Immatriculation : ");
    string immatriculation = Console.ReadLine();
    Console.Write("Marque : ");
    string marque = Console.ReadLine();
    Console.Write("Modèle : ");
    string modele = Console.ReadLine();
    Console.Write("Type (Voiture, Camion, Moto) : ");
    string type = Console.ReadLine();

    Console.Write("Kilométrage : ");
    if (!int.TryParse(Console.ReadLine(), out int kilometrage))
    {
        Console.WriteLine("Erreur : Veuillez entrer un nombre entier valide pour le kilométrage.");
        return;
    }

    Console.Write("Consommation par km : ");
    if (!double.TryParse(Console.ReadLine(), out double consommation))
    {
        Console.WriteLine("Erreur : Veuillez entrer un nombre décimal valide pour la consommation.");
        return;
    }

    Vehicule vehicule;
    switch (type.ToLower())
    {
        case "voiture":
            vehicule = new Voiture(immatriculation, marque, modele, kilometrage, consommation);
            break;
        case "camion":
            vehicule = new Camion(immatriculation, marque, modele, kilometrage, consommation);
            break;
        case "moto":
            vehicule = new Moto(immatriculation, marque, modele, kilometrage, consommation);
            break;
        default:
            Console.WriteLine("Type de véhicule invalide.");
            return;
    }

    AjouterVehicule(vehicule);
}

// Méthode pour modifier un véhicule
static void ModifierVehiculeMenu()
{
    Console.WriteLine("\n=== Modifier un véhicule ===");
    Console.Write("Entrez l'immatriculation du véhicule à modifier : ");
    string immatriculation = Console.ReadLine();
    Console.Write("Nouveau kilométrage : ");
    if (int.TryParse(Console.ReadLine(), out int nouveauKilometrage))
    {
        ModifierVehicule(immatriculation, nouveauKilometrage);
    }
    else
    {
        Console.WriteLine("Erreur : Veuillez entrer un nombre entier valide pour le kilométrage.");
    }
}

// Méthode pour supprimer un véhicule
static void SupprimerVehiculeMenu()
{
    Console.WriteLine("\n=== Supprimer un véhicule ===");
    Console.Write("Entrez l'immatriculation du véhicule à supprimer : ");
    string immatriculation = Console.ReadLine();
    SupprimerVehicule(immatriculation);
}

// Méthode pour ajouter un chauffeur
static void AjouterChauffeurMenu()
{
    Console.WriteLine("\n=== Ajouter un chauffeur ===");
    Console.Write("Nom : ");
    string nom = Console.ReadLine();
    Console.Write("Permis (B, C, A) : ");
    string permis = Console.ReadLine();
    Console.Write("Ancienneté (en années) : ");
    if (int.TryParse(Console.ReadLine(), out int anciennete))
    {
        AjouterChauffeur(new Chauffeur(nom, permis, anciennete));
    }
    else
    {
        Console.WriteLine("Erreur : Veuillez entrer un nombre entier valide pour l'ancienneté.");
    }
}

// Méthode pour modifier un chauffeur
static void ModifierChauffeurMenu()
{
    Console.WriteLine("\n=== Modifier un chauffeur ===");
    Console.Write("Entrez le nom du chauffeur à modifier : ");
    string nom = Console.ReadLine();
    Console.Write("Nouvelle ancienneté : ");
    if (int.TryParse(Console.ReadLine(), out int nouvelleAnciennete))
    {
        ModifierChauffeur(nom, nouvelleAnciennete);
    }
    else
    {
        Console.WriteLine("Erreur : Veuillez entrer un nombre entier valide pour l'ancienneté.");
    }
}

// Méthode pour supprimer un chauffeur
static void SupprimerChauffeurMenu()
{
    Console.WriteLine("\n=== Supprimer un chauffeur ===");
    Console.Write("Entrez le nom du chauffeur à supprimer : ");
    string nom = Console.ReadLine();
    SupprimerChauffeur(nom);
}

static async Task PlanifierTrajet()
{
    Console.WriteLine("\n=== Planification automatique de trajet ===");

    // Saisie du point de départ et d'arrivée
    string depart = "";
    string arrivee = "";
    double distance = 0;
    TimeSpan dureeEstimee = TimeSpan.Zero;

    // Étape 1 : Saisie du point de départ
    while (true)
    {
        Console.Write("Point de départ : ");
        depart = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(depart))
            break;
        Console.WriteLine("Erreur : Le point de départ ne peut pas être vide. Veuillez réessayer.");
    }

    // Étape 2 : Saisie du point d'arrivée
    while (true)
    {
        Console.Write("Point d'arrivée : ");
        arrivee = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(arrivee))
            break;
        Console.WriteLine("Erreur : Le point d'arrivée ne peut pas être vide. Veuillez réessayer.");
    }

    // Étape 3 : Saisie de la distance
    while (true)
    {
        Console.Write("Distance (en km) : ");
        if (double.TryParse(Console.ReadLine(), out distance) && distance > 0)
            break;
        Console.WriteLine("Erreur : Veuillez entrer une distance valide (nombre positif). Veuillez réessayer.");
    }

    // Étape 4 : Saisie de la durée estimée
    while (true)
    {
        Console.Write("Durée estimée (hh:mm:ss) : ");
        if (TimeSpan.TryParse(Console.ReadLine(), out dureeEstimee))
            break;
        Console.WriteLine("Erreur : Veuillez entrer une durée valide au format hh:mm:ss. Veuillez réessayer.");
    }

    // Filtrer les véhicules disponibles
    List<Vehicule> vehiculesDisponibles = flotte.FindAll(v => !v.EstEnMaintenance && !v.HistoriqueTrajets.Exists(t => t.EnCours));
    if (vehiculesDisponibles.Count == 0)
    {
        Console.WriteLine("Aucun véhicule disponible pour planifier le trajet.");
        return;
    }

    // Sélectionner le premier véhicule disponible (vous pouvez utiliser d'autres critères si souhaité)
    Vehicule vehiculeChoisi = vehiculesDisponibles[0];

    // Filtrer les chauffeurs disponibles en fonction du véhicule
    List<Chauffeur> chauffeursDisponibles = chauffeurs.FindAll(c =>
        c.VehiculesAutorises.Contains(vehiculeChoisi) &&
        !c.HistoriqueTrajets.Exists(t => t.EnCours));

    if (chauffeursDisponibles.Count == 0)
    {
        Console.WriteLine($"Aucun chauffeur disponible pour conduire le véhicule {vehiculeChoisi.Immatriculation}.");
        return;
    }

    // Sélectionner le premier chauffeur disponible
    Chauffeur chauffeurChoisi = chauffeursDisponibles[0];

    // Afficher la sélection automatique
    Console.WriteLine($"Planification automatique réussie !");
    Console.WriteLine($"Véhicule assigné : {vehiculeChoisi.Immatriculation} - {vehiculeChoisi.Marque} {vehiculeChoisi.Modele}");
    Console.WriteLine($"Chauffeur assigné : {chauffeurChoisi.Nom} (Permis : {chauffeurChoisi.Permis})");

    // Étape finale : Création et simulation du trajet
    try
    {
        Trajet trajet = new Trajet(depart, arrivee, distance, dureeEstimee, vehiculeChoisi, chauffeurChoisi);
        chauffeurChoisi.AjouterTrajet(trajet);
        vehiculeChoisi.AjouterTrajet(trajet);

// Utilisez `HistoriqueTrajets` pour ajouter le trajet à l'historique du chauffeur
        chauffeurChoisi.HistoriqueTrajets.Add(trajet);  



 // Ajoutez le trajet dans la base de données
        db.AjouterTrajet(depart, arrivee, distance, dureeEstimee.ToString(), vehiculeChoisi.Immatriculation, chauffeurChoisi.Nom);

        Console.WriteLine("Trajet créé avec succès.");
        // Simuler le trajet de manière asynchrone
        await trajet.SimulerTrajetAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la planification du trajet : {ex.Message}");
    }
}


}
