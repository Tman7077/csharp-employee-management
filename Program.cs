class Program
{
    static void Main(string[] args)
    {
        Prompter prompter = new Prompter();              // class that handles the asking of questions and checking for validity
        List<Employee> employees = new List<Employee>(); // a list to which employees can be added
        Employee? employee = null;                       // mutable employee variable to be used in various locations
        int choice = -1; // the number the user inputs, should be 1-8
        int choices = 0; // the number of times the user has selected an option

        // display the menu to the user
        // this loops until the user quits
        do 
        {
            // display the menu
            Menu();
            // increment choices for grammatical changes upon retrial
            choices++;
        } while (choice != 0);

        // display the main menu options to the user and act on them
        void Menu()
        {
            int nthTime = 0; // number of times the user has input any value (valid or not) this time in the menu
                             // this is used instead of choices because of the use of Prompter, it will display
                             // a custom message if the user inputs something invalid during that instance of menuing

            // display the menu to the user, using Prompter to check for validity
            // this loops until a valid input is received
            do
            {
                // set response to the user input, displaying a custom message upon invalid input
                string response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Menu, false, choices.ToString()) : prompter.Ask(Prompter.AllowedWhatValues.Menu, true, choices.ToString());
                // sets choice equal to the user input (as an int) if it is able to parse it
                int.TryParse(response, out choice);
                // increments nthTime in order to display the error message if necessary
                nthTime++;
            } while (choice == -1);

            // decide which menu item to act upon
            switch (choice)
            {
                case 1: // Load an employee database
                    bool overwrite = true; // whether the currently loaded employee information should be overwritten

                    // if there is employee data loaded
                    if (employees.Count > 0)
                    {
                        Console.WriteLine($"You have {employees.Count} employee{(employees.Count() > 1 ? "s" : "")} loaded already, would you like to overwrite them?");
                        nthTime = 0;     // number of times the user has tried to input a value in this prompter
                        string response; // user's response, valid or not

                        // ask the user whether they would like to overwrite the currently loaded data, using Prompter to check for validity
                        // this loops until a valid input is received
                        do
                        {
                            response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.YN) : prompter.Ask(Prompter.AllowedWhatValues.YN, true);
                        } while (response != "y" && response != "n");

                        // set overwrite accordingly
                        overwrite = response == "y" ? true : false;
                    }
                    // if there is no employee data loaded, or if the user wants to overwrite it
                    if (overwrite)
                    {
                        // reset the employees list and prompt the user to load a file
                        employees = new List<Employee>();
                        LoadFile();
                    }
                    // if there is employee data loaded and the user would not like to overwrite it
                    else
                    {
                        Console.WriteLine("Returning to menu.");
                    }
                    break;

                case 2: // Add an employee
                    // add a brand new employee to the employees list
                    employees.Add(new Employee(employees.Count() + 1));
                    break;

                case 3: // Remove an employee
                    // select an employee to remove (this may return null if there are no employees to remove)
                    employee = SelectEmployee();

                    // if there is an employee to remove and it was selected
                    if (employee is not null)
                    {
                        employees.Remove(employee);
                    }
                    break;

                case 4: // Display an employee's details
                    // select an employee whose details to display (this may return null if there are no employees whose details to display)
                    employee = SelectEmployee();

                    // if there is an employee whose details to display and it was selected
                    if (employee is not null)
                    {
                        employee.DisplayDetails();
                    }
                    break;

                case 5: // Display all employee's details
                    // if there are employees whose details to display
                    if (employees.Count > 0)
                    {
                        // display the details of each employee in employees
                        foreach (Employee e in employees)
                        {
                            e.DisplayDetails();
                            Console.WriteLine();
                        }
                    }
                    // if there are no employees whose details to display
                    else
                    {
                        Console.WriteLine("No employees found. Returning to menu.");
                    }
                    break;

                case 6: // Edit an employee's details
                    // select an employee to edit (this may return null if there are no employees to edit)
                    employee = SelectEmployee();

                    // if there is an employee to edit and it was selected
                    if (employee is not null)
                    {
                        employee.Edit();
                    }
                    break;

                case 7: // Save current employee list
                    // if there is loaded employee data to save
                    if (employees.Count > 0)
                    {
                        SaveFile();
                    }
                    // if there is no loaded employee data to save
                    else
                    {
                        Console.WriteLine("No employees found. Returning to menu.");
                    }
                    break;

                case 8: // Quit
                    // set choice to 0 to break the while loop and exit
                    choice = 0;
                    break;

                default: // If choice is not 1-8
                    Console.WriteLine("Please enter a number from 1-8.");
                    break;
            }
        }
        // load employee information from a specially-formatted file (doesn't handle incorrect formatting)
        void LoadFile()
        {
            int nthTime = 0;  // number of times the user has tried to input a filename
            string? filename; // the user-input filename (relative or explicit)

            // ask the user for a filename
            // this runs only until there is any input, it does not check if that input is a real or usable file
            do
            {
                // set filename to the user input, displaying a custom message upon invalid input
                filename = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.File) : prompter.Ask(Prompter.AllowedWhatValues.File, true);
                // increments nthTime in order to display the error message if necessary
                nthTime++;
            } while (string.IsNullOrEmpty(filename));
            
            string[] lines = System.IO.File.ReadAllLines(filename); // reads each line in the file to a string array
            
            // adds a new employee to the employees list for each line in the file
            for (int i = 0; i < lines.Count(); i++)
            {
                // each piece of information necessary to create an Employee is separated by a ":" (addresses are handled separately)
                // this splits those apart and creates an employee with that information
                string[] linesSplit = lines[i].Split(":");
                employees.Add(new Employee(int.Parse(linesSplit[0]), linesSplit[1], linesSplit[2], linesSplit[3], linesSplit[4]));
            }

            Console.WriteLine("File loaded.");
        }
        // save currently loaded employee information to a user-specified file
        void SaveFile()
        {
            Console.WriteLine("Please enter the name of the file to which you would like to save.");
            Console.WriteLine("If nothing is entered, it will be saved as employees.txt");
            string? filename = Console.ReadLine(); // the user's chosen filename

            // if the user did not choose a filename, set it to a default value
            if (string.IsNullOrEmpty(filename))
            {
                filename = "employees.txt";
            }

            List<string> lines = new List<string>(); // list of lines to write

            // add a line for each employee loaded
            foreach (Employee employee in employees)
            {
                lines.Add(employee.GetInfo());
            }

            // write each line to a text file
            using (StreamWriter file = new StreamWriter(filename))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }
        // select and return an employee from the currently loaded set, chosen by employee ID (You can display that info if you don't know the ID, as they are auto-generated)
        Employee? SelectEmployee()
        {
            // if there is no employee information to select from
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found. Returning to menu.");
                return null;
            }

            string? response = null; // the user's response to the below question(s)
            int nthTime = 0;         // number of times the user has attempted to select an employee
            int number;              // the employee ID that the user input

            // ask the user for an employee ID
            // this will loop until either a valid ID is selected, or 0 is input to exit without selecting an employee
            do
            {
                // set response to the user input, displaying a custom message upon invalid input
                response = nthTime > 0 ? prompter.Ask(Prompter.AllowedWhatValues.SelectEmployee, true, response) : prompter.Ask(Prompter.AllowedWhatValues.SelectEmployee);
                // sets number equal to the user input (as an int) if it is able to parse it
                int.TryParse(response, out number);
                // increments nthTime in order to display the error message if necessary
                nthTime++;
            } while (string.IsNullOrEmpty(number.ToString()) || number > employees.Count());

            // if the user entered 0 to cancel
            if (number == 0)
            {
                Console.WriteLine("Canceled.");
                return null;
            }
            // if the user selected a valid employee ID
            else
            {
                return employees[number-1];
            }
        }
    }
}