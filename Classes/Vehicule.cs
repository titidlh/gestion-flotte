using System;
using System.Collections.Generic;
using GestionFlotte.Classes;

namespace GestionFlotte.Classes
{
    public abstract class Vehicule : IMaintenable
    {
        public event Action<Vehicule> MaintenanceDue;

        public string Immatriculation { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Type { get; set; }
        public int Kilometrage { get; set; }
        public double ConsommationParKm { get; set; }  // En litres par kilomètre
        public bool EstEnMaintenance { get; set; }  // Propriété indiquant si le véhicule est en maintenance

        // Propriété pour l'historique des trajets effectués par le véhicule
        public List<Trajet> HistoriqueTrajets { get; set; }

        public Vehicule(string immatriculation, string marque, string modele, string type, int kilometrage, double consommationParKm)
        {
            Immatriculation = immatriculation ?? throw new ArgumentNullException(nameof(immatriculation));
            Marque = marque ?? throw new ArgumentNullException(nameof(marque));
            Modele = modele ?? throw new ArgumentNullException(nameof(modele));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Kilometrage = kilometrage;
            ConsommationParKm = consommationParKm;
            EstEnMaintenance = false;  // Par défaut, le véhicule n'est pas en maintenance
            HistoriqueTrajets = new List<Trajet>();  // Initialisation de la liste des trajets
        }

        public abstract void AfficherDetails();

        // Vérifie si la maintenance est due
        public bool EstMaintenanceDue()
        {
            return Kilometrage >= 100000;  // Exemple de seuil de maintenance
        }

        // Planifie la maintenance si nécessaire
        public void PlanifierMaintenance()
        {
            if (EstMaintenanceDue())
            {
                MaintenanceDue?.Invoke(this);
                Console.WriteLine($"Maintenance nécessaire pour {Marque} {Modele} - {Immatriculation}");
                EstEnMaintenance = true;
            }
        }

        // Terminer la maintenance du véhicule
        public void TerminerMaintenance()
        {
            EstEnMaintenance = false;
            Console.WriteLine($"Maintenance terminée pour {Marque} {Modele} - {Immatriculation}");
        }

        // Ajoute un trajet à l'historique et met à jour le kilométrage
        public void AjouterTrajet(Trajet trajet)
        {
            Kilometrage += (int)trajet.Distance;
            HistoriqueTrajets.Add(trajet);  // Ajout du trajet à l'historique
            Console.WriteLine($"Trajet ajouté. Nouveau kilométrage : {Kilometrage} km");
        }
    }
}
