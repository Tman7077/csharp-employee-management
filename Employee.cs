class Employee
{
    private int? _id = null;       // employee ID, auto-generated (incremented by 1)
    private string? _name = null;  // employee name, stored as one string for first, middle, last, and whatever else
    private string? _email = null; // employee email
    private string? _phone = null; // employee phone, stored as a string (does not handle correcting the formatting)
    private Address _address;      // employee address, as an Address object rather than a string

    // create an employee, given the employee number (ID)
    public Employee(int id)
    {
        Prompter prompter = new Prompter(); // create a Prompter to ask the user questions

        string? name = null;  // employee name
        string? email = null; // employee email
        string? phone = null; // employee address

        List<string> neededInfo = new List<string>{"name", "email", "phone"}; // items to ask the user for
        
        // loop through neededInfo, asking the user for each item in it
        foreach (string item in neededInfo)
        {
            string? response = null; // the user's response
            int nthTime = 0;         // the number of times the user has tried to input an answer

            // ask the user for the given item
            // this loops until all answers are received (any answer, formatting is not checked)
            do {
                response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Employee, false, item) : prompter.Ask(Prompter.AllowedWhatValues.Employee, true, item);
                nthTime++;
            } while (string.IsNullOrEmpty(response));

            // set the correct item to the user's response
            switch (item)
            {
                case "name":
                    name = response;
                    break;
                case "email":
                    email = response;
                    break;
                case "phone":
                    phone = response;
                    break;
            }
        }

        // set member variables based on the above
        // this also calls a similar constructor for colecting the information for the address
        _id = id;
        _name = name;
        _email = email;
        _phone = phone;
        _address = new Address();

        Console.WriteLine($"Employee with ID {_id} created.");
    }
    // create an Employee, given all necessary information
    public Employee(int id, string name, string email, string phone, string address)
    {
        _id = id;
        _name = name;
        _email = email;
        _phone = phone;
        _address = new Address(address.Split("|"));
    }

    // display all relevant employee information to the console
    public void DisplayDetails()
    {
        Console.WriteLine($"ID: {_id}");
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine($"Email: {_email}");
        Console.WriteLine($"Phone: {_phone}");
        Console.WriteLine($"Address:");
        _address.DisplayDetails();
    }
    // return a string that is formatted for exporting
    public string GetInfo()
    {
        string address = _address.GetInfo(); // address as a similarly formatted string
                                             // this is separated with "|" rather than ":" for differentiation
        List<string> infoList = [_id.ToString(), _name, _email, _phone, address]; // items to return
        string infoString = ""; // string to contain relevant information, separated by ":"

        // loop through infoList and add each bit
        foreach (string str in infoList)
        {
            infoString += str + ":";
        }

        // return everything except the last character (which is ":")
        return infoString.Substring(0, infoString.Length - 1);
    }
    // edit the details of an already-created employee
    public void Edit()
    {
        Prompter prompter = new Prompter(); // create a Prompter to ask the user questions
        int nthTime = 0;          // the number of times the user has input something
                                  // this is set outside of the do-while because it keeps track of
                                  // if the user wants to chenge multiple things for one employee
        int number;               // the user's input number
        bool canContinue = false; // whether the input number is actually a valid choice

        // ask the user which item they would like to edit
        // this loops until the user inputs a number from 1-4
        do
        {
            bool success = true; // whether the user input any parseable int
            string response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Edit) : !success ? prompter.Ask(Prompter.AllowedWhatValues.Edit, true) : prompter.Ask(Prompter.AllowedWhatValues.Edit, true, "employee,true");
            success = int.TryParse(response, out number);

            // determine whether the user-input number is a valid choice (1-4)
            if (success)
            {
                canContinue = number > 0 && number < 5 ? true : false;
            }
            nthTime++;
        } while (!canContinue);

        // set the user's choice item to the correct variable
        switch (number)
        {
            case 1:
                _name = doEdit("name");
                break;
            case 2:
                _email = doEdit("email");
                break;
            case 3:
                _phone = doEdit("phone");
                break;
            case 4:
                _address.Edit();
                break;
        }

        // perform the edit, asking the user for the new content of the item they selected above
        string doEdit(string what)
        {
            string response = ""; // the string to which they would like to change the value
            nthTime = 0;          // the number of times the user has input something or nothing

            // ask the user for their input
            // this will loop until there is any input at all
            do {
                response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Employee, false, what) : prompter.Ask(Prompter.AllowedWhatValues.Employee, true, what);              
                nthTime++;
            } while (string.IsNullOrEmpty(response));
            return response;
        }
    }
}