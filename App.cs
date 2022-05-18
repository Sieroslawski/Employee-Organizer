using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class App
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.createMainMenu();
            Console.ReadLine();          
        }

        public void exceptionMenu()
        {
            Console.WriteLine("Invalid entry.");
            Console.Write("\nPress ENTER to continue.");
            Console.ReadLine();
            Console.Clear();
        }

        public void organizationSubMenu()
        {            
            Console.Write("\nPress ENTER to continue.");
            Console.ReadLine();
            Console.Clear();
        }        
    }
}
