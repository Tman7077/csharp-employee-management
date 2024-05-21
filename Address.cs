class Address
{
    private string _street;  // number and street name
    private string _city;    // city
    private string _state;   // state / province
    private string _zipCode; // zip code
    private string _country; // country

    // create a new Address, asking for all relevant pieces of information
    public Address()
    {
        Prompter prompter = new Prompter(); // create a Prompter to ask the user questions
        List<string> neededInfo = new List<string>{"street", "city", "state", "zip code", "country"}; // items for which to ask the user
        // empty strings to fill later
        string street = "";
        string city = "";
        string state = "";
        string zipCode = "";
        string country = "";

        // loop through each item in neededInfo and ask the user for input
        foreach (string item in neededInfo)
        {
            string response;
            int nthTime = 0;

            // ask the user for input
            // this will loop until there is any input at all
            do
            {
                response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Address, false, item) : prompter.Ask(Prompter.AllowedWhatValues.Address, true, item);
                nthTime++;
            } while (string.IsNullOrEmpty(response));

            // set the corresponding variable accordingly
            switch (item)
            {
                case "street":
                    street = response;
                    break;
                case "city":
                    city = response;
                    break;
                case "state":
                    state = response;
                    break;
                case "zip code":
                    zipCode = response;
                    break;
                case "country":
                    country = response;
                    break;
            }
        }

        // set the Address attributes to the above input
        _street = street;
        _city = city;
        _state = state;
        _zipCode = zipCode;
        _country = country;
    }
    // create an address given a string array containing the needed information
    public Address(string[] parts)
    {
        _street = parts[0];
        _city = parts[1];
        _state = parts[2];
        _zipCode = parts[3];
        _country = parts[4];
    }

    // display the address as a correctly formatted string in the console
    public void DisplayDetails()
    {
        Console.WriteLine($"{_street}\n{_city}, {_state} {_zipCode}\n{_country}");
    }
    // return a string that is formatted for exporting
    public string GetInfo()
    {
        List<string> infoList = [_street, _city, _state, _zipCode, _country]; // items to return
        string infoString = ""; // string to contain relevant information, separated by "|"

        // loop through infoList and add each bit 
        foreach (string str in infoList)
        {
            infoString += str + "|";
        }

        // return everything except the last character (which is "|")
        return infoString.Substring(0, infoString.Length - 1);
    }
    // edit the details of an already-created address
    public void Edit()
    {
        Prompter prompter = new Prompter(); // create a Prompter to ask the user questions
        int nthTime = 0;          // number of times the user has input something
                                  // this is set outside of the do-while because it keeps track of
                                  // if the user wants to chenge multiple things for one employee
        int number;               // the user's input number
        bool canContinue = false; // whether the input number is actually a valid choice

        // ask the user which item they would like to edit
        // this loops until the user inputs a number from 1-5
        do
        {
            bool success = true; // whether the user input any parseable int
            string response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Edit, false, "address,false") : !success ? prompter.Ask(Prompter.AllowedWhatValues.Edit, true, "address,false") : prompter.Ask(Prompter.AllowedWhatValues.Edit, true, "address,true");
            success = int.TryParse(response, out number);

            // determine whether the user-input number is a valid choice (1-5)
            if (success)
            {
                canContinue = number > 0 && number < 6 ? true : false;
            }
            nthTime++;
        } while (!canContinue);

        // set the user's choice item to the correct variable
        switch (number)
        {
            case 1:
                _street = doEdit("street");
                break;
            case 2:
                _city = doEdit("city");
                break;
            case 3:
                _state = doEdit("state");
                break;
            case 4:
                _zipCode = doEdit("zip code");
                break;
            case 5:
                _country = doEdit("country");
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
                response = nthTime == 0 ? prompter.Ask(Prompter.AllowedWhatValues.Address, false, what) : prompter.Ask(Prompter.AllowedWhatValues.Address, true, what);
                nthTime++;
            } while (string.IsNullOrEmpty(response));
            return response;
        }
    }
}