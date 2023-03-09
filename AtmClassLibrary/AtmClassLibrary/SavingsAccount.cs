using System.Xml.Serialization;
using static System.Environment;
using static System.IO.Path;
using static System.Console;
using System.Xml.Serialization;

namespace AtmClassLibrary
{
    public class SavingsAccount : Account
    {
        private decimal interestRate;
        public SavingsAccount(string fname, string lname, decimal debit,decimal  Irate) : base(fname, lname, debit)
        {
            InterestRate = Irate;
            creditLimit = 2000M;
        }

        public SavingsAccount() : base()
        {
            InterestRate = 4M;
            creditLimit = 2000M;
        }

        //added functionality
        [XmlAttribute("InterestRate")]
        public decimal  InterestRate {
            get
            {
                return interestRate;
            }
            set
            {
                interestRate = value / 100M;
            }
        }

        public override decimal ? Balance()
        {
            return Debit * (1 + InterestRate);
        }

        public override string ToString()
        {
            return $"\n\nSavings Account\n" +
                $"AcccountId : {AccountId}\n" +
                $"FirstName  :{FirstName}\n" +
                $"LastName   :{LastName}\n" +
                $"Interest Earned : {Debit * InterestRate:C}\n" +
                $"Balance : {Balance():C}\n";
        }

        
    }
}
