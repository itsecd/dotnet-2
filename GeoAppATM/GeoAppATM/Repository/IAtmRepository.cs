using GeoAppATM.Model;
using System.Collections.Generic;

namespace GeoAppATM.Repository
{
    public interface IAtmRepository
    {
        Atm GetAtmById(string id);

        Atm ChangeBalanceById(string id, int balance);

        List<Atm> GetAtms();
    }
}
