using System.Collections.Generic;

namespace BackendTest.Application.Models;

public sealed record PurchaseReport(
    int PurchaseId,
    string CustomerName,
    IReadOnlyList<PurchaseReportLine> Lines);

public sealed record PurchaseReportLine(
    int ProductId,
    int Count,
    string ProductName,
    decimal Price);
