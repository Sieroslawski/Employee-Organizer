using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    class Display
    {
        public void getAllOrganizations()
        {
            //Display all organizations
            App app = new App();
            Menu menu = new Menu();

            Console.Clear();
            Console.WriteLine("\nALL ORGANIZATIONS\n");
            EmployeeDetailEntities allOrganizations = new EmployeeDetailEntities();

            var organizationQuery = from o in allOrganizations.Organizations
                                    select new { o.organizationName, o.city, o.state };

            foreach (var detail in organizationQuery)
            {
                Console.WriteLine((detail.organizationName).PadLeft(9) + (":").PadRight(4) + detail.city + ", " + detail.state);
            }

            app.exceptionMenu();
            menu.createMainMenu();
        }

        public void getAllEmployees()
        {
            //Display all employees
            Menu menu = new Menu();
            App app = new App();

            Console.Clear();
            Console.WriteLine("\nALL EMPLOYEES");
            EmployeeDetailEntities allEmployees = new EmployeeDetailEntities();

            var organizationQuery = (from e in allEmployees.Employees
                                     from d in allEmployees.Departments
                                     where e.departmentID == d.departmentID
                                     select new
                                     { organizationName = d.organizationName }).Distinct();

            var employeeQuery = from e in allEmployees.Employees
                                from d in allEmployees.Departments
                                where e.departmentID == d.departmentID
                                orderby d.organizationName, e.lastName
                                select new
                                {
                                    d.organizationName,
                                    e.lastName,
                                    e.firstName
                                };

            foreach (var orgGroup in organizationQuery)
            {
                Console.WriteLine("\n" + orgGroup.organizationName);
                foreach (var empGroup in employeeQuery)
                {
                    if (empGroup.organizationName == orgGroup.organizationName)
                    {
                        Console.WriteLine("".PadLeft(5) + empGroup.firstName + " " + empGroup.lastName);
                    }
                }
            }
            app.exceptionMenu();
            menu.createMainMenu();
        }

        public void getOrganizationDetail()
        {
            //Display the details of each organization
            Menu menu = new Menu();
            App app = new App();

            Console.Clear();
            Console.WriteLine("\nORGANIZATION SELECTION\n");
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            IList<Organization> orgs = db.Organizations.ToList();
            IList<Employee> empI = db.Employees.ToList();

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

            for (int i = 0; i < orgs.Count(); i++)
            {
                int questionNum = i + 1;
                Console.WriteLine(questionNum + ") " + orgs[i].organizationName);
            }

            Console.Write("\nPlease enter your selection: ");
            string userOrgChoice = Console.ReadLine();
            try
            {
                int choiceNum = Convert.ToInt32(userOrgChoice) - 1;
                Console.Clear();
                
                Console.WriteLine("\n===================================================");
                foreach (var location in locationQuery)
                {
                    if (orgs[choiceNum].organizationName == location.organizationName)
                    {
                        Console.WriteLine("Organization: " + location.organizationName +
                            "\n" + "State: ".PadRight(14) + location.state +
                            "\n" + "City: ".PadRight(14) + location.city +
                            "\n" + "Timezone: ".PadRight(14) + location.timeZone);
                    }
                }
                Console.WriteLine("===================================================\n");

                foreach (var location in locationQuery)
                {
                    if (orgs[choiceNum].organizationName.Contains(""))
                    {
                        getEmployees(orgs[choiceNum].organizationName);
                        app.organizationSubMenu();
                        menu.createMainMenu();
                    }
                    else
                    {
                        Console.WriteLine("   No employees entries exist for this organization.");
                        app.organizationSubMenu();
                        menu.createMainMenu();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                app.exceptionMenu();
                Console.Clear();
                menu.createMainMenu();
            }

            Console.ReadLine();
        }

        public void getEmployees(string organizationName)
        {
            //Display all employees for each organization
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            Menu menu = new Menu();

            var employeeQuery = from e in db.Employees
                                from d in db.Departments
                                .Where(emp => emp.departmentID == e.departmentID)
                                .Where(emp => emp.organizationName == organizationName)
                                orderby e.lastName
                                select new
                                {
                                    firstName = e.firstName,
                                    lastName = e.lastName,
                                    employeeID = e.employeeID,
                                    organizationName = d.organizationName,
                                    department = d.departmentName
                                };

            if (!employeeQuery.Any())
            {
                Console.WriteLine("   No employee entries exist for this organization");

            }
            else
            {
                foreach (var employee in employeeQuery)
                {
                    Console.WriteLine("\n    Employee Name: " + employee.lastName + ", " + employee.firstName);
                    Console.WriteLine("    Department:    " + employee.department);
                    getCertification(employee.employeeID);
                }
            }
        }

        public void getCertification(int employeeID)
        {
            //Display all tickets for each employee
            EmployeeDetailEntities db = new EmployeeDetailEntities();
            App app = new App();

            var certificationQuery = from e in db.Employees
                                     from c in e.Certifications
                                     where e.employeeID == employeeID
                                     select new
                                     {
                                         employeeID = e.employeeID,
                                         certification = c.certificate
                                     };

            bool certified = false;
            foreach (var cert in certificationQuery)
            {
                if (cert.certification.Length > 0)
                {
                    if (certified == false)
                    {
                        Console.WriteLine("    Certification(s): ");
                        Console.WriteLine("                   -" + cert.certification);
                        certified = true;
                    }
                    else if (certified == true)
                    {
                        Console.WriteLine("                   -" + cert.certification);
                    }
                }
            }
        }
    }
}
