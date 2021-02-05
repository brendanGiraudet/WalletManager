using System.Threading.Tasks;
using WalletManagerDTO;

namespace WalletManagerServices.File
{
    public interface IFileService
    {
        Task<Response<bool>> Write(string filePath, string content);
        Task<Response<string[]>> Read(string filePath);
    }
}
