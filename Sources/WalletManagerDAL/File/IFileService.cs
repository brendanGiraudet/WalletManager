using System.Threading.Tasks;
using WalletManagerDTO;

namespace WalletManagerDAL.File
{
    public interface IFileService
    {
        Task<Response<bool>> Write(string filePath, string content);
        Task<Response<string[]>> Read(string filePath);
        Task<Response<bool>> Delete(string filePath);
    }
}
