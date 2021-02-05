using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletManagerDAL.Serializer
{
    public interface ISerializer<T>
    {
        Task<bool> Serialize(IEnumerable<T> objects, string filePath);

        IEnumerable<T> Deserialize(IEnumerable<string> lines);

    }
}