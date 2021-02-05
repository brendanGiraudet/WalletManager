using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WalletManagerDAL.File;
using WalletManagerDTO;
using WalletManagerDTO.Exceptions;

namespace WalletManagerDAL.Serializer
{
    public class CategorySerializer : ISerializer<Category>
    {
        readonly char _columnSeparator = ';';
        private readonly IFileService _fileService;

        public CategorySerializer(IFileService fileService)
        {
            _fileService = fileService;
        }

        async Task<bool> ISerializer<Category>.Serialize(IEnumerable<Category> categories, string filePath)
        {
            if (categories == null || !categories.Any()) throw new SerializerException("Impossible to serialize an empty category list");

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("Name;CreationDate");
            stringWriter.Write(stringWriter.NewLine);

            foreach (var category in categories)
            {
                stringWriter.Write(category.Name + _columnSeparator);
                stringWriter.Write(category.CreationDate.ToString() + _columnSeparator);
                stringWriter.Write(stringWriter.NewLine);
            }

            try
            {
                var fileServiceresponse = await _fileService.Write(filePath, stringWriter.ToString());
                return !fileServiceresponse.HasError;
            }
            catch (Exception ex)
            {
                throw new SerializerException($"Error : impossible to serialize categories in path { filePath } due to " + ex.Message, ex);
            }
        }

        IEnumerable<Category> ISerializer<Category>.Deserialize(IEnumerable<string> lines)
        {
            var categories = new List<Category>();
            try
            {
                lines = lines.Skip(1);// Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(';').ToArray();
                    var category = new Category
                    {
                        Name = values[0],
                        CreationDate = Convert.ToDateTime(values[1]),
                    };

                    categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                throw new SerializerException($"Error : impossible to deserialize categories due to " + ex.Message, ex);
            }

            return categories;
        }
    }
}
