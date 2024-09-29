using System;
using System.Collections.Generic;
using GestionFlotte.Classes;

namespace GestionFlotte.Classes
{
    public class Moto : Vehicule
    {
        public Moto(string immatriculation, string marque, string modele, int kilometrage, double consommationParKm)
            : base(immatriculation, marque, modele, "Moto", kilometrage, consommationParKm)
        {
        }

        public override void AfficherDetails()
        {
            Console.WriteLine($"Moto {Marque} {Modele} - {Immatriculation}, {Kilometrage} km");
        }
    }
}
