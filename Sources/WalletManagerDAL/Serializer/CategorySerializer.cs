using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO.Exceptions;

namespace WalletManagerDAL.Serializer
{
    public class CategorySerializer : ISerializer<string>
    {
        readonly char _columnSeparator = ';';

        bool ISerializer<string>.Serialize(IEnumerable<string> categories, string filePath)
        {
            if (categories == null || !categories.Any()) throw new SerializerException("Impossible to serialize an empty category list");

            try
            {
                var content = string.Join(_columnSeparator, categories);
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                throw new SerializerException($"Error : impossible to serialize categories in { filePath } due to " + ex.Message, ex);
            }
        }

        IEnumerable<string> ISerializer<string>.Deserialize(string filePath)
        {
            var categories = new List<string>();
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var inLineCategories = line.Split(_columnSeparator).ToList();
                    categories.AddRange(inLineCategories);
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
