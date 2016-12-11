using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2CSharp;

namespace GranitXMLTemplate
{
    class TransactionAdapter
    {
        public string Originator
        {
            get
            {
                return Transaction.Originator.Account.AccountNumber;
            }
            set
            {
                Transaction.Originator.Account.AccountNumber = value;
            }
        }

        public string BeneficiaryName
        {
            get
            {
                return Transaction.Beneficiary.Name;
            }
            set
            {
                Transaction.Beneficiary.Name = value;
            }
        }

        public string BeneficiaryAccount
        {
            get
            {
                return Transaction.Beneficiary.Account.AccountNumber;
            }
            set
            {
                Transaction.Beneficiary.Account.AccountNumber = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return Transaction.Amount.Value;
            }
            set
            {
                Transaction.Amount.Value = value;
            }
        }

        public string Currency
        {
            get
            {
                return Transaction.Amount.Currency;
            }
            set
            {
                Transaction.Amount.Currency = value;
            }
        }
        public DateTime ExecutionDate
        {
            get { return DateTime.Parse(Transaction.RequestedExecutionDate); }
            set { Transaction.RequestedExecutionDate = value.ToString("yyyy-MM-dd"); }
        }

        public string Comment 
        {
            get { return Transaction.RemittanceInfo.Text[0]; }
            set { Transaction.RemittanceInfo.Text[0] = value; }
        }

        private Transaction Transaction { get; set; }

        public TransactionAdapter( Transaction t )
        {
            Transaction = t;
        }

    }
}
