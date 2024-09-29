using System;
using System.Collections.Generic;
using GestionFlotte.Classes;

namespace GestionFlotte.Classes
{
    public class Voiture : Vehicule
    {
        public Voiture(string immatriculation, string marque, string modele, int kilometrage, double consommationParKm)
            : base(immatriculation, marque, modele, "Voiture", kilometrage, consommationParKm)
        {
        }

        public override void AfficherDetails()
        {
            Console.WriteLine($"Voiture {Marque} {Modele} - {Immatriculation}, {Kilometrage} km");
        }
    }
}
