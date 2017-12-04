using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.Globalization;

namespace Bank
{
    public class Account
    {
        [Key]
        [ForeignKey("User")]
        public int id { get; set; }
        public int user_id { get; set; }
        public DateTime transaction_date { get; set; }
        public decimal Amount { get; set; }

        public Account()
        {
            transaction_date = new DateTime();
        }
    }

    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public void ViewBalance(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                Console.OutputEncoding = Encoding.UTF8;
                string suser = currentUser;
                var user = btx.Users.SingleOrDefault(r => r.username == suser);
                var account = btx.Accounts.SingleOrDefault(i => i.id == user.id);
                Console.WriteLine(account.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR")));
            }
        }
        //View normal account balance as Admin
        public void ViewBalance()
        {
            using (BankContext btx = new BankContext())
            {
                Console.OutputEncoding = Encoding.UTF8;
                while (true)
                {
                    Console.Clear();
                    Console.Write("Select user account you wish to access:\n>");
                    string suser = Console.ReadLine();
                    try
                    {
                        if (suser == "admin")
                        {
                            Console.WriteLine("To view the Internal Bank Account Balance, please select option number 1");
                        }

                        else
                        {
                            var user = btx.Users.SingleOrDefault(r => r.username == suser);
                            var account = btx.Accounts.SingleOrDefault(i => i.id == user.id);
                            Console.WriteLine(account.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR")));
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("The user you typed does not exist.\nPlease try again.");
                        Console.ReadKey();
                    }
                }

            }
        }

        //Deposit to Internal Bank Account
        public string DepositToInternal(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.Write("Select the amount you wish to deposit:\n>");
                    try
                    {
                        decimal depositedAmount = decimal.Parse(Console.ReadLine());
                        var Internalaccount = btx.Accounts.SingleOrDefault(i => i.id == 1);
                        var tempuser = currentUser;
                        var user = btx.Users.SingleOrDefault(r => r.username == tempuser);
                        var CurrentUserAccount = btx.Accounts.SingleOrDefault(i => i.id == user.id);
                        btx.Accounts.Update(Internalaccount);
                        btx.Accounts.Update(CurrentUserAccount);
                        Internalaccount.Amount += depositedAmount;
                        CurrentUserAccount.Amount -= depositedAmount;
                        btx.SaveChanges();
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("This is not a valid entry.Please try again.");
                        Console.ReadKey();
                    }
                }
            }
            return "Statement";
        }
        //Deposit to other accounts
        public string Deposit(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;

                    Console.WriteLine("Select user account you wish to deposit to");
                    string suser = Console.ReadLine();
                    string BaseUser = currentUser;
                    if (currentUser == "admin" && suser == "admin")
                    {
                        Console.WriteLine("Invalid operation.Administrators cannot deposit to Internal Bank Account.");
                        Console.ReadKey();
                        continue;
                    }
                    else if (currentUser != "admin" && currentUser == suser)
                    {
                        Console.WriteLine("Invalid operation.Only deposit to other members allowed");
                    }
                    else
                    {
                        try
                        {
                            var userToDeposit = btx.Users.SingleOrDefault(r => r.username == suser);
                            var accountToDeposit = btx.Accounts.SingleOrDefault(i => i.id == userToDeposit.id);
                            var ActiveUser = btx.Users.SingleOrDefault(x => x.username == BaseUser);
                            var ActiveAccount = btx.Accounts.SingleOrDefault(y => y.id == ActiveUser.id);
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Select the amount you wish to deposit:\n>");
                                    decimal depositedAmount = decimal.Parse(Console.ReadLine());
                                    btx.Accounts.Update(accountToDeposit);
                                    btx.Accounts.Update(ActiveAccount);
                                    accountToDeposit.Amount += depositedAmount;
                                    ActiveAccount.Amount -= depositedAmount;
                                    btx.SaveChanges();
                                    break;
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid entry, please retype the amount");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid entry, user does not exist");
                            Console.ReadKey();
                        }
                    }


                }
            }
            return "Statement";
        }
        //Withdraws funds from users, adds them to internal bank account
        public string Withdraw(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;

                    try
                    {
                        Console.WriteLine("Select user account you wish to withdraw from");
                        string suser = Console.ReadLine();
                        string BaseUser = currentUser;
                        if (currentUser == "admin" && suser == "admin")
                        {
                            Console.WriteLine("Invalid operation.Administrators cannot withdraw Internal Bank Account.");
                            Console.ReadKey();
                            continue;
                        }

                        else
                        {
                            Console.Write("Select the amount you wish to deposit:\n>");
                            decimal depositedAmount = decimal.Parse(Console.ReadLine());
                            var userToWithdrawFrom = btx.Users.SingleOrDefault(r => r.username == suser);
                            var accountToWithdrawFrom = btx.Accounts.SingleOrDefault(i => i.id == userToWithdrawFrom.id);
                            var Admin = btx.Users.SingleOrDefault(x => x.username == BaseUser);
                            var AdminAccount = btx.Accounts.SingleOrDefault(y => y.id == Admin.id);
                            btx.Accounts.Update(accountToWithdrawFrom);
                            btx.Accounts.Update(AdminAccount);
                            accountToWithdrawFrom.Amount -= depositedAmount;
                            AdminAccount.Amount += depositedAmount;
                            btx.SaveChanges();
                            break;
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("This is not a valid entry.Please try again.");
                        Console.ReadKey();
                    }
                }
            }
            return "Statement";
        }

        public void SendStatement(string user)
        {
            Console.WriteLine("Send admin statement");
        }
        public void ExitApp()
        {
            Console.WriteLine("Bye!");
        }
    }
}
