using GeoApp.Model;
using System.Collections.Generic;

namespace GeoApp.Repository

{
    public interface IATMRepository
    {
        // XmlATM InsertATM(XmlATM ATM);
        JsonATM GetATMById(string id);

        JsonATM ChangeBalanceById(string id, int balance);

        // XmlATM DeleteATMById(string id);

        List<JsonATM> GetAllATMs();
    }
}