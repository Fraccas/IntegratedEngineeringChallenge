using CsvParserChallenge.Models;
using CsvParserChallenge.Services;
using CsvParserChallenge.Utilities;
using CsvParserChallenge.Validators;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CsvParserChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            // ================================================================
            // Toggle between:
            //    Assignment Mode  -> full logging
            //    Benchmark Mode   -> no logging (much faster)
            // ================================================================
            bool isLargeDataset = false;   // SET TO false for assignment submission

            if (isLargeDataset)
            {
                // Generate 500K rows only when testing performance
                CsvGenerator.Generate("input_large.csv", 500000);
            }

            string inputPath = isLargeDataset ? "input_large.csv" : "input.csv";

            Console.WriteLine("=== CSV Parser Challenge Benchmark ===\n");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"ERROR: File not found: {inputPath}");
                return;
            }

            var parser = new CsvParser(inputPath);

            // Use logging depending on mode
            var validator = new CustomerValidator(enableLogging: !isLargeDataset);

            // --------------------------------------------------------------
            // Parse CSV
            // --------------------------------------------------------------
            var swParse = Stopwatch.StartNew();
            var rows = new List<(int RowNumber, string[] Fields)>(parser.Parse());
            swParse.Stop();

            Console.WriteLine($"Parsed {rows.Count} rows in {swParse.ElapsedMilliseconds} ms\n");

            // --------------------------------------------------------------
            // Run performance comparisons
            // --------------------------------------------------------------
            RunMultithreaded(rows, validator);
            RunSingleThreaded(rows, validator);
        }

        // ======================================================================
        //  MULTITHREADED VALIDATION MODE
        // ======================================================================
        private static void RunMultithreaded(
            List<(int RowNumber, string[] Fields)> rows,
            CustomerValidator validator)
        {
            Console.WriteLine("=== Multithreaded Validation ===");

            var validRecords = new ConcurrentBag<CustomerRecord>();
            var swValidation = Stopwatch.StartNew();

            // Parallel.ForEach runs iterations on multiple threads.
            // Safe because each row is independent AND validRecords is a thread-safe ConcurrentBag.
            Parallel.ForEach(rows, row =>
            {
                var (rowNumber, fields) = row;

                if (fields.Length == 0)
                    return;

                if (validator.TryValidate(rowNumber, fields, out var record))
                    validRecords.Add(record!);
            });

            swValidation.Stop();

            Console.WriteLine($"Validated: {swValidation.ElapsedMilliseconds} ms");
            Console.WriteLine($"Valid records: {validRecords.Count}");

            var swJson = Stopwatch.StartNew();
            JsonExporter.Export("output_parallel.json", validRecords);

            swJson.Stop();

            Console.WriteLine($"JSON export: {swJson.ElapsedMilliseconds} ms\n");
        }

        // ======================================================================
        //  SINGLE-THREADED VALIDATION MODE
        // ======================================================================
        private static void RunSingleThreaded(
            List<(int RowNumber, string[] Fields)> rows,
            CustomerValidator validator)
        {
            Console.WriteLine("=== Single-threaded Validation ===");

            var validRecords = new List<CustomerRecord>();
            var swValidation = Stopwatch.StartNew();

            foreach (var (rowNumber, fields) in rows)
            {
                if (fields.Length == 0)
                    continue;

                if (validator.TryValidate(rowNumber, fields, out var record))
                    validRecords.Add(record!);
            }

            swValidation.Stop();

            Console.WriteLine($"Validated: {swValidation.ElapsedMilliseconds} ms");
            Console.WriteLine($"Valid records: {validRecords.Count}");

            var swJson = Stopwatch.StartNew();
            JsonExporter.Export("output_single.json", validRecords);

            swJson.Stop();

            Console.WriteLine($"JSON export: {swJson.ElapsedMilliseconds} ms\n");
        }
    }
}
