using static System.Console;
using static System.IO.Path;
using static System.Environment;
using static System.Console;
using System.Xml.Serialization;

namespace AtmClassLibrary
{
    public class ATM
    {

        private decimal[] rates = { 0.03M,0.04M,0.05M};

        private Account? currentHolder;

        //Method to Create an account
        public  void OpenAccount()
        {
            WriteLine("\n\nChoose Account Type :\n" +
                "1.Savings Account\n" +
                "2.Current Account\n" +
                "3.Premium Account\n" +
                "Your Choice : ");

            int userChoice = int.Parse(ReadLine());

            currentHolder = userChoice switch
            {
                1 => new SavingsAccount(),
                2 => new CurrentAccount(),
                3 => new PremiumAccount(),
                _ => throw new InvalidOperationException("Invalid Choice From the Menu")
            };

            Write("Enter your FirstName : ");
            currentHolder.FirstName= ReadLine().ToUpper();
            Write("Enter your LastName : ");
            currentHolder.LastName = ReadLine().ToUpper();
            Write("Enter Opening Balance: ");
            currentHolder.Debit = decimal.Parse(ReadLine());
            if (currentHolder is SavingsAccount Holder)
            {
                Write("Enter Interest Rate");
                //must fix these occurances!!!
                Holder.InterestRate = decimal.Parse(ReadLine());
                currentHolder = Holder;
            }
            if (currentHolder is PremiumAccount holder)
            {
                holder.setSavingsDetails(holder.FirstName, holder.LastName);
                currentHolder = holder;
            }
        }
        /// <summary>
        /// Deposits into Account`s Debit 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Deposit()
        {
            decimal amount;
            if (currentHolder is PremiumAccount holder)
            {
                Write("\n\nChoose Account To Deposit : \n" +
                    "1.Current Account\n" +
                    "2.Premium Savings Account\n" +
                    "Your Choice : ");
                int choice = int.Parse(ReadLine());

                if (choice > 2)
                {
                    throw new InvalidOperationException("Invalid Choice");
                }
                Write("Enter amount to  Deposit : ");
                amount = decimal.Parse(ReadLine());

                if (choice == 1)
                {
                    holder.Debit += amount;
                }
                if (choice == 2)
                {
                    holder.DepositIntoSavings(amount);

                }

                currentHolder = holder;

            }
            else
            {
                Write("Enter amount to deposit : ");
                if (decimal.TryParse(ReadLine(), out amount))
                {
                    if (currentHolder.Credit > 0)
                    {
                        currentHolder.Credit -= amount;
                        return;
                    }
                    currentHolder.Debit += amount;
                }
                else
                {
                    WriteLine("Enter valid amount");
                }
               
            }

            WriteLine("Deposit Successfull");
        }
        /// <summary>
        /// Method to Withdraw from an Account
        /// </summary>
        /// <returns>true if Withdrawal was Successsfull false otherwise</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Withdraw()
        {
            Write("Enter Amount to Withdraw:");
            int amount = int.Parse(ReadLine());

            if (currentHolder is PremiumAccount holder)
            {
                Write("\n\nSelect Account \n" +
                    "1.Current Account\n" +
                    "2.PremiumSavings Account\n" +
                    "Your choice : ");
                int userChoice = int.Parse(ReadLine());

                //check if holder has sufficent funds for the transaction
                if (holder.Balance() < amount && holder.SavingsBalance() < amount)
                {
                    //Ask user to withdraw from Credit Account
                    if (holder.creditLimit >= (holder.Credit + amount))
                    {
                        Write("Insufficient funds in Debit Account \n" +
                        "Would you like to Withdraw from Credit Account? <yes:1 No :0 >");
                        int Choice = int.Parse(ReadLine());
                        if (Choice == 1)
                        {
                            holder.Credit += amount;
                            return true;
                        }
                        if (Choice == 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        WriteLine("Credit Limit Reached");
                        return false;
                    }
                }
                if (userChoice > 2)
                {
                    throw new InvalidOperationException("Invalid Choice");
                }
                if (userChoice == 1)
                {
                    holder.Debit -= amount;

                }
                if (userChoice == 2)
                {
                    holder.WithdrawFromSavings(amount);
                }
                return true;
            }
            else
            {
                if (currentHolder.Debit < amount)
                {
                    if (currentHolder.creditLimit >= currentHolder.Credit + amount)
                    {
                        Write("Insufficient funds in Debit Account \n" +
                                "Would you like to Withdraw from Credit Account? <yes:1 No :0 >");
                        int Choice = int.Parse(ReadLine());
                        if (Choice == 1)
                        {
                            currentHolder.Credit += amount;
                            return true;
                        }
                        if (Choice != 1 | Choice != 0)
                        {
                            throw new InvalidOperationException("Invalid Choice");
                        }
                        if (Choice == 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        WriteLine("Credit Limit Reached");
                        return false;
                    }
                }
                else
                {
                    //Sufficient funds
                    currentHolder.Debit -= amount;
                }
            }
            return true;            
        }
        public virtual void Deserialize(long AccountId)
        {
            string filePath = Combine(CurrentDirectory, $"{AccountId}.xml");
            if (!File.Exists(filePath))
            {
                throw new InvalidDataException("AccountID not found");
            }
            WriteLine(@"Select Account
[1] SavingsAccount
[2] Current Account
[3] Premium Account
Press  NUM that matches your account");
            ConsoleKey option = ReadKey().Key;

            currentHolder = option switch
            {
                ConsoleKey.D1 => new SavingsAccount(),
                ConsoleKey.D2 => new CurrentAccount(),
                ConsoleKey.D3 => new PremiumAccount(),
                _ => throw new InvalidDataException("Invalid Option")
            };
            XmlSerializer xs = new(currentHolder.GetType());
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                currentHolder = option switch
                {
                    ConsoleKey.D1 => xs.Deserialize(stream) as SavingsAccount,
                    ConsoleKey.D2 => xs.Deserialize(stream) as CurrentAccount,
                    ConsoleKey.D3 => xs.Deserialize(stream) as PremiumAccount,
                    _ => throw new InvalidDataException("Invalid Choice")
                };
                WriteLine("\nSuccessfully Loaded Account");
            }
        }
        /// <summary>
        /// Method to Run the ATM Simulation
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void runSim()
        {

            do
            {
                //Menu
                Write("\n\nWelcome To The Money Machine Menu\n" +
                    "1.Open Account\n" +
                    "2.Withdraw\n" +
                    "3.Deposit\n" +
                    "4.Balance\n" +
                    "5.Account Details/Info\n" +
                    "6.Save Account\n" +
                    "7.Load Account\n" +
                    "Your Choice : ");

                int? userChoice = int.Parse(ReadLine());
                //check if user`s Selection is in the domain
                switch(userChoice)
                {
                    case 1:
                        {
                            if(currentHolder != null)
                            {
                                WriteLine($"\n\nAccount : {currentHolder.AccountId} \n" +
                                    $"Is Currently In Session ");
                                break;
                            }
                            OpenAccount();
                            break;
                        }
                    case 2:
                        {
                            if(currentHolder == null)
                            {
                                WriteLine("\n\nOpen an Account to start Withdrawing funds");
                                break;
                            }
                            Withdraw();
                            break;
                        }
                    case 3:
                        {
                            if (currentHolder == null)
                            {
                                WriteLine("\n\nOpen an Account to start Depositing funds");
                                break;
                            }
                            Deposit();
                            break;
                        }
                    case 4:
                        {
                            if (currentHolder == null)
                            {
                                WriteLine("\n\nOpen an Account to start Checking for balance");
                                break;
                            }
                            WriteLine($"\n\nBalance => AccountiD({currentHolder.AccountId}) : {currentHolder.Balance():C}");
                            break;
                        }
                    case 5:
                        {
                            if (currentHolder == null)
                            {
                                WriteLine("\n\nOpen an Account for Account Details");
                                break;
                            }
                            WriteLine(currentHolder);
                            break;
                        }
                    case 6:
                        {
                            if (currentHolder == null)
                            {
                                WriteLine("\n\nNo Account Opened");
                                break;
                            }
                            currentHolder.Serialize();
                            WriteLine($"\n\nAccount : {currentHolder.AccountId} has been Saved");
                            return;
                        }
                    case 7:
                        {
                            Write("Enter your Account Number : ");
                            long AccountId;
                            if(long.TryParse(ReadLine(),out AccountId))
                            {
                                Deserialize(AccountId);
                            }
                            else
                            {
                                throw new InvalidDataException("Invalid Account ID");
                            }
                            break;
                            
                        }
                    default:
                        {
                            throw new InvalidDataException("Invalid Choice From The Menu");
                        }
                }

            } while (true);
        }
        public  void CurrentAccountHolder()
        {
            if (currentHolder is PremiumAccount premiumAccountholder)
            {
                WriteLine(premiumAccountholder);
            }
            if (currentHolder is CurrentAccount currentAccount)
            {
                WriteLine(currentAccount);
            }
            if (currentHolder is SavingsAccount holder)
            {
                WriteLine(holder);
            }
        }
    }
}
