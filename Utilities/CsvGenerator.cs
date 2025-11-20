using System.Text;

namespace CsvParserChallenge.Utilities
{
    public static class CsvGenerator
    {
        private static readonly Random Rand = new();

        private static readonly string[] FirstNames =
            { "John","Jane","Alex","Chris","Sam","Emily","Tyler","Kate","Robert","Linda" };

        private static readonly string[] LastNames =
            { "Smith","Doe","Johnson","Brown","Davis","Miller","Wilson","Garcia","Clark","Hall" };

        private static readonly string[] States =
            { "GA","FL","NY","TX","CA","IL","WA","OR","AZ","NC" };

        private static readonly string[] CarMakes =
            { "Toyota","Honda","Ford","Chevy","Nissan","BMW","Audi","Hyundai","Kia","Mazda" };

        private static readonly string[] CarModels =
            { "Corolla","Civic","Accord","Camry","Focus","Model3","Altima","Soul","CX5","Pilot" };

        public static void Generate(string path, int rows)
        {
            using var writer = new StreamWriter(path, false, Encoding.UTF8);

            writer.WriteLine(
                "customer_id,first_name,last_name,email,phone_number,address,city,state,postal_code,car_make,car_model,car_year,license_plate,purchase_date,purchase_price");

            for (int i = 1; i <= rows; i++)
            {
                string first = RandArray(FirstNames);
                string last = RandArray(LastNames);
                string email = $"{first.ToLower()}.{last.ToLower()}{i}@example.com";

                string phone = $"{Rand.Next(100, 999)}-{Rand.Next(100, 999)}-{Rand.Next(1000, 9999)}";
                string address = $"{Rand.Next(1, 9999)} Main St";
                string city = "City" + Rand.Next(1, 100);

                string state = RandArray(States);
                string postal = Rand.Next(10000, 99999).ToString();

                string make = RandArray(CarMakes);
                string model = RandArray(CarModels);
                int year = Rand.Next(2000, 2025);

                string license = $"{RandomLetters(3)}{Rand.Next(100, 999)}";

                string date = new DateTime(
                    Rand.Next(2005, 2025),
                    Rand.Next(1, 12),
                    Rand.Next(1, 28)).ToString("MM/dd/yyyy");

                decimal price = Rand.Next(3000, 50000);

                writer.WriteLine($"{i},{first},{last},{email},{phone},{address},{city},{state},{postal},{make},{model},{year},{license},{date},{price}");
            }
        }

        private static string RandArray(string[] arr) => arr[Rand.Next(arr.Length)];

        private static string RandomLetters(int count)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] result = new char[count];
            for (int i = 0; i < count; i++)
                result[i] = letters[Rand.Next(letters.Length)];
            return new string(result);
        }
    }
}
