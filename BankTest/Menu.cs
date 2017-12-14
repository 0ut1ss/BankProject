using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Menu
    {
        public void DisplayMenu(User user)
        {

            int menuInput = 0;


            while (menuInput != 6)
            {

                try
                {
                    Console.WriteLine($"WELCOME {user.username.ToUpper()}\n");
                    if (user.username == "admin")
                    {
                        Console.Write("1.View Internal Bank Account\n\n2.View Member Bank Account" +
                                "\n\n3.Deposit to Member Account\n\n4.Withdraw from Member Account\n\n5.Store today's Transactions\n\n6.Exit\n>");
                        menuInput = int.Parse(Console.ReadLine());
                    }

                    else
                    {
                        Console.Write("1.View Account\n\n2.Deposit to Internal Bank Account" +
                                "\n\n3.Deposit to other Account\n\n4.Store today's Transactions\n\n5.Exit\n>");
                        menuInput = int.Parse(Console.ReadLine());

                    }
                    Console.Clear();

                    switch (menuInput)
                    {
                        case 1:
                            {
                                //ViewOtherAccount is false, Viebalance for normal users
                                user.ViewBalance(user.username, false);
                            }
                            break;
                        case 2:

                            //If user is Admin ViewOtherAccount is true, displays options for admin
                            if (user.username == "admin")
                            {
                                user.ViewBalance(user.username, true);
                            }

                            //Set to false means users deposit to the internal bank account
                            else
                            {
                                user.Deposit(user.username, false);

                            }
                            break;

                            //Set to true means current user chooses account to deposit
                        case 3:
                            user.Deposit(user.username, true);
                            break;



                        case 4:
                            //If user is admin, withdraw function is called
                            if (user.username == "admin")
                            {
                                user.Withdraw(user.username);
                            }

                            //if normal user, statement file is written
                            else
                            {
                                FileAccess.WriteToFile(user.username);

                            }
                            break;

                        case 5:

                            //if admin, statement file is written
                            if (user.username == "admin")
                            {
                                FileAccess.WriteToFile(user.username);
                            }

                            else
                            {
                                //if not admin menuInput is set to 6, so application terminates automatically
                                user.ExitApp();
                                menuInput = 6;
                            }
                            break;

                        case 6:
                            if (user.username == "admin")
                            {
                                user.ExitApp();
                                
                            }

                            else
                            {
                                //If not admin display error message and set menu item to an invalid number
                                Console.WriteLine("Invalid input, please try again!");
                                menuInput = 100;
                            }
                            break;
                        default:
                            {
                                Console.WriteLine("Invalid input, please try again!");
                            }
                            break;
                    }
                    Console.ReadKey();
                    Console.Clear();

                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
