﻿using System.Collections.Generic;

namespace WalletManagerDAL.Serializer
{
    public interface ISerializer
    {
        void Serialize(List<WalletManagerDTO.Transaction> transactions, string path);

        List<WalletManagerDTO.Transaction> Deserialize(string csvPath);
    }
}