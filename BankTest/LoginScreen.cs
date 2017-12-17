using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    class LoginScreen
    {
        public void Login()
        {
            using (BankContext btx = new BankContext())
            {
                int counter = 3;
                while (counter != 0)
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
                        var user = new User() {username = userName, password = pWd };
                        Menu menu = new Menu();
                        menu.DisplayMenu(user);
                        break;
                        
                    }

                    else
                    {
                        Console.WriteLine("Invalid username or password");
                        counter -=1;
                        Console.WriteLine($"Attempts remaining {counter}");
                        Console.ReadLine();
                        Console.Clear();
                    }

                }
                if (counter==0)
                {
                    Console.WriteLine("Due to three consecutive failed attempts account is temporarily locked\nPlease contact office support");
                    Console.ReadKey();
                }
                
            }
               
            
        }
    }
}
