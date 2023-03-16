public interface IGiftItemService{
    IEnumerable<GiftItem> GetGiftItems();
    GiftItem? GetGiftItemDetail(int id);
    void UpdateGiftItem(GiftItem model);
    void DeleteGiftItem(int id);
    void Create(GiftItem item);
    void AddToCart(CartDetail cartDetail);
    List<CartDetail> GetCardEntry(string? name);
    List<CartEntryGiftItemDetail> GetCardGiftItemDetail(string? name);
    void PlaceOrder(string userName);

    List<Order> GetOrders(string userName);
    List<OrderDetailViewModel> GetOrderDetails(int orderId);
}