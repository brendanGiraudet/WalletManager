using System.Collections.Generic;

namespace WalletManagerDAL.Serializer
{
    public interface ISerializer<T>
    {
        bool Serialize(IEnumerable<T> objects, string filePath);

        IEnumerable<T> Deserialize(string filePath);

    }
}