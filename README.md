# CSV Parser Challenge  
A C# console application that parses customer CSV data, validates each row according to strict field rules, logs errors when required, and exports all valid rows to JSON.

This project also includes a *Large Dataset Benchmark Mode* to compare **single-threaded vs multithreaded validation performance** using up to 500,000+ generated rows.

---

## 📌 Modes of Operation

This application supports **two modes**, controlled by a single flag inside `Program.cs`:

### **1. Assignment Mode (error logging ON)**  
Set:
```csharp
bool isLargeDataset = false;
```

✔ Prints validation errors  
✔ Uses your real input file (`input.csv`)  
✔ Generates clean JSON output  
✔ Ideal for assignment submissions

---

### **2. Benchmark Mode (error logging OFF)**  
Set:
```csharp
bool isLargeDataset = true;
```

✔ Disables error logging for speed  
✔ Auto-generates `input_large.csv` with 500,000 rows  
✔ Runs both:
- **Multithreaded validation**
- **Single-threaded validation**

✔ Outputs:
- `output_parallel.json`
- `output_single.json`

This mode demonstrates the performance impact of parallel CPU workloads.

---

## 📁 Project Structure

```
CsvParserChallenge/
 ├── Models/
 │     └── CustomerRecord.cs
 │     └── CustomerRecordJson.cs
 ├── Services/
 │     └── CsvParser.cs
 ├── Validators/
 │     └── CustomerValidator.cs
 ├── Utilities/
 │     └── CsvGenerator.cs
 │     └── JsonExporter.cs
 ├── Program.cs
 ├── input.csv
 ├── input_large.csv (auto-generated benchmark file)
 ├── output_single.json
 ├── output_parallel.json
 └── README.md
```

---

## 🧪 Validation Rules

Every CSV row must contain **15 fields**. Invalid rows are rejected and (optionally) logged.

| Field | Requirements |
|-------|--------------|
| `customer_id` | Positive integer |
| `first_name` | Non-empty string |
| `last_name` | Non-empty string |
| `email` | Valid email format (regex) |
| `phone_number` | `XXX-XXX-XXXX` with dashes |
| `address` | Non-empty string |
| `city` | Non-empty string |
| `state` | Valid 2-letter US state abbreviation |
| `postal_code` | Max 5 digits, numeric only |
| `car_make` | String |
| `car_model` | String |
| `car_year` | Integer between 1900–2025 |
| `license_plate` | Pattern: `ABC123` or `123ABC` |
| `purchase_date` | Valid date between year 2000 and today |
| `purchase_price` | Decimal number |

In **assignment mode**, each failed rule prints:
```
Row X: reason for failure
```

---

## ⚙️ How It Works

### **Step 1 — Parse the CSV**
`CsvParser` reads lines, skips the header, splits fields, and tracks row numbers.

### **Step 2 — Validate Rows**
`CustomerValidator.TryValidate()` checks every rule and returns:

- `true + CustomerRecord` for valid rows  
- `false` and optionally logs errors  

### **Step 3 — Export JSON**
Valid rows are serialized to JSON using `System.Text.Json`.

### **Step 4 — (Optional) Benchmark**
In large mode:

- Multithreaded: uses `Parallel.ForEach`  
- Single-threaded: uses `foreach`  
- Both produce separate JSON files and timing logs

---

## 🧵 Multithreading Benchmark Example Output

```
Parsed 500000 rows in 1200 ms

=== Multithreaded Validation ===
Validated: 258 ms
Valid records: 500000
JSON export: 835 ms

=== Single-threaded Validation ===
Validated: 577 ms
Valid records: 500000
JSON export: 700 ms
```

This demonstrates a **2× speed improvement** for parallel validation at scale.

---

## ▶️ Running the Application

### **Build**
```bash
dotnet build
```

### **Run**
```bash
dotnet run
```

### **Switch modes**  
Inside `Program.cs`, set:

#### Assignment mode:
```csharp
bool isLargeDataset = false;
```

#### Benchmark mode:
```csharp
bool isLargeDataset = true;
```

---

## 🧰 Technologies Used

- **C# / .NET 8**
- `System.Text.Json` for JSON serialization
- `Parallel.ForEach` for multithreaded CPU processing
- `Stopwatch` for benchmarking
- Regular expressions for data validation


