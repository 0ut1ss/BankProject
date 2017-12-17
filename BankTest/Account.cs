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

        //Initialize transaction_date
        public Account()
        {
            transaction_date = new DateTime();
        }

        //Print account amount to console with the correct format
        public void DisplayAmount(Account account)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(account.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR")));
        }

        //Format amount to display correctly
        public static string FormatAmount(decimal amount)
        {
            Console.OutputEncoding = Encoding.UTF8;
            return amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"));
        }



        public static void UpdateAccounts(Account primaryAccount, Account secondaryAccount, BankContext btc)
        {
            btc.Accounts.Update(primaryAccount);
            btc.Accounts.Update(secondaryAccount);
        }


        public static void UpdateTime(Account primaryAccount, Account secondaryAccount)
        {
            primaryAccount.transaction_date = DateTime.Now;
            secondaryAccount.transaction_date = DateTime.Now;
        }

        //Update users Bank account amounts, using boolean to determine if the method is deposit or withdraw
        public static void UpdateAmount(Account primaryAccount, Account secondaryAccount, decimal amount, bool isDeposit)
        {
            if (isDeposit)
            {
                secondaryAccount.Amount += amount;
                primaryAccount.Amount -= amount;
            }

            else
            {
                secondaryAccount.Amount -= amount;
                primaryAccount.Amount += amount;
            }
        }


    }

    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        //Overriding the ToString Method
        public override string ToString()
        {
            using (BankContext btx = new BankContext())
            {
                var account = btx.Accounts.SingleOrDefault(i => i.id == id);
                return ToString(account);
            }

        }
        //Helper method for the override
        public String ToString(Account account)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");
            return $"{dt}\n{username.ToUpper()}'s Account Balance is: {Account.FormatAmount(account.Amount)}";
        }

        //Initialize user
        public User CreateUser(BankContext btc, string xuser)
        {
            var user = new User();
            return user = btc.Users.SingleOrDefault(r => r.username == xuser);

        }


        //Initialize Account
        public Account CreateAccount(BankContext btc, string user)
        {
            var Account = new Account();
            var User = CreateUser(btc, user);
            return Account = btc.Accounts.SingleOrDefault(i => i.id == User.id);

        }

        //Create string for transaction report
        public static string GenerateReport(User activeUser, decimal transactionAmount, User secondUser, bool IsDeposit, bool IsInternal)
        {
            //If conditions match send statement for deposit to other accounts
            if (IsDeposit && !IsInternal)
            {
                return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF")}\n" +
                $"{activeUser.username.ToUpper()} deposited {Account.FormatAmount(transactionAmount)}" +
                 $" to {secondUser.username.ToUpper()}";
            }

            //if conditions match send statetement for deposit to Internal
            else if(!IsDeposit && IsInternal)
            {
                return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF")}\n" +
                $"{activeUser.username.ToUpper()} deposited {Account.FormatAmount(transactionAmount)}" +
                $" to the Internal Bank Account";
            }

            // Send deposit for withdraw
            else
            {
                return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF")}\n" +
                $"{activeUser.username.ToUpper()} withdrew {Account.FormatAmount(transactionAmount)}" +
                $" from {secondUser.username.ToUpper()}";
            }

        }



        public void ViewBalance(string currentUser, bool IsViewOtherAccount)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    //If true displays the options for the admin
                    if (IsViewOtherAccount)
                    {
                        Console.Clear();
                        Console.Write("Select user account you wish to access:\n>");
                        string suser = Console.ReadLine();
                        try
                        {
                            //If the active user is admin and the selected user is admin, user is prompted to use another option
                            if (suser == "admin")
                            {
                                Console.WriteLine("To view the Internal Bank Account Balance, please select option number 1");

                            }
                            //View Balance of other users as admin
                            else
                            {
                                var user = CreateUser(btx, suser);
                                var account = CreateAccount(btx, suser);
                                Console.Clear();
                                Console.WriteLine(user.ToString());
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("The user you typed does not exist.\nPlease try again.");
                            Console.ReadKey();
                        }
                    }
                    //false displays the accounts for normal users
                    else
                    {
                        var user = CreateUser(btx, currentUser);
                        var account = CreateAccount(btx, currentUser);
                        Console.Clear();
                        Console.WriteLine(user.ToString());
                        break;
                    }

                }

            }
        }


        public void Deposit(string currentUser, bool depositToOther)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    //True equals to  despositing to other accounts and not to internal
                    if (depositToOther)
                    {
                        Console.WriteLine("Select user account you wish to deposit to");
                        string suser = Console.ReadLine();

                        //Checks to determine if admin tries to deposit to the internal
                        if (currentUser == "admin" && suser == "admin")
                        {
                            Console.WriteLine("Invalid operation.Administrators cannot deposit to Internal Bank Account.");
                            Console.ReadKey();
                            continue;
                        }

                        //Checks to determine if user tries to deposit to themselves
                        else if (currentUser != "admin" && currentUser == suser && suser != "admin")
                        {
                            Console.WriteLine("Invalid operation.Only deposit to other members allowed");
                            Console.ReadKey();
                            continue;
                        }

                        //Checks to determine if user tries to deposit to internal
                        else if (currentUser != "admin" && suser == "admin")
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

                                        //Checks for negative amount entries and if the active user has sufficient money to make the deposit
                                        if (depositedAmount > 0 && ActiveAccount.Amount >= depositedAmount)
                                        {
                                            Account.UpdateAccounts(ActiveAccount, accountToDeposit, btx);
                                            Account.UpdateAmount(ActiveAccount, accountToDeposit, depositedAmount, true);
                                            Account.UpdateTime(ActiveAccount, accountToDeposit);

                                            Console.Clear();
                                            Console.WriteLine($"Successfully deposited {Account.FormatAmount(depositedAmount)}");

                                            //Adds transaction to buffer
                                            FileAccess.AddToBuffer(User.GenerateReport(ActiveUser,depositedAmount,userToDeposit,true, false));
                                            btx.SaveChanges();
                                            break;
                                        }

                                        else
                                        {
                                            Console.WriteLine("Invalid entry, please retype the amount");
                                            Console.ReadKey();
                                            continue;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("This is not a valid entry.Please try again.");
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
                    //Deposit to Internal
                    else
                    {
                        try
                        {
                            var Internalaccount = CreateAccount(btx, "admin");
                            var InternalUser = CreateUser(btx, "admin");
                            var user = CreateUser(btx, currentUser);
                            var CurrentUserAccount = CreateAccount(btx, currentUser);

                            Console.Write("Select the amount you wish to deposit:\n>");

                            decimal depositedAmount = decimal.Parse(Console.ReadLine());

                            //Checks for negative amount entries and if the active user has sufficient money to make the deposit
                            if (depositedAmount > 0 && CurrentUserAccount.Amount >= depositedAmount)
                            {
                                Account.UpdateAccounts(CurrentUserAccount, Internalaccount, btx);
                                Account.UpdateAmount(CurrentUserAccount, Internalaccount, depositedAmount, true);
                                Account.UpdateTime(CurrentUserAccount, Internalaccount);

                                Console.Clear();
                                Console.WriteLine($"Successfully deposited {Account.FormatAmount(depositedAmount)}");

                                //Adds transaction to buffer
                                FileAccess.AddToBuffer(GenerateReport(user, depositedAmount, InternalUser, false, true));
                                btx.SaveChanges();
                                break;
                            }

                            else
                            {
                                Console.WriteLine("Invalid entry, please retype the amount");
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

        }
        //Admin withdraws funds from users, adds them to internal bank account
        public void Withdraw(string currentUser)
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Clear();

                    Console.WriteLine("Select user account you wish to withdraw from");
                    string suser = Console.ReadLine();

                    //Checks to see if admin tries to withdraw from the internal bank account
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

                                    //Checks for negative amount entries and if the normal user has more money than the requested amount
                                    if (withdrawnAmount > 0 && accountToWithdrawFrom.Amount >= withdrawnAmount)
                                    {
                                        Account.UpdateAccounts(ActiveAccount, accountToWithdrawFrom, btx);
                                        Account.UpdateAmount(ActiveAccount, accountToWithdrawFrom, withdrawnAmount, false);
                                        Account.UpdateTime(ActiveAccount, accountToWithdrawFrom);

                                        Console.Clear();
                                        Console.WriteLine($"Successfully withdrawn {Account.FormatAmount(withdrawnAmount)}");

                                        //Add transaction to buffer
                                        FileAccess.AddToBuffer(GenerateReport(ActiveUser,withdrawnAmount,userToWithdrawFrom,false,false));
                                        btx.SaveChanges();
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("Invalid amount, please try again");
                                        Console.ReadKey();
                                        continue;
                                    }

                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("This is not a valid entry.Please try again.");
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
            Console.WriteLine("Have a nice Day.");

        }
    }
}
