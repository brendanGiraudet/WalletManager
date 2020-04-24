using System.Collections.Generic;
using System.IO;

namespace WalletManagerDAL.Serializer
{
    public interface ISerializer
    {
        void Serialize(List<WalletManagerDTO.Transaction> transactions, string path);

        List<WalletManagerDTO.Transaction> Deserialize(string csvPath);

        List<WalletManagerDTO.Transaction> Deserialize(Stream stream);
    }
}