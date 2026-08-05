using System.Globalization;
using System.Text;
using BackendTest.Application.Models;
using BackendTest.Application.Reports;

namespace BackendTest.Infrastructure.Reports
{
    public sealed class CsvPurchaseReportFormatter : IPurchaseReportFormatter
    {
        private const char Delimiter = ';';
        private const string NewLine = "\r\n";
        private const string CustomerNameHeader = "CustomerName:";
        private const string ProductIdHeader = "ProductId";
        private const string CountHeader = "Count";
        private const string ProductNameHeader = "ProductName";
        private const string PriceHeader = "Price";

        private static readonly Encoding Utf8WithoutBom = new UTF8Encoding(
            encoderShouldEmitUTF8Identifier: false);

        private static readonly NumberFormatInfo PriceFormat = new()
        {
            NumberDecimalSeparator = ","
        };

        public byte[] Format(PurchaseReport report)
        {
            var csv = new StringBuilder();
            AppendRow(csv, CustomerNameHeader, report.CustomerName);
            AppendRow(csv, ProductIdHeader, CountHeader, ProductNameHeader, PriceHeader);

            foreach (var line in report.Lines)
            {
                AppendRow(
                    csv,
                    line.ProductId.ToString(CultureInfo.InvariantCulture),
                    line.Count.ToString(CultureInfo.InvariantCulture),
                    line.ProductName,
                    line.Price.ToString("0.00", PriceFormat));
            }

            return Utf8WithoutBom.GetBytes(csv.ToString());
        }

        private static void AppendRow(StringBuilder csv, params string[] values)
        {
            for (var index = 0; index < values.Length; index++)
            {
                if (index > 0)
                {
                    csv.Append(Delimiter);
                }

                csv.Append(Escape(values[index]));
            }

            csv.Append(NewLine);
        }

        private static string Escape(string value)
        {
            if (!value.Contains(Delimiter) && !value.Contains('"') && !value.Contains('\r') && !value.Contains('\n'))
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
    }
}
