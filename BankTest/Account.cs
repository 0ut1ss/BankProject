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

        public void DisplayAmount(Account account)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(account.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR")));
        }

        public static string FormatAmount(decimal amount)
        {
            Console.OutputEncoding = Encoding.UTF8;
            return amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"));
        }

    }

    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public User CreateUser(BankContext btc, string xuser)
        {
            var user = new User();
           return  user = btc.Users.SingleOrDefault(r => r.username == xuser);
            
        }

        public Account CreateAccount(BankContext btc, string user)
        {
            var Account = new Account();
            var User = CreateUser(btc, user);
            return Account = btc.Accounts.SingleOrDefault(i => i.id == User.id);

        }

        public void ViewBalance(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                var user = CreateUser(btx, currentUser);
                var account = CreateAccount(btx, currentUser);
                account.DisplayAmount(account);
            }
        }
        //View normal account balance as Admin
        public void ViewBalance()
        {
            using (BankContext btx = new BankContext())
            {
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
                            var user = CreateUser(btx, suser);
                            var account = CreateAccount(btx, suser);
                            account.DisplayAmount(account);
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
        public void DepositToInternal(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;
                    try
                    {
                        Console.Write("Select the amount you wish to deposit:\n>");
                        decimal depositedAmount = decimal.Parse(Console.ReadLine());
                        if (depositedAmount >= 0)
                        {
                            var Internalaccount = CreateAccount(btx, "admin");
                            var user = CreateUser(btx, currentUser);
                            var CurrentUserAccount = CreateAccount(btx, currentUser);
                            btx.Accounts.Update(Internalaccount);
                            btx.Accounts.Update(CurrentUserAccount);
                            Internalaccount.Amount += depositedAmount;
                            CurrentUserAccount.Amount -= depositedAmount;
                            Console.WriteLine($"Successfully deposited {Account.FormatAmount(depositedAmount)}");
                            //Add action to Buffer List
                            FileAccess.AddToBuffer($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} " +
                                $"{currentUser.ToUpper()} deposited {Account.FormatAmount(depositedAmount)}" +
                                $" to the Internal Bank Account");
                            btx.SaveChanges();
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Cannot deposit negative amounts");
                            Console.ReadKey();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("This is not a valid entry.Please try again.");
                        Console.ReadKey();
                    }

                }

            }
        }
        //Deposit to other accounts
        public void Deposit(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.OutputEncoding = Encoding.UTF8;

                    Console.WriteLine("Select user account you wish to deposit to");
                    string suser = Console.ReadLine();
                    if (currentUser == "admin" && suser == "admin")
                    {
                        Console.WriteLine("Invalid operation.Administrators cannot deposit to Internal Bank Account.");
                        Console.ReadKey();
                        continue;
                    }
                    else if (currentUser != "admin" && currentUser == suser && suser != "admin")//Check if user tries to deposit to themselves
                    {
                        Console.WriteLine("Invalid operation.Only deposit to other members allowed");
                        Console.ReadKey();
                        continue;
                    }

                    else if (currentUser != "admin" && suser == "admin")//Check if user tries to deposit to Admin
                    {
                        Console.WriteLine("Invalid operation.Please use the Deposit to Internal Bank Account option.");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        try
                        {
                            var userToDeposit = CreateUser(btx, suser);
                            var accountToDeposit = CreateAccount(btx, suser);
                            var ActiveUser = CreateUser(btx, currentUser);
                            var ActiveAccount = CreateAccount(btx, currentUser);
                            do
                            {
                                Console.Clear();
                                try
                                {
                                    Console.Write("Select the amount you wish to deposit:\n>");
                                    decimal depositedAmount = decimal.Parse(Console.ReadLine());
                                    if (depositedAmount >= 0)
                                    {
                                        btx.Accounts.Update(accountToDeposit);
                                        btx.Accounts.Update(ActiveAccount);
                                        accountToDeposit.Amount += depositedAmount;
                                        ActiveAccount.Amount -= depositedAmount;
                                        Console.WriteLine($"Successfully deposited {Account.FormatAmount(depositedAmount)}");
                                        FileAccess.AddToBuffer($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} " +
                                        $"{ActiveUser.username.ToUpper()} deposited {Account.FormatAmount(depositedAmount)}" +
                                        $" to {userToDeposit.username.ToUpper()}");
                                        btx.SaveChanges();
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("Cannot deposit negative amounts");
                                        Console.ReadKey();
                                        continue;
                                    }

                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid entry, please retype the amount");
                                    Console.ReadKey();
                                }
                            } while (true);

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
            
        }
        //Withdraws funds from users, adds them to internal bank account
        public void Withdraw(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();

                    Console.WriteLine("Select user account you wish to withdraw from");
                    string suser = Console.ReadLine();
                    if (currentUser == "admin" && suser == "admin")
                    {
                        Console.WriteLine("Invalid operation.Cannot withdraw from Internal Bank Account.");
                        Console.ReadKey();
                        continue;
                    }

                    else
                    {
                        try
                        {
                            var userToWithdrawFrom = CreateUser(btx, suser);
                            var accountToWithdrawFrom = CreateAccount(btx, suser);
                            var ActiveUser = CreateUser(btx, currentUser);
                            var ActiveAccount = CreateAccount(btx, currentUser);
                            do
                            {
                                Console.Clear();
                                try
                                {
                                    Console.Write("Select the amount you wish to withdraw:\n>");
                                    decimal withdrawnAmount = decimal.Parse(Console.ReadLine());
                                    if (withdrawnAmount >= 0)
                                    {
                                        btx.Accounts.Update(accountToWithdrawFrom);
                                        btx.Accounts.Update(ActiveAccount);
                                        accountToWithdrawFrom.Amount -= withdrawnAmount;
                                        ActiveAccount.Amount += withdrawnAmount;
                                        Console.WriteLine($"Successfully withdrawn {Account.FormatAmount(withdrawnAmount)}");
                                        FileAccess.AddToBuffer($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} " +
                                        $"{ActiveUser.username.ToUpper()} withdrew {Account.FormatAmount(withdrawnAmount)}" +
                                        $" from {userToWithdrawFrom.username.ToUpper()}");
                                        btx.SaveChanges();
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("Cannot withdraw negative amounts");
                                        Console.ReadKey();
                                        continue;
                                    }

                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid entry, please retype the amount");
                                    Console.ReadKey();
                                }
                            } while (true);

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
            
        }

        public void ExitApp()
        {
            Console.WriteLine("Have a nice Day");
        }
    }
}
