namespace CsvParserChallenge.Models
{
    public class CustomerRecordJson
    {
        public int customer_id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postal_code { get; set; }
        public string? car_make { get; set; }
        public string? car_model { get; set; }
        public int car_year { get; set; }
        public string? license_plate { get; set; }
        public DateTime purchase_date { get; set; }
        public decimal purchase_price { get; set; }
    }
}
