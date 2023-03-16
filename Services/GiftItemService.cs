
using System.Text.Json;

public class GiftItemService : IGiftItemService
{
    private readonly IProducerRepository producerRepository;
    public GiftItemService(IProducerRepository producerRepository)
    {
       this.producerRepository=producerRepository;
    }

    public void AddToCart(CartDetail cartDetail)
    {
        this.producerRepository.AddToCart(cartDetail);
    }

    public void Create(GiftItem item)
    {
        this.producerRepository.Create(item);
    }

    public void DeleteGiftItem(int id)
    {
        this.producerRepository.DeleteGiftItem(id);
    }

    public List<CartDetail> GetCardEntry(string? name)
    {
        return this.producerRepository.GetCardEntry(name);
    }

    public GiftItem? GetGiftItemDetail(int id)
    {
        return this.producerRepository.GetGiftItemDetail(id);
    }

    public IEnumerable<GiftItem> GetGiftItems()
    {
        return this.producerRepository.GetGiftItems();
    }

    public void UpdateGiftItem(GiftItem model)
    {
        this.producerRepository.UpdateGiftItem(model);
    }

    public  List<CartEntryGiftItemDetail> GetCardGiftItemDetail(string? name)
    {
        return this.producerRepository.GetCardGiftItemDetail(name);
    }

    public  void PlaceOrder(string userName)
    {
        var cartData=this.producerRepository.GetCardGiftItemDetail(userName);
        List<OrderDetail> orderDetails=new List<OrderDetail>();
        foreach(var item in cartData)
        {
            OrderDetail orderDetail=new OrderDetail{
           GiftItemId=item.GiftItemID,
           Units=item.Units,
           CostPerUnit=item.Price
        };
        orderDetails.Add(orderDetail);
        }
        
        Order order=new Order{
            UserId=cartData[0].UserId,
            NumberOfItemsOrder=cartData.Sum(x=>x.Units),
            TotalAmount=cartData.Sum(x=>x.Units*x.Price),
            BusinessDay=DateTime.Now,
            OrderDetails=orderDetails
        };
        string message = JsonSerializer.Serialize(order);
        ProduceEvents events=new ProduceEvents();
        events.SendOrderRequest("test",message);
        this.producerRepository.PlaceOrder(order);
        this.producerRepository.RemoveCart(userName);
    }

    public List<Order> GetOrders(string userName)
    {
        return this.producerRepository.GetOrders(userName);
    }

    public List<OrderDetailViewModel> GetOrderDetails(int orderId)
    {
        return this.producerRepository.GetOrderDetails(orderId);
    }
}