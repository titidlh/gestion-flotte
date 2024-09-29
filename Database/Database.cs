using System;
using GestionFlotte.Classes;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GestionFlotte.Database
{
    public class Database
    {
        private SQLiteConnection connection;

        public Database(string dbPath)
        {
            connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture de la connexion : {ex.Message}");
            }
        }

        public void CreerTables()
        {
            string createVehiculesTable = @"
                CREATE TABLE IF NOT EXISTS Vehicules (
                    Immatriculation TEXT PRIMARY KEY,
                    Marque TEXT NOT NULL,
                    Modele TEXT NOT NULL,
                    Type TEXT NOT NULL,
                    Kilometrage INTEGER NOT NULL,
                    ConsommationParKm REAL NOT NULL
                )";

            string createChauffeursTable = @"
                CREATE TABLE IF NOT EXISTS Chauffeurs (
                    Nom TEXT PRIMARY KEY,
                    Permis TEXT NOT NULL,
                    Anciennete INTEGER NOT NULL
                )";

            string createTrajetsTable = @"
                CREATE TABLE IF NOT EXISTS Trajets (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Depart TEXT NOT NULL,
                    Arrivee TEXT NOT NULL,
                    Distance REAL NOT NULL,
                    DureeEstimee TEXT NOT NULL,
                    VehiculeImmatriculation TEXT NOT NULL,
                    ChauffeurNom TEXT NOT NULL,
                    FOREIGN KEY (VehiculeImmatriculation) REFERENCES Vehicules(Immatriculation),
                    FOREIGN KEY (ChauffeurNom) REFERENCES Chauffeurs(Nom)
                )";

            ExecuteNonQuery(createVehiculesTable);
            ExecuteNonQuery(createChauffeursTable);
            ExecuteNonQuery(createTrajetsTable);
        }

        private void ExecuteNonQuery(string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
                }
            }
        }

        public void AjouterVehicule(string immatriculation, string marque, string modele, string type, int kilometrage, double consommationParKm)
        {
            if (ExisteVehicule(immatriculation)) return;

            string query = @"
                INSERT INTO Vehicules (Immatriculation, Marque, Modele, Type, Kilometrage, ConsommationParKm)
                VALUES (@Immatriculation, @Marque, @Modele, @Type, @Kilometrage, @ConsommationParKm)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Immatriculation", immatriculation);
                command.Parameters.AddWithValue("@Marque", marque);
                command.Parameters.AddWithValue("@Modele", modele);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Kilometrage", kilometrage);
                command.Parameters.AddWithValue("@ConsommationParKm", consommationParKm);
                command.ExecuteNonQuery();
            }
        }

        public void AjouterChauffeur(string nom, string permis, int anciennete)
        {
            if (ExisteChauffeur(nom)) return;

            string query = @"
                INSERT INTO Chauffeurs (Nom, Permis, Anciennete)
                VALUES (@Nom, @Permis, @Anciennete)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nom", nom);
                command.Parameters.AddWithValue("@Permis", permis);
                command.Parameters.AddWithValue("@Anciennete", anciennete);
                command.ExecuteNonQuery();
            }
        }

        private bool ExisteVehicule(string immatriculation)
        {
            string query = "SELECT COUNT(*) FROM Vehicules WHERE Immatriculation = @Immatriculation";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Immatriculation", immatriculation);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private bool ExisteChauffeur(string nom)
        {
            string query = "SELECT COUNT(*) FROM Chauffeurs WHERE Nom = @Nom";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nom", nom);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void FermerConnexion()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la fermeture de la connexion : {ex.Message}");
            }
        }

        public List<Vehicule> RecupererVehicules()
        {
            List<Vehicule> vehicules = new List<Vehicule>();
            string query = "SELECT * FROM Vehicules";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string? immatriculation = reader["Immatriculation"]?.ToString();
                    string? marque = reader["Marque"]?.ToString();
                    string? modele = reader["Modele"]?.ToString();
                    string type = reader["Type"].ToString();
                    int kilometrage = Convert.ToInt32(reader["Kilometrage"]);
                    double consommation = Convert.ToDouble(reader["ConsommationParKm"]);

                    if (immatriculation != null && marque != null && modele != null)
                    {
                        Vehicule vehicule;
                        if (type == "Voiture")
                            vehicule = new Voiture(immatriculation, marque, modele, kilometrage, consommation);
                        else if (type == "Camion")
                            vehicule = new Camion(immatriculation, marque, modele, kilometrage, consommation);
                        else
                            vehicule = new Moto(immatriculation, marque, modele, kilometrage, consommation);

                        vehicules.Add(vehicule);
                    }
                }
            }
            return vehicules;
        }

        public List<Chauffeur> RecupererChauffeurs()
        {
            List<Chauffeur> chauffeurs = new List<Chauffeur>();
            string query = "SELECT * FROM Chauffeurs";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string? nom = reader["Nom"]?.ToString();
                    string? permis = reader["Permis"]?.ToString();
                    int anciennete = Convert.ToInt32(reader["Anciennete"]);

                    if (nom != null && permis != null)
                    {
                        chauffeurs.Add(new Chauffeur(nom, permis, anciennete));
                    }
                }
            }
            return chauffeurs;
        }

        public void AjouterTrajet(string depart, string arrivee, double distance, string dureeEstimee, string vehiculeImmatriculation, string chauffeurNom)
        {
            string query = @"
                INSERT INTO Trajets (Depart, Arrivee, Distance, DureeEstimee, VehiculeImmatriculation, ChauffeurNom)
                VALUES (@Depart, @Arrivee, @Distance, @DureeEstimee, @VehiculeImmatriculation, @ChauffeurNom)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Depart", depart);
                command.Parameters.AddWithValue("@Arrivee", arrivee);
                command.Parameters.AddWithValue("@Distance", distance);
                command.Parameters.AddWithValue("@DureeEstimee", dureeEstimee);
                command.Parameters.AddWithValue("@VehiculeImmatriculation", vehiculeImmatriculation);
                command.Parameters.AddWithValue("@ChauffeurNom", chauffeurNom);
                command.ExecuteNonQuery();
            }
        }

        public void ModifierVehicule(string immatriculation, int nouveauKilometrage)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "UPDATE Vehicules SET Kilometrage = @kilometrage WHERE Immatriculation = @immatriculation";
                cmd.Parameters.AddWithValue("@kilometrage", nouveauKilometrage);
                cmd.Parameters.AddWithValue("@immatriculation", immatriculation);
                cmd.ExecuteNonQuery();
            }
        }

        public void SupprimerVehicule(string immatriculation)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "DELETE FROM Vehicules WHERE Immatriculation = @immatriculation";
                cmd.Parameters.AddWithValue("@immatriculation", immatriculation);
                cmd.ExecuteNonQuery();
            }
        }

        public void ModifierChauffeur(string nom, int nouvelleAnciennete)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "UPDATE Chauffeurs SET Anciennete = @anciennete WHERE Nom = @nom";
                cmd.Parameters.AddWithValue("@anciennete", nouvelleAnciennete);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.ExecuteNonQuery();
            }
        }

        public void SupprimerChauffeur(string nom)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "DELETE FROM Chauffeurs WHERE Nom = @nom";
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
