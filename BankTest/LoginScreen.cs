using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    class LoginScreen
    {
        public string Login()
        {
            using (BankContext btx = new BankContext())
            {
                while (true)
                {
                    Console.Write("Please enter your user name:\n>");
                    string userName = Console.ReadLine();


                    Console.Write("Please enter your password:\n>");
                    string pWd = Console.ReadLine();
                    bool IsUser = btx.Users.Any(r => r.username == userName && r.password == pWd);
                    Console.Clear();

                    if (IsUser && userName == "admin")
                    {
                        return userName;
                        break;
                    }

                    else if (IsUser)
                    {
                        return userName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password");
                        Console.ReadLine();
                        Console.Clear();
                    }

                }
            }
               
            return "";
        }
    }
}
