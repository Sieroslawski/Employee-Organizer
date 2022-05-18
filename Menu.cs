using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Menu
    {
        public string optionInput;
        Display d = new Display();
        EmployeeInput e = new EmployeeInput();
        public void createMainMenu()
        {   
            //Create the visual menu
            Console.Clear();
            Console.WriteLine("\nMAIN MENU\n");
            Console.WriteLine("1. All Organizations");
            Console.WriteLine("2. All Employees");
            Console.WriteLine("3. Organization Detail");
            Console.WriteLine("4. Create New Employee");
            Console.WriteLine("5. Quit\n");
            Console.Write("Please enter your selection: ");
            enterTheMenus();
        }

        public void enterTheMenus()
        {
            //Enter the menu
            optionInput = Console.ReadLine();
            switch (optionInput)
            {
                case "1":
                    d.getAllOrganizations();
                    break;
                case "2":
                    d.getAllEmployees();
                    break;
                case "3":
                    d.getOrganizationDetail();
                    break;
                case "4":
                    e.createNewEmployee();
                    break;
                case "5":
                    Console.WriteLine("Exit Menu");
                    break;

                default:
                    Console.WriteLine("Invalid entry. Please try again.\n\n");
                    Console.Write("Press RETURN to continue.");
                    optionInput = Console.ReadLine();
                    Console.Clear();
                    createMainMenu();
                    break;
            }
        }
    }
}