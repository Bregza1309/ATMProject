namespace AtmClassLibrary
{
    public class CurrentAccount : Account
    {
        
        public CurrentAccount(string fname, string lname, decimal debit) : base(fname, lname, debit)
        {
            creditLimit = 3000M;
        }

        public CurrentAccount() : base() { creditLimit = 3000M; }

        public override string ToString()
        {
            return $"\n\nCurrent Account\n" +
                $"AcccountId : {AccountId}\n"+
                $"FirstName  :{FirstName}\n" +
                $"LastName   :{LastName}\n" +
                $"Credit Balance : {Credit:C}\n" +
                $"Debit Balance   :{Debit:C}\n"+
                $"Balance : {Balance():C}\n";
        }
  
    }
}
