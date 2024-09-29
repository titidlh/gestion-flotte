using System;
using System.Collections.Generic;
using GestionFlotte.Classes;


namespace GestionFlotte.Classes
{
    public class Camion : Vehicule
    {
        public Camion(string immatriculation, string marque, string modele, int kilometrage, double consommationParKm)
            : base(immatriculation, marque, modele, "Camion", kilometrage, consommationParKm)
        {
        }

        public override void AfficherDetails()
        {
            Console.WriteLine($"Camion {Marque} {Modele} - {Immatriculation}, {Kilometrage} km");
        }
    }
}
