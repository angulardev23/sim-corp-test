namespace BackendTest.Infrastructure.Persistence.Records;

internal sealed class PurchaseProductRecord
{
    internal PurchaseProductRecord(int purchaseId, int productId, int quantity)
    {
        PurchaseId = purchaseId;
        ProductId = productId;
        Quantity = quantity;
    }

    public int PurchaseId { get; private set; }
    public PurchaseRecord Purchase { get; private set; } = null!;
    public int ProductId { get; private set; }
    public ProductRecord Product { get; private set; } = null!;
    public int Quantity { get; private set; }
}
