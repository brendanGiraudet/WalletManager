using System;
using System.Collections.Generic;

namespace WalletManagerServices.Serializer
{
    public interface ISerializer
    {
        List<WalletManagerDTO.Transaction> Serialize(string csvString);
    }
}
