using System.Collections.Generic;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public interface ICsvSerializer
    {
        List<Transaction> Deserialize(IEnumerable<string> csvLines);
    }
}