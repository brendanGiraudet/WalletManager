using System;
using System.Collections.Generic;

namespace WalletManagerServices.Serializer
{
    public interface ISerializer
    {
        void Serialize(List<WalletManagerDTO.Transaction> transactions);

        List<WalletManagerDTO.Transaction> Deserialize(string csvPath);
    }
}