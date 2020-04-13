using System;
using System.Collections.Generic;
using System.Text;

namespace WalletManagerDTO.Exceptions
{
    public class TransactionServiceException : Exception
    {
        public TransactionServiceException()
        {

        }

        public TransactionServiceException(string message) : base(message)
        {

        }

        public TransactionServiceException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
