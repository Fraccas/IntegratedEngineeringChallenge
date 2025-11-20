using CsvParserChallenge.Models;
using System.Text.RegularExpressions;

namespace CsvParserChallenge.Validators
{
    public class CustomerValidator
    {
        // Enables or disables printing validation errors.
        private readonly bool _enableLogging;

        public CustomerValidator(bool enableLogging = true)
        {
            _enableLogging = enableLogging;
        }

        // Email regex
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // Phone number: XXX-XXX-XXXX
        private static readonly Regex PhoneRegex =
            new(@"^\d{3}-\d{3}-\d{4}$", RegexOptions.Compiled);

        // License plate: ABC123 or 123ABC
        private static readonly Regex LicenseRegex =
            new(@"^([A-Za-z]{3}\d{3}|\d{3}[A-Za-z]{3})$", RegexOptions.Compiled);

        // Valid 2-letter US states
        private static readonly HashSet<string> ValidStates = new(StringComparer.OrdinalIgnoreCase)
        {
            "AL","AK","AZ","AR","CA","CO","CT","DE","FL","GA","HI","ID","IL","IN","IA","KS","KY",
            "LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND",
            "OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VT","VA","WA","WV","WI","WY"
        };

        public bool TryValidate(int rowNumber, string[] fields, out CustomerRecord? customer)
        {
            customer = null;

            if (fields.Length != 15)
            {
                PrintError(rowNumber, "Invalid number of fields (expected 15).");
                return false;
            }

            int i = 0;

            // Extract
            string customerIdStr = fields[i++].Trim();
            string firstName = fields[i++].Trim();
            string lastName = fields[i++].Trim();
            string email = fields[i++].Trim();
            string phone = fields[i++].Trim();
            string address = fields[i++].Trim();
            string city = fields[i++].Trim();
            string state = fields[i++].Trim();
            string postalCode = fields[i++].Trim();
            string carMake = fields[i++].Trim();
            string carModel = fields[i++].Trim();
            string carYearStr = fields[i++].Trim();
            string license = fields[i++].Trim();
            string purchaseDateStr = fields[i++].Trim();
            string purchasePriceStr = fields[i++].Trim();

            // Start validation
            var errors = new List<string>();

            // customer_id
            if (!int.TryParse(customerIdStr, out int customerId) || customerId <= 0)
                errors.Add("customer_id must be a positive integer.");

            // first_name
            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("first_name is required.");

            // last_name
            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("last_name is required.");

            // email
            if (!EmailRegex.IsMatch(email))
                errors.Add("email format is invalid.");

            // phone_number
            if (!PhoneRegex.IsMatch(phone))
                errors.Add("phone_number must be XXX-XXX-XXXX.");

            // address
            if (string.IsNullOrWhiteSpace(address))
                errors.Add("address is required.");

            // city
            if (string.IsNullOrWhiteSpace(city))
                errors.Add("city is required.");

            // state
            if (state.Length != 2 || !ValidStates.Contains(state))
                errors.Add("state must be a valid two-letter US abbreviation.");

            // postal_code
            if (postalCode.Length > 5 || !int.TryParse(postalCode, out _))
                errors.Add("postal_code must be digits only, max length 5.");

            // car_make
            if (string.IsNullOrWhiteSpace(carMake))
                errors.Add("car_make is required.");

            // car_model
            if (string.IsNullOrWhiteSpace(carModel))
                errors.Add("car_model is required.");

            // car_year
            if (!int.TryParse(carYearStr, out int carYear) || carYear < 1900 || carYear > 2025)
                errors.Add("car_year must be between 1900 and 2025.");

            // license_plate
            if (!LicenseRegex.IsMatch(license))
                errors.Add("license_plate must be ABC123 or 123ABC format.");

            // purchase_date
            if (!DateTime.TryParse(purchaseDateStr, out DateTime purchaseDate) ||
                purchaseDate.Year < 2000 ||
                purchaseDate > DateTime.Now)
            {
                errors.Add("purchase_date must be MM/DD/YYYY and between 2000 and today.");
            }

            // purchase_price
            if (!decimal.TryParse(purchasePriceStr, out decimal purchasePrice))
                errors.Add("purchase_price must be a valid decimal number.");

            // If any errors: print and return false
            if (errors.Count > 0)
            {
                foreach (var err in errors)
                    PrintError(rowNumber, err);

                return false;
            }

            // Build CustomerRecord
            customer = new CustomerRecord
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                City = city,
                State = state,
                PostalCode = postalCode,
                CarMake = carMake,
                CarModel = carModel,
                CarYear = carYear,
                LicensePlate = license,
                PurchaseDate = purchaseDate,
                PurchasePrice = purchasePrice
            };

            return true;
        }

        private void PrintError(int row, string msg)
        {
            if (_enableLogging)
                Console.WriteLine($"Row {row}: {msg}");
        }
    }
}
