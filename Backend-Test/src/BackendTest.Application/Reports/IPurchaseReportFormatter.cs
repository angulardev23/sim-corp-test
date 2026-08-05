using BackendTest.Application.Models;

namespace BackendTest.Application.Reports
{
    public interface IPurchaseReportFormatter
    {
        byte[] Format(PurchaseReport report);
    }
}
