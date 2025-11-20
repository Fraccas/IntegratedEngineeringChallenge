using CsvParserChallenge.Models;
using System.Text.Json;

namespace CsvParserChallenge.Utilities
{
    public static class JsonExporter
    {
        public static void Export(string path, IEnumerable<CustomerRecord> records)
        {
            // Map to snake_case JSON objects
            var jsonReady = records.Select(r => new CustomerRecordJson
            {
                customer_id = r.CustomerId,
                first_name = r.FirstName,
                last_name = r.LastName,
                email = r.Email,
                phone_number = r.PhoneNumber,
                address = r.Address,
                city = r.City,
                state = r.State,
                postal_code = r.PostalCode,
                car_make = r.CarMake,
                car_model = r.CarModel,
                car_year = r.CarYear,
                license_plate = r.LicensePlate,
                purchase_date = r.PurchaseDate,
                purchase_price = r.PurchasePrice
            });

            var json = JsonSerializer.Serialize(
                jsonReady,
                new JsonSerializerOptions { WriteIndented = true }
            );

            File.WriteAllText(path, json);
        }
    }
}
