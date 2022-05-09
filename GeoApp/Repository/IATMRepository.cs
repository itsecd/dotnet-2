using GeoApp.Model;
using System.Collections.Generic;

namespace GeoApp.Repository

{
    public interface IATMRepository
    {
        JsonATM GetATMById(string id);

        JsonATM ChangeBalanceById(string id, int balance);

        List<JsonATM> GetAllATMs();
    }
}