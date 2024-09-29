using System;
using System.Collections.Generic;
using GestionFlotte.Classes;

namespace GestionFlotte.Classes
{
    public class Chauffeur
    {
        public string Nom { get; set; }
        public string Permis { get; set; }
        public int Anciennete { get; set; }
        public List<Vehicule> VehiculesAutorises { get; set; }  // Liste des véhicules que ce chauffeur est autorisé à conduire
        public List<Trajet> HistoriqueTrajets { get; set; }  // Liste des trajets effectués par le chauffeur

        public Chauffeur(string nom, string permis, int anciennete)
        {
            Nom = nom;
            Permis = permis;
            Anciennete = anciennete;
            VehiculesAutorises = new List<Vehicule>();
            HistoriqueTrajets = new List<Trajet>();  // Initialisation de la liste des trajets
        }

        // Ajoute un véhicule à la liste des véhicules autorisés
        public void AjouterVehiculeAutorise(Vehicule vehicule)
        {
            VehiculesAutorises.Add(vehicule);
        }

        // Ajoute un trajet à l'historique des trajets du chauffeur
        public void AjouterTrajet(Trajet trajet)
        {
            HistoriqueTrajets.Add(trajet);
        }
    }
}
