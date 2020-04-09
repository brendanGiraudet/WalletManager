﻿using System;

namespace WalletManagerDTO
{
    public class Transaction
    {
        public string Compte { get; set; }

        public DateTime ComptabilisationDate { get; set; }

        public DateTime OperationDate { get; set; }

        public string Label { get; set; }

        public string Reference { get; set; }

        public DateTime ValueDate { get; set; }

        public double Amount { get; set; }

        public Enumerations.TransactionCategory Category { get; set; }
    }
}
