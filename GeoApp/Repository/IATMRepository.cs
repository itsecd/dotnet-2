using GeoApp.Model;
using System.Collections.Generic;

namespace GeoApp.Repository

{
    public interface IATMRepository
    {
        void InsertATM(ATM ATM);
        ATM GetATMById(string id);

        ATM ChangeBalanceById(string id, int balance);

        ATM DeleteATMById(string id);

        List<ATM> GetAllATMs();
    }
}