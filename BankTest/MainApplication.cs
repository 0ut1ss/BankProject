using System;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Bank
{
    class MainApplication
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, is it me you're looking for?");
            Menu m = new Menu();
            m.DisplayMenu();

            //using (BankTestContext btx = new BankTestContext())
            //{
            //    string suser = "user1";
            //    var user = btx.Users.SingleOrDefault(r => r.username == suser);
            //    var account = btx.Accounts.SingleOrDefault(i => i.id == user.id);
            //    Console.WriteLine(account.Amount);
            //    Console.ReadKey();
            //}
        }
    }
}
