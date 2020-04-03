using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
