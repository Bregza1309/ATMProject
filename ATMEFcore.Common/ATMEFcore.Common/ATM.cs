using Microsoft.EntityFrameworkCore;
using static System.Console;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ATM_EFCore
{
    public class ATM
    {
        Regex pinChecker = new(@"^[0-9]{5}$");
        Regex nameChecker = new(@"^[A-Z][a-z]+$");
        private Account? currentHolder { get; set; }

        public ATM()
        {
            using (ATMdb db = new())
            {
                bool available = db.Database.CanConnect();
                if(!available)
                {
                    bool created = db.Database.EnsureCreated();
                    WriteLine($"ATM Database Created : {created}");
                }
            }
        }
        public void OpenAccount()
        {
            string? fname, lname , password;
            bool  exist;
            Account? account = new();
            //get User Details
            Write("Enter your FirstName : ");
            
            fname = ReadLine();
            if (nameChecker.IsMatch(fname))
            {
                account.FirstName = fname;
            }
            else
            {
                throw new InvalidDataException("FirstName not in Correct Format");
            }

            Write("Enter your LastName : ");
            lname  = ReadLine();

            if (nameChecker.IsMatch(lname))
            {
                account.LastName = lname;
            }
            else
            {
                throw new InvalidDataException("LastName not in Correct Format");
            }

            Write("Enter your Password : ");
            password = ReadLine();

            
            if(pinChecker.IsMatch(password)) { 
                account.Pin = int.Parse(password);
            }
            else
            {
                throw new InvalidDataException("Pin must 5 numbers");
            }
            //account.AccountId = (new Random()).Next(780000, 789999);
            account.Balance= 0;
            currentHolder = account;
            //Check if Account Exist
            using (ATMdb db = new())
            {
                try
                {
                    Account? accounts = db.Accounts?
                        .First(account => account.FirstName == fname && account.LastName == lname);
                    exist = true;
                }
                catch (InvalidOperationException)
                {
                    exist= false;
                }
               
            }
            if (exist)
            {
                throw new InvalidDataException("Account Already Exist");
            }
            //Add Account to Database
            using (ATMdb db = new())
            {
                db.Accounts.Add(currentHolder);

                int affected = db.SaveChanges();
                if (affected > 0)
                {
                    WriteLine("Account has been Created ");
                }
            }
        }
        bool Authenticate(string fname,string lname , int pin)
        {
            using (ATMdb db = new())
            {
                IQueryable<Account>? account = db.Accounts?
                    .Where(a => a.FirstName == fname && a.LastName == lname
                    && a.Pin == pin);
                try
                {
                    currentHolder = new() { FirstName = fname, LastName = lname, Balance = account.First().Balance, AccountId = account.First().AccountId };
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                
                return true;
            }
        }
        public void login()
        {
            string? fname, lname;
            int pin;
            Write("Enter your FirstName :");
            fname = ReadLine();
            if (!nameChecker.IsMatch(fname))
            {
                throw new InvalidDataException("FirstName not in correct format");
            }
            Write("Enter your LastName  :");
            lname = ReadLine();
            if (!nameChecker.IsMatch(lname))
            {
                throw new InvalidDataException("LastName not in correct format");
            }
            Write("Enter your Pin : ");
            if(int.TryParse(ReadLine(),out pin))
            {
                if (!pinChecker.IsMatch($"{pin}"))
                {
                    throw new InvalidDataException("Pin must be 5 numbers");
                }
            }
            else
            {
                throw new InvalidDataException("Pin must be 5 numbers");
            }
            
            //Authenticate
            if(Authenticate(fname, lname, pin))
            {
                WriteLine("Login Successfull");
            }
            else
            {
                WriteLine("Login Unsuccesfull");
            }
        }
        public void Deposit()
        {
            decimal amount;
            WriteLine("Enter amount to Deposit : ");
            if (decimal.TryParse(ReadLine() ,out amount))
            {
                if(amount % 10 != 0) {
                    WriteLine("Amount should be in multiples of 10\n\n");
                    Deposit();
                }
            }
            else
            {
                throw new InvalidDataException("Amount not in correct format");
            }

            using (ATMdb db = new())
            {
                Account account = db.Accounts.First(
                    a => a.AccountId == currentHolder.AccountId);
                account.Balance += amount;
                currentHolder.Balance += amount;
                if(db.SaveChanges() > 0)
                {
                    WriteLine("Deposit Successfull");
                }
                else
                {
                    WriteLine("Deposit Unsuccessfull");
                }

            }
        }
        public void Withdraw()
        {
            decimal amount;
            WriteLine("Enter amount to Withdraw : ");
            if(!decimal.TryParse(ReadLine(),out amount))
            {
                throw new InvalidDataException("Amount should only contain digits");
            }
            if(amount % 10 != 0)
            {
                WriteLine("Amount should be multiples of 10");
                Withdraw();
            }
            if(amount < 100M)
            {
                WriteLine($"Amount should be greater than {100:C}");
            }
            if(amount > currentHolder.Balance)
            {
                throw new InvalidDataException("Insufficient funds");
            }
            //Update the Account
            using (ATMdb db = new())
            {
                Account account = db.Accounts.First(
                    a => a.AccountId == currentHolder.AccountId);

                account.Balance -= amount;
                currentHolder.Balance-= amount;

                if(db.SaveChanges() > 0)
                {
                    WriteLine("Withdrawal Successful");
                }
                else
                {
                    WriteLine("Withdrawal UnSuccessful");
                }
            }
        }
        public void runSim()
        {
            try
            {
                Write(@"Welcome to The Money Machine
1.Login
2.Open New Account
Your Choice : ");
                ConsoleKey key = ReadKey().Key;
                WriteLine();
                if (key == ConsoleKey.D1)
                {
                    login();
                }
                else if (key == ConsoleKey.D2)
                {
                    OpenAccount();
                }
                else
                {
                    throw new InvalidDataException("Invalid Choice");
                }

                do
                {
                    Write("\n\n");
                    Write(@"1.Deposit
2.Withdraw
3.Balance
4.Account Details
5.Exit
Your Choice : ");
                    key = ReadKey().Key;
                    WriteLine();
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            {
                                Deposit();
                                break;
                            }
                        case ConsoleKey.D2:
                            {
                                Withdraw();
                                break;
                            }
                        case ConsoleKey.D3:
                            {
                                WriteLine($"Balance : {currentHolder.Balance:C}");
                                break;
                            }
                        case ConsoleKey.D4:
                            {
                                WriteLine(this);
                                break;
                            }
                        case ConsoleKey.D5:
                            {
                                WriteLine("Thank you for using the Money Machine");
                                return;
                            }
                        default:
                            {
                                WriteLine("Invalid Choice");
                                break;
                            }
                    }

                } while (true);
            }
            catch(Exception ex)
            {
                WriteLine($"Error : {ex.Message}");
            }
        }
        public override string ToString()
        {
            return currentHolder.ToString();
        }
    }
}
