using System;

namespace WalletManagerDTO
{
    public class Category
    {
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return $"Category Name : {Name}, Creation Date : {CreationDate}";

        }
    }
}
