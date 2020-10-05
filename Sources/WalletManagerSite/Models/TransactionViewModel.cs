using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using WalletManagerDTO;

namespace WalletManagerSite.Models
{
    public class TransactionViewModel
    {
        public string Compte { get; set; }

        public DateTime OperationDate { get; set; }

        public string Label { get; set; }

        public string Reference { get; set; }

        public decimal Amount { get; set; }

        public Category Category
        {
            get => new Category { Name = CategoryName };
            set => CategoryName = value.Name;
        }

        public string CategoryName { get; set; }

        public string Color { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public SelectList CategoriesToSelect
        {
            get
            {
                return new SelectList(Categories, nameof(Category.Name), nameof(Category.Name));
            }
        }
    }
}
