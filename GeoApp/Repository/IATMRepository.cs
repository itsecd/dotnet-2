using GeoApp.Model;
using System.Collections.Generic;

namespace GeoApp.Repository

{
    public interface IATMRepository
    {
        ATM InsertATM(ATM ATM);
        ATM GetATMById(string id);

        ATM ChangeBalanceById(string id, int balance);

        ATM DeleteATMById(string id);

        List<ATM> GetAllATMs();
    }
}