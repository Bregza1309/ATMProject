namespace ATM_EFCore
{
    public class Account
    {
        public int AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public decimal? Balance { get; set; }
        public int  Pin { get; set; }

        public override string ToString()
        {
            return $@"Account Information
AccountId   : {AccountId}
FirstName   : {FirstName}
LastName    : {LastName}
Balance     : {Balance:C}";
        }
    }
}
