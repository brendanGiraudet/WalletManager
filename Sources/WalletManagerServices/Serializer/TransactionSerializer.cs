using System;
using System.Collections.Generic;

namespace WalletManagerServices.Serializer
{
    public class TransactionSerializer : ISerializer
    {
        public List<WalletManagerDTO.Transaction> Serialize(string csvString)
        {
               
            return new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Amount = 0,
                    ComptabilisationDate = DateTime.Now,
                    Compte = "",
                    Label = "",
                    OperationDate = DateTime.Now,
                    Reference = "",
                    ValueDate = DateTime.Now
                }
            };
        }
    }
}
