
using System.Xml.Serialization;
using static System.IO.Path;
using static System.Environment;
using static System.Console;

namespace AtmClassLibrary
{
    public abstract class Account
    {   
        //private fields
        private string  firstName;
        private string lastName;
        private decimal credit;
        private decimal debit;
        XmlSerializer ? xs;
        string ? filePath;


        //Constructors 
        public Account(string fname,string lname ,decimal debit)
        {
            this.firstName = fname;
            this.lastName = lname;
            this.debit= debit;
            this.credit = 0M;
            //randomly generate a new Account Id
            this.AccountId = (new Random()).NextInt64(430000,439999);
        }

        public Account() {
            this.AccountId = (new Random()).NextInt64(430000, 439999);
        }

        //properties
        [XmlAttribute("creditLimit")]
        public decimal creditLimit { get; set; }
        [XmlAttribute("ID")]
        public long AccountId { get; set; }
        [XmlAttribute("Fname")]
        public string  FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName= value;
            }
        }
        [XmlAttribute("Lname")]
        public string  LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName= value;
            }
        }
        [XmlAttribute("Debit")]
        public decimal Debit
        {
            get
            {
                if(debit == null)
                {
                    return 0M;
                }
                return debit;
            }
            set { 
                debit= value;
            }
        }
        [XmlAttribute("Credit")]
        public decimal Credit
        {
            get
            {
                if(credit == null)
                {
                    return 0M;
                }
                return credit;
            }
            set
            {
                credit = value;
            }
        }
        //abstract method
        public virtual decimal? Balance()
        {
            return Debit - Credit;
        }
        public virtual void Serialize()
        {
            //instantiate the xmlSerializer
            xs = new(this.GetType());

            //filepath
            string filePath = Combine(CurrentDirectory, $"{this.AccountId}.xml");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //Serialize the Account
            using (FileStream stream = File.Create(filePath))
            {
                xs.Serialize(stream, this);
            }
            WriteLine("Successfully saved Account");
        }
        
        
    }
}