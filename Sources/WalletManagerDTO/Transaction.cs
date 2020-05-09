using System;

namespace WalletManagerDTO
{
    public class Transaction
    {
        public string Compte { get; set; }

        public DateTime OperationDate { get; set; }

        public string Label { get; set; }

        public string Reference { get; set; }

        public decimal Amount { get; set; }

        public Enumerations.TransactionCategory Category { get; set; }

        public override string ToString()
        {
            return $"Compte : {Compte}, Operation Date : {OperationDate}, Label : {Label}, Reference : {Reference}, Amount : {Amount}, Category : {Category}";

        }
    }
}
