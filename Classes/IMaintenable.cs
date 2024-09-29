using System;
using System.Collections.Generic;
using GestionFlotte.Classes;

namespace GestionFlotte.Classes
{
    public interface IMaintenable
    {
        void PlanifierMaintenance();
        bool EstMaintenanceDue();
    }
}
