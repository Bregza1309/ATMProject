using static System.Console;
using System.Xml.Serialization;
using static System.Environment;
using static System.IO.Path;

namespace AtmClassLibrary
{
    public class PremiumAccount : Account
    {

        private SavingsAccount? PremiumSavings; 

        public PremiumAccount(string fname, string lname, decimal debit) : base(fname, lname, debit)
        {
            PremiumSavings = new(fname, lname, 0, 0.1M);
            creditLimit = 5000M;
        }

        public PremiumAccount():base() {
            PremiumSavings = new();
            PremiumSavings.AccountId = AccountId;
            creditLimit = 5000M;
        }
        /// <summary>
        /// Deposits into the Internal Savings Account
        /// </summary>
        /// <param name="amount">Amount to be deposited</param>
        /// <param name="fromAccount">Flag to check if the deposit amount is coming from the holders account</param>
        public void DepositIntoSavings(decimal  amount)
        {
                PremiumSavings.Debit += amount;  
        }
        public void setSavingsDetails(string firstname,string lastname)
        {
            PremiumSavings.FirstName = firstname;
            PremiumSavings.LastName = lastname;
        }

        /// <summary>
        /// Withdraw from Premium Savings Account
        /// </summary>
        /// <param name="amount">Amount to be withdrawn</param>
        /// <param name="intoAccount">Flag to check if the amount should be deposited to the current account</param>
        /// <exception cref="ArgumentException"></exception>
        public void WithdrawFromSavings(decimal amount, bool intoAccount = false)
        {
            if(PremiumSavings?.Debit < amount)
            {
                throw new ArgumentException("Insuffucient funds in Savings Account");
            }
            if(intoAccount)
            {
                PremiumSavings.Debit -= amount;
                this.Debit += amount;
                WriteLine($"Successfully Deposited {amount}into Your Current Account from Your Premium Savings");
            }
            else
            {
                PremiumSavings.Debit -= amount;
                WriteLine("Successfully Withdrawn from Your Premium Savings");
            }
        }
        public decimal ? SavingsBalance()
        {
            return PremiumSavings.Balance();
        }
        public override string ToString()
        {
            return $"\n\nPremium Account\n" +
                $"AcccountId : {AccountId}\n" +
                $"FirstName  :{FirstName}\n" +
                $"LastName   :{LastName}\n" +
                $"Interest Earned on PremiumSavings : {PremiumSavings.InterestRate * PremiumSavings.Debit:C}\n" +
                $"Balance in {nameof(PremiumSavings)} : {PremiumSavings.Balance():C}\n" +
                $"Balance in Current Account : {Balance():C}\n" +
                $"Total Balance in Premium Account (including Savings) : {Balance() + PremiumSavings.Balance():C}\n";
        }

        
    }
}
