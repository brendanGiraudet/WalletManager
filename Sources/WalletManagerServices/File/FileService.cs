using System;
using System.Threading.Tasks;
using WalletManagerDTO;

namespace WalletManagerServices.File
{
    public class FileService : IFileService
    {
        public async Task<Response<string[]>> Read(string filePath)
        {
            var response = new Response<string[]>();
            if (string.IsNullOrEmpty(filePath))
            {
                response.ErrorsMessage = "The file path should not be empty";
                return response;
            }

            try
            {
                response.Content = await System.IO.File.ReadAllLinesAsync(filePath);
            }
            catch (Exception ex)
            {
                response.ErrorsMessage = ex.Message;
            }

            return response;
        }

        public async Task<Response<bool>> Write(string filePath, string content)
        {
            var response = new Response<bool>();
            if (string.IsNullOrEmpty(filePath))
            {
                response.ErrorsMessage = "The file path should not be empty";
                return response;
            }

            if (string.IsNullOrEmpty(content))
            {
                response.ErrorsMessage = "The content should not be empty";
                return response;
            }

            try
            {
                await System.IO.File.WriteAllTextAsync(filePath, content);
            }
            catch (Exception ex)
            {
                response.ErrorsMessage = ex.Message;
            }

            return response;
        }
    }
}