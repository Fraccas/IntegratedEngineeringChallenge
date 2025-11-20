namespace CsvParserChallenge.Services
{
    public class CsvParser
    {
        private readonly string _filePath;

        public CsvParser(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Reads CSV and returns list of field arrays.
        /// First line (header) is skipped automatically.
        /// </summary>
        public IEnumerable<(int RowNumber, string[] Fields)> Parse()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"CSV not found: {_filePath}");
            }

            var lines = File.ReadLines(_filePath);
            bool isFirstLine = true;
            int rowNumber = 1;

            foreach (var line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false; // Skip header
                    rowNumber++;
                    continue;
                }

                // Handle empty or whitespace-only rows
                if (string.IsNullOrWhiteSpace(line))
                {
                    yield return (rowNumber, Array.Empty<string>());
                    rowNumber++;
                    continue;
                }

                // Split CSV on commas (simple version)
                var fields = line.Split(',');

                yield return (rowNumber, fields);

                rowNumber++;
            }
        }
    }
}
