using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GranitXMLEditor
{
    public class Constants
    {
        //xml
        public const string HUFTransactions = "HUFTransactions";
        public const string Account = "Account";
        public const string AccountNumber = "AccountNumber";
        public const string Originator = "Originator";
        public const string Beneficiary = "Beneficiary";
        public const string Name = "Name";
        public const string Amount = "Amount";
        public const string Currency = "Currency";
        public const string RemittanceInfo = "RemittanceInfo";
        public const string Text = "Text";
        public const string Transaction = "Transaction";
        public const string RequestedExecutionDate = "RequestedExecutionDate";
        public const string DateFormat = "yyyy-MM-dd";
        //DataGridView
        public const string OriginatorHeader = "Originator";
        public const string BeneficiaryAccount = "BeneficiaryAccount";
        public const string BeneficiaryAccountHeader = "BeneficiaryAccount";
        public const string BeneficiaryName = "BeneficiaryName";
        public const string BeneficiaryNameHeader= "BeneficiaryName";
        public const string AmountHeader = "Amount";
        public const string CurrencyHeader = "Currency";
        public const string RequestedExecutionDateHeader = "RequestedExecutionDate";
        public const string RemittanceInfoHeader = "Info";
    }
}
