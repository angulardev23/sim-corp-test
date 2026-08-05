using System.Collections.Generic;

namespace BackendTest.Application.Models;

public sealed record PurchaseData(int Id, int CustomerId, IReadOnlyList<int> ProductId);
