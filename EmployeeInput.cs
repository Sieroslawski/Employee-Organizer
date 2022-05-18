using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class EmployeeInput
    {
        public void createNewEmployee()
        {
            //Create the employee's first and last names
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            Employee newEmployee = new Employee();

            Console.Clear();
            Console.WriteLine("\nEMPLOYEE DATA ENTRY\n");

            Console.Write("Enter the first name: ");
            newEmployee.firstName = Console.ReadLine();

            Console.Write("Enter the last name: ");
            newEmployee.lastName = Console.ReadLine();

            if (newEmployee.firstName.Length < 2 || newEmployee.lastName.Length < 2)
            {
                Console.Write("\nPlease enter a first and last name with at least two characters.");
                Console.Write("\n\nPress ENTER to continue");
                Console.ReadLine();
                createNewEmployee();
            }

            Console.Clear();
            insertIntoOrganization(newEmployee);
        }

        public void insertIntoOrganization(Employee newEmployee)
        {
            //Assign the employee an organization and then a department and an ID
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            App app = new App();
            IList<Department> department = db.Departments.ToList();

            for (int i = 0; i < department.Count(); i++)
            {
                int questionNum = i + 1;
                Console.WriteLine(questionNum + ") " + department[i].organizationName);
            }

            Console.WriteLine("\nORGANIZATION ENTRY");
            Console.WriteLine("\nPlease enter an organization for: " + newEmployee.firstName + " " + newEmployee.lastName);
            Console.Write("\nPlease enter your selection: ");
            
            try
            {
                int userOrgChoice = Convert.ToInt32(Console.ReadLine()) - 1;
                string orgChoice = department[userOrgChoice].organizationName;

                IList<Department> query = db.Departments
                             .Where(d => d.organizationName.Contains(orgChoice)).ToList();

                Console.Clear();

                Console.WriteLine("\nDEPARTMENT ENTRY");
                int questionNum = 1;
                foreach (var item in query)
                {

                    Console.WriteLine(questionNum + ") " + item.departmentName + "\n");
                    questionNum++;
                }

                Console.WriteLine("Please enter a department for " + newEmployee.firstName + " " + newEmployee.lastName);
                Console.Write("\nPlease enter your selection: ");
                Console.ReadLine();

                int depChoice = department[userOrgChoice].departmentID;
                newEmployee.departmentID = depChoice;

                db.Employees.Add(newEmployee);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                app.exceptionMenu();
                insertIntoOrganization(newEmployee);                
            }

            Console.Clear();
            insertCertificate(newEmployee);
        }

        public void insertCertificate(Employee newEmployee)
        {
            //Assign the employee a certificate
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            App app = new App();

            IList<Certification> certificate = db.Certifications.ToList();
            IList<Department> query = db.Departments.ToList();
            List<string> certList = new List<string>();

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\nCERTIFICATION ENTRY");
                    
                    for (int i = 0; i < certificate.Count(); i++)
                    {
                        int questionNum = i + 1;

                        Console.WriteLine(questionNum + ") " + certificate[i].certificate);

                        if (i == certificate.Count() - 1)
                        {
                            Console.WriteLine(questionNum + 1 + ") Done.");
                        }
                    }
                    if (certList.Count() != 0)
                    {
                        Console.WriteLine("\nTotal certificates: " + certList.Count());

                    }

                    Console.WriteLine("\nPlease enter a selection for " + newEmployee.firstName + " " + newEmployee.lastName + " ");
                    Console.Write("Please enter your selection: ");

                    string userCertChoice = Console.ReadLine();
                    int certChoice = Convert.ToInt32(userCertChoice) - 1;

                    if (certChoice + 1 > certificate.Count)
                    {
                        employeeConfirmation(newEmployee, certList);
                        break;
                    }
                    string employeeCertificate = certificate[certChoice].certificate;


                    Certification empCert = (from c in db.Certifications
                                             where c.certificate == employeeCertificate
                                             select c).FirstOrDefault();

                    Employee empID = (from e in db.Employees
                                      where e.departmentID == newEmployee.departmentID
                                      select e).FirstOrDefault();

                    empID.Certifications.Add(empCert);
                    certList.Add(empCert.certificate);
                }

            }
            catch (Exception ex)
            {
                app.exceptionMenu();
                insertCertificate(newEmployee);
            }
        }

        public void employeeConfirmation(Employee newEmployee, List<String> certList)
        {
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            Display d = new Display();
            Menu menu = new Menu();
            const int CANCEL_ENTRY = 2;

            var locationQuery = from o in db.Organizations
                                from r in db.Regions
                                where o.city == r.city
                                select new
                                {
                                    organizationName = o.organizationName,
                                    state = o.state,
                                    city = o.city,
                                    timeZone = r.timezone
                                };

            Console.Clear();
            string organizationName = newEmployee.Department.organizationName;
            Console.WriteLine("\nEMPLOYEE CREATION CONFIRMATION\n");
            Console.WriteLine("Please verify the employee details: ");
            Console.WriteLine("\n===================================================");
            foreach (var location in locationQuery)
            {
                if (organizationName == location.organizationName)
                {
                    Console.WriteLine("Organization: " + location.organizationName +
                        "\n" + "State: ".PadRight(14) + location.state +
                        "\n" + "City: ".PadRight(14) + location.city +
                        "\n" + "Timezone: ".PadRight(14) + location.timeZone);
                }
            }
            Console.WriteLine("===================================================\n");
            Console.WriteLine("   Employee Name:    " + newEmployee.firstName + " " + newEmployee.lastName);
            Console.WriteLine("\n   Department:       " + newEmployee.Department.departmentName);
            bool certified = false;

            foreach (var cert in certList.Distinct())
            {
                if (cert.Length > 0)
                {
                    if (certified == false)
                    {
                        Console.WriteLine("\n   Certifications:   -" + cert);
                        certified = true;
                    }
                    else if (certified == true)
                    {
                        Console.WriteLine("                     -" + cert);
                    }
                }
            }
            Console.WriteLine("1) Save entry.");
            Console.WriteLine("2) Cancel entry.");

            Console.Write("\nPlease enter your selection: ");
            string saveChoice = Console.ReadLine();
            int userSaveChoice = Convert.ToInt32(saveChoice);
            if (userSaveChoice == 1)
            {
                db.SaveChanges();
                Console.WriteLine("Entry saved");
                Console.Write("\nPress ENTER to continue");
                Console.ReadLine();
                menu.createMainMenu();
            }
            else if (userSaveChoice == CANCEL_ENTRY)
            {
                Console.WriteLine("Entry not saved");
                Console.Write("\nPress ENTER to continue back to main menu");
                Console.ReadLine();
                menu.createMainMenu();
            }
            else if (userSaveChoice < 1 || userSaveChoice > CANCEL_ENTRY)
            {
                Console.WriteLine("Invalid entry");
                Console.Write("\nPress ENTER to continue back to main menu");
                Console.ReadLine();
                menu.createMainMenu();
            }
        }
    }
}
