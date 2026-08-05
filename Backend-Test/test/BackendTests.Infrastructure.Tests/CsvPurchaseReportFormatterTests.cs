using System.Text;
using BackendTest.Application.Models;
using BackendTest.Infrastructure.Reports;
using Xunit;

namespace BackendTests.Infrastructure.Tests
{
    public sealed class CsvPurchaseReportFormatterTests
    {
        private readonly CsvPurchaseReportFormatter _formatter = new();

        [Fact]
        public void Format_ReturnsSpecifiedCsvContract()
        {
            var report = new PurchaseReport(
                1,
                "John Doe",
                [
                    new PurchaseReportLine(1, 1, "Pipe Wrench", 19.99m),
                    new PurchaseReportLine(3, 2, "Garden Hose", 4.99m),
                    new PurchaseReportLine(4, 1, "Toilet Plunger", 1.49m)
                ]);

            var csv = Encoding.UTF8.GetString(_formatter.Format(report));

            Assert.Equal(
                "CustomerName:;John Doe\r\n" +
                "ProductId;Count;ProductName;Price\r\n" +
                "1;1;Pipe Wrench;19,99\r\n" +
                "3;2;Garden Hose;4,99\r\n" +
                "4;1;Toilet Plunger;1,49\r\n",
                csv);
        }

        [Fact]
        public void Format_EscapesCsvControlCharacters()
        {
            var report = new PurchaseReport(
                1,
                "Doe; John",
                [new PurchaseReportLine(1, 1, "24\" Pipe", 19.99m)]);

            var csv = Encoding.UTF8.GetString(_formatter.Format(report));

            Assert.Contains("CustomerName:;\"Doe; John\"\r\n", csv);
            Assert.Contains("1;1;\"24\"\" Pipe\";19,99\r\n", csv);
        }
    }
}
