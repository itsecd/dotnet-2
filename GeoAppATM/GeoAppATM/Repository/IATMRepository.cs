using GeoAppATM.Model;
using System.Collections.Generic;

namespace GeoAppATM.Repository
{
    public interface IATMRepository
    {
        GeoJsonATM GetATMByID(string id);

        GeoJsonATM ChangeBalanceByID(string id, int balance);
        List<GeoJsonATM> GetAllATM();
    }
}
