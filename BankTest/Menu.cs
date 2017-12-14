using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Menu
    {
        private string _userCredentials;
        public void DisplayMenu()
        {
            LoginScreen Lg = new LoginScreen();
            _userCredentials = Lg.Login();
            User User = new User();
            int menuInput = 0;


            while (menuInput != 6)
            {

                try
                {
                    Console.WriteLine($"WELCOME {_userCredentials.ToUpper()}\n");
                    if (_userCredentials == "admin")
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
                                User.ViewBalance(_userCredentials, false);
                            }
                            break;
                        case 2:

                            //If user is Admin ViewOtherAccount is true, displays options for admin
                            if (_userCredentials == "admin")
                            {
                                User.ViewBalance(_userCredentials, true);
                            }

                            //Set to false means users deposit to the internal bank account
                            else
                            {
                                User.Deposit(_userCredentials, false);

                            }
                            break;

                            //Set to true means current user chooses account to deposit
                        case 3:
                            User.Deposit(_userCredentials, true);
                            break;



                        case 4:
                            //If user is admin, withdraw function is called
                            if (_userCredentials == "admin")
                            {
                                User.Withdraw(_userCredentials);
                            }

                            //if normal user, statement file is written
                            else
                            {
                                FileAccess.WriteToFile(_userCredentials);

                            }
                            break;

                        case 5:

                            //if admin, statement file is written
                            if (_userCredentials == "admin")
                            {
                                FileAccess.WriteToFile(_userCredentials);
                            }

                            else
                            {
                                //if not admin menuInput is set to 6, so application terminates automatically
                                User.ExitApp();
                                menuInput = 6;
                            }
                            break;

                        case 6:
                            if (_userCredentials == "admin")
                            {
                                User.ExitApp();
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
