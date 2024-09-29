using System;
using System.Collections.Generic;
using GestionFlotte.Classes;


namespace GestionFlotte.Classes
{
    public class PermisInvalideException : Exception
    {
        public PermisInvalideException(string message) : base(message)
        {
        }
    }

    public class VehiculeIndisponibleException : Exception
    {
        public VehiculeIndisponibleException(string message) : base(message)
        {
        }
    }
}
