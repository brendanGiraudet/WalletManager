using System.Collections.Generic;
using System.IO;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public interface ISerializer
    {
        void Serialize(List<Transaction> transactions, string path);

        List<Transaction> Deserialize(string csvPath);

        List<Transaction> Deserialize(Stream stream);

        List<Transaction> Deserialize(IEnumerable<string> csvLines);

        void Serialize(IEnumerable<string> categories, string csvPath);
    }
}