
using Microsoft.EntityFrameworkCore;

public class ProducerRepository : IProducerRepository
{
    private readonly ProducerContext producerContext;
    public ProducerRepository(ProducerContext producerContext)
    {
        this.producerContext=producerContext;
    }

    public void AddToCart(CartDetail cartDetail)
    {
        if(this.producerContext.CartDetails.Any(x=>x.ID==cartDetail.ID && x.GiftItemID==cartDetail.GiftItemID))
        {
           var cd=this.producerContext.CartDetails.Where(x=>x.ID==cartDetail.ID && x.GiftItemID==cartDetail.GiftItemID).FirstOrDefault();
           cd.Units=cartDetail.Units;
           cd.BusinessDay=cartDetail.BusinessDay;
        }
        else{
  this.producerContext.CartDetails.Add(cartDetail);
        }
      
        this.producerContext.SaveChanges();
    }

    public void Create(GiftItem item)
    {
        this.producerContext.GiftItems.Add(item);
        //this.producerContext.GiftItemDetails.Add(item.giftItemDetails);
        this.producerContext.SaveChanges();
    }

    public void DeleteGiftItem(int id)
    {
        GiftItem item=this.producerContext.GiftItems.Where(x=>x.Id==id).FirstOrDefault();
        GiftItemDetail detail=this.producerContext.GiftItemDetails.Where(x=>x.GiftItemId==id).FirstOrDefault();
        this.producerContext.GiftItemDetails.Remove(detail);
        this.producerContext.GiftItems.Remove(item);
        this.producerContext.SaveChanges();
    }

    public List<CartDetail> GetCardEntry(string? name)
    {
        return this.producerContext.CartDetails.Where(x=>x.UserId==name).ToList<CartDetail>();
    }

    public List<CartEntryGiftItemDetail> GetCardGiftItemDetail(string? value)
    {
        var result= from card in producerContext.CartDetails
        join giftItem in producerContext.GiftItems
        on card.GiftItemID equals giftItem.Id
        join giftItemDetail in producerContext.GiftItemDetails
        on giftItem.Id equals giftItemDetail.GiftItemId
        where card.UserId==value
        select new CartEntryGiftItemDetail{
             GiftItemName=giftItem.Name,
             Price=giftItemDetail.SalePrice,
             Photo=giftItem.Photo,
             Id=card.ID,
             Units=card.Units,
             CostPerUnit=card.CostPerUnit,
             UserId=card.UserId,
             GiftItemID=giftItem.Id
        };
        return result.ToList<CartEntryGiftItemDetail>();
    }

    public GiftItem? GetGiftItemDetail(int id)
    {
        var result=from g in producerContext.GiftItems
        join gd in producerContext.GiftItemDetails
        on g.Id equals gd.GiftItemId
        where g.Id==id
        select new GiftItem {
            Name=g.Name,
            InStock=g.InStock,
            Photo=g.Photo,
            Id=g.Id,
            giftItemDetails=new GiftItemDetail{
                Manufacturer=gd.Manufacturer,
                GiftItemId=gd.GiftItemId,
                GiftItemDetailID=gd.GiftItemDetailID,
                Price=gd.Price,
                SalePrice=gd.SalePrice
            }
        };
        return result.FirstOrDefault();
    }

    public IEnumerable<GiftItem> GetGiftItems()
    {
        return producerContext.GiftItems.ToList();
    }

    public List<OrderDetailViewModel> GetOrderDetails(int orderId)
    {
        var result= from od in producerContext.OrderDetails
               join gi in producerContext.GiftItems
               on od.GiftItemId equals gi.Id
               where od.OrderID==orderId
               select new OrderDetailViewModel{
                GiftItemName=gi.Name,
                CostPerUnit=od.CostPerUnit,
                Units=od.Units,
                OrderID=od.OrderID
               };
               return result.ToList<OrderDetailViewModel>();
    }

    public List<Order> GetOrders(string userName)
    {
        return this.producerContext.Orders.Where(x=>x.UserId==userName).ToList();
    }

    public void PlaceOrder(Order order)
    {
        producerContext.Orders.Add(order);
        producerContext.SaveChanges();
    }

    public void RemoveCart(string userName)
    {

        producerContext.CartDetails.RemoveRange(producerContext.CartDetails.Where(x=>x.UserId==userName));
        producerContext.SaveChanges();

    }

    public void UpdateGiftItem(GiftItem model)
    {
         
        GiftItem detail=producerContext.GiftItems.Include(x=>x.giftItemDetails).Where(x=>x.Id==model.Id).FirstOrDefault();
        if(detail!=null)
        {
           detail.giftItemDetails.Price=model.giftItemDetails.Price;
           detail.giftItemDetails.SalePrice=model.giftItemDetails.SalePrice;
           detail.giftItemDetails.Manufacturer=model.giftItemDetails.Manufacturer;
           detail.Name=model.Name;
           detail.InStock=model.InStock;
           detail.Photo=model.Photo;
           producerContext.SaveChanges();
        }
        
    }
}