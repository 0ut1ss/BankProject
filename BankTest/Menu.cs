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
                                User.ViewBalance(_userCredentials, false);//All users access their own account
                            }
                            break;
                        case 2:
                            if (_userCredentials == "admin")
                            {
                                User.ViewBalance(_userCredentials, true);//Admin accessses other accounts
                            }

                            else
                            {
                                User.DepositToInternal(_userCredentials);

                            }
                            break;

                        case 3:
                            User.Deposit(_userCredentials);
                            break;

                        case 4:
                            if (_userCredentials == "admin")
                            {
                                User.Withdraw(_userCredentials);
                            }

                            else
                            {
                                FileAccess.WriteToFile(_userCredentials);

                            }
                            break;

                        case 5:
                            if (_userCredentials == "admin")
                            {
                                FileAccess.WriteToFile(_userCredentials);
                            }

                            else
                            {
                                User.ExitApp();
                                menuInput = 6;
                            }
                            break;

                        case 6:
                            if (_userCredentials == "admin")
                            {
                                User.ExitApp();
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
