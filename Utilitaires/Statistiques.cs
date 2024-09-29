using System;
using System.Collections.Generic;
using GestionFlotte.Classes; // Assurez-vous de bien inclure l'espace de noms nécessaire

namespace GestionFlotte.Utilitaires
{
    public static class Statistiques
    {
        // Méthode pour afficher le kilométrage total des véhicules
        public static void AfficherKilometrageTotal(List<Vehicule> flotte)
        {
            Console.WriteLine("\nKilométrage total des véhicules :");
            foreach (var vehicule in flotte)
            {
                Console.WriteLine($"{vehicule.Marque} {vehicule.Modele} - {vehicule.Kilometrage} km parcourus");
            }
        }

        // Méthode pour afficher la consommation totale de carburant
        public static void AfficherConsommationTotale(List<Vehicule> flotte)
        {
            Console.WriteLine("\nConsommation totale de carburant des véhicules :");
            foreach (var vehicule in flotte)
            {
                double consommationTotale = vehicule.Kilometrage * vehicule.ConsommationParKm;
                Console.WriteLine($"{vehicule.Marque} {vehicule.Modele} - {consommationTotale:F2} litres consommés");
            }
        }

        // Méthode pour afficher les chauffeurs les plus actifs
        public static void AfficherChauffeursLesPlusActifs(List<Chauffeur> chauffeurs)
        {
            Console.WriteLine("\nChauffeurs les plus actifs (nombre de trajets) :");
            foreach (var chauffeur in chauffeurs)
            {
                Console.WriteLine($"{chauffeur.Nom} - {chauffeur.HistoriqueTrajets.Count} trajets");
            }
        }

        // Ajoutez d'autres méthodes ici...
    }
}
