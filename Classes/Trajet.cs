using System;
using System.Collections.Generic;
using GestionFlotte.Classes;
using System.Threading.Tasks;

namespace GestionFlotte.Classes
{
    public class Trajet
    {
        public string Depart { get; set; }
        public string Arrivee { get; set; }
        public double Distance { get; set; }
        public TimeSpan DureeEstimee { get; set; }
        public Vehicule VehiculeAssigne { get; set; }
        public Chauffeur ChauffeurAssigne { get; set; }
        public bool EnCours { get; private set; }  // Propriété pour suivre l'état du trajet

        public Trajet(string depart, string arrivee, double distance, TimeSpan dureeEstimee, Vehicule vehicule, Chauffeur chauffeur)
        {
            Depart = depart;
            Arrivee = arrivee;
            Distance = distance;
            DureeEstimee = dureeEstimee;
            VehiculeAssigne = vehicule;
            ChauffeurAssigne = chauffeur;
            EnCours = false;  // Initialisation à false

            // Vérifier si le chauffeur est autorisé à conduire le véhicule
            if (!chauffeur.VehiculesAutorises.Contains(vehicule))
            {
                throw new PermisInvalideException($"{chauffeur.Nom} n'est pas autorisé à conduire ce type de véhicule.");
            }

            // Vérifier si le véhicule est disponible (par exemple, pas en maintenance)
            if (vehicule.EstEnMaintenance)
            {
                throw new VehiculeIndisponibleException($"Le véhicule {vehicule.Immatriculation} est indisponible (en maintenance).");
            }

            // Marquer le véhicule comme étant en cours d'utilisation
            vehicule.EstEnMaintenance = true;
        }

        // Méthode pour afficher les détails du trajet
        public void AfficherDetails()
        {
            Console.WriteLine($"Trajet : {Depart} -> {Arrivee}, Distance : {Distance} km, Durée estimée : {DureeEstimee}");
            Console.WriteLine($"Véhicule : {VehiculeAssigne.Marque} {VehiculeAssigne.Modele}, Immatriculation : {VehiculeAssigne.Immatriculation}");
            Console.WriteLine($"Chauffeur : {ChauffeurAssigne.Nom}, Permis : {ChauffeurAssigne.Permis}");
        }

        // Méthode asynchrone pour simuler le trajet en temps réel
        public async Task SimulerTrajetAsync()
        {
            EnCours = true;  // Marquer le trajet comme en cours
            Console.WriteLine($"Le trajet de {Depart} à {Arrivee} commence maintenant avec le véhicule {VehiculeAssigne.Marque} {VehiculeAssigne.Modele}.");

            // Simulation du départ du trajet
            await Task.Delay(1000);  // Simule 1 seconde de délai
            Console.WriteLine($"Le trajet de {Depart} à {Arrivee} est en cours...");

            // Simulation de la progression du trajet
            await Task.Delay(2000);  // Simule 2 secondes de délai pour la progression
            Console.WriteLine($"Le trajet de {Depart} à {Arrivee} est presque terminé...");

            // Simulation de l'arrivée
            await Task.Delay(1000);  // Simule 1 seconde de délai pour l'arrivée
            Console.WriteLine($"Le trajet de {Depart} à {Arrivee} est terminé. Le chauffeur {ChauffeurAssigne.Nom} est arrivé à destination.");

            // Mettre à jour l'état du trajet et la disponibilité du véhicule
            EnCours = false;  // Marquer le trajet comme terminé
            VehiculeAssigne.EstEnMaintenance = false;  // Marquer le véhicule comme disponible
        }
    }
}
