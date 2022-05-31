using GeoAppATM.Model;
using System.Collections.Generic;

namespace GeoAppATM.Repository
{
    public interface IAtmRepository
    {
        Atm GetAtmByID(string id);

        Atm ChangeBalanceByID(string id, int balance);

        List<Atm> GetAtms();
    }
}
