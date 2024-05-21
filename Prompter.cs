class Prompter
{
    // the different types of prompts that can be made by a Prompter
    public enum AllowedWhatValues {
        Menu,
        File,
        Employee,
        Address,
        SelectEmployee,
        Edit,
        YN
    }
    
    // the Prompter object doesn't hold any information other than the above enum,
    // since it is used as a function object, so it doesn't need any info in its constructor
    public Prompter() {}

    // ask the user a question
    // the action of this function is largely determined by the AllowedWhatValues input, as it is one big switch statement
    public string Ask(AllowedWhatValues what, bool again = false, string? extraInput = null)
    {
        switch (what)
        {
            case AllowedWhatValues.Menu: // display the main menu to the user
                                         // extraInput is the number of times the user has used the menu ("0")
                // if the user input an invalid choice
                if (again)
                {
                    Console.WriteLine($"Your response cannot be blank, and must be a number from 1-8. Enter 8 to quit");
                }

                // display the menu, using extraInput to determine if "what" should turn into "what else"
                Console.WriteLine($"What {(!string.IsNullOrEmpty(extraInput) ? (int.Parse(extraInput) > 0 ? "else " : "") : "")}would you like to do?");
                Console.WriteLine("  1. Load an employee database (.txt)");
                Console.WriteLine("  2. Add an employee");
                Console.WriteLine("  3. Remove an employee");
                Console.WriteLine("  4. Display an employee's details");
                Console.WriteLine("  5. Display all employee's details");
                Console.WriteLine("  6. Edit an employee's details");
                Console.WriteLine("  7. Save current employee list");
                Console.WriteLine("  8. Quit");
                return "" + Console.ReadLine();
            case AllowedWhatValues.File: // ask the user for a filename
                                         // this does not check for validity
                Console.WriteLine("Enter the name of the file you would like to load.");
                return "" + Console.ReadLine();
            case AllowedWhatValues.Employee: // ask the user for a piece of employee information
            case AllowedWhatValues.Address:  // the same template is used for address information
                                             // extraInput is the item for which to ask ("name")
                // if the user did not input anything
                if (again)
                {
                    Console.WriteLine($"{extraInput} cannot be blank.");
                }

                // prompt for the information
                Console.WriteLine($"Enter employee {extraInput}:");
                return "" + Console.ReadLine();
            case AllowedWhatValues.SelectEmployee: // prompt the user to select an employee
                                                   // extraInput is the employee number they selected, only used on retrial
                // if the user input either nothing, or an employee number that cannot be found
                if (again)
                {
                    // if the user input nothing
                    if (extraInput is null)
                    {
                        Console.WriteLine("Employee number cannot be blank.");
                    }
                    // if the user input an invalid number
                    else if (extraInput is string)
                    {
                        Console.WriteLine($"No employee with number {extraInput} found.");
                    }
                }

                // prompt the user for an employee number
                Console.WriteLine("Enter an employee number (enter 0 to cancel):");
                return "" + Console.ReadLine();
            case AllowedWhatValues.Edit: // prompt the user for just one item of an employee's information to edit
                                         // extraInput is a string concatenation of either "employee" or "address",
                                         // determining which menu to show, and "true" or "false",
                                         // representing whether the previous input was an invalid one
                string employeeOrAddress = extraInput is not null ? extraInput.Split(",")[0] : "employee"; // either "employee" or "address"
                bool invalidInput = extraInput is not null ? bool.Parse(extraInput.Split(",")[1]) : false; // either true or false, as a bool

                // if the user is either editing a second piece of information,
                // or input an invalid choice the first time around
                if (again)
                {
                    // if the previous input was not a valid choice (1-4 or 1-5 for employee or address respectively)
                    if (extraInput is not null ? invalidInput : false)
                    {
                        Console.WriteLine($"Please enter a number from 1-{(employeeOrAddress == "employee" ? 4 : 5)}.");
                        Console.WriteLine("What would you like to edit?");
                    }
                    // if the user is editing a second piece of information
                    else
                    {
                        Console.WriteLine("What else would you like to edit?");
                    }
                }
                // if this is the user's first time being asked
                else
                {
                    Console.WriteLine("What would you like to edit?");
                }

                // if the user is editing employee details
                if (employeeOrAddress == "employee")
                {
                    Console.WriteLine("  1. Name");
                    Console.WriteLine("  2. Email");
                    Console.WriteLine("  3. Phone");
                    Console.WriteLine("  4. Address");
                }
                // if the user is editing address details
                else if (employeeOrAddress == "address")
                {
                    Console.WriteLine("  1. Street");
                    Console.WriteLine("  2. City");
                    Console.WriteLine("  3. State");
                    Console.WriteLine("  4. Zip Code");
                    Console.WriteLine("  5. Country");
                }
                return "" + Console.ReadLine();
            case AllowedWhatValues.YN: // asks yes or no, in the form of "y" or "n"
                // if the user did not input "y" or "n"
                if (again)
                {
                    Console.WriteLine("""Please input "y" or "n".""");
                }
                // if this is the user's first time being asked
                else
                {
                    Console.WriteLine("""Yes or no? Input "y" or "n".""");
                }
                return "" + Console.ReadLine();
            default: // this should never run, but it is a required part of the switch statement because there is no other return value in Ask()
                Console.WriteLine("Error.");
                return "";
        }
    }
}