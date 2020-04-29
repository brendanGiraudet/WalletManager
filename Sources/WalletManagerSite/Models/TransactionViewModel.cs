using System;
using WalletManagerDTO.Enumerations;

namespace WalletManagerSite.Models
{
    public class TransactionViewModel
    {
        public string Compte { get; set; }

        public DateTime ComptabilisationDate { get; set; }

        public DateTime OperationDate { get; set; }

        public string Label { get; set; }

        public string Reference { get; set; }

        public DateTime ValueDate { get; set; }

        public double Amount { get; set; }

        public TransactionCategory Category { get; set; } = TransactionCategory.NA;

        public string Color { get; set; }
    }
}
