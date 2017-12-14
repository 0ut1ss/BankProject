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
                    Console.WriteLine("PLEASE TYPE YOUR CREDENTIALS\n");
                    Console.Write("enter your user name:\n>");
                    string userName = Console.ReadLine();


                    Console.Write("enter your password:\n>");
                    string pWd = Console.ReadLine();

                    //Check to see if user exists in the database
                    bool IsUser = btx.Users.Any(r => r.username == userName && r.password == pWd);
                    Console.Clear();


                    
                    if (IsUser)
                    {
                        return userName;
                        
                    }

                    else
                    {
                        Console.WriteLine("Invalid username or password");
                        Console.ReadLine();
                        Console.Clear();
                    }

                }
            }
               
            
        }
    }
}
