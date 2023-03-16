using Microsoft.AspNetCore.Identity;
public class Order{
    public string UserId{get;set;}
    public int OrderID{get;set;}
    public decimal NumberOfItemsOrder{get;set;}

    public decimal TotalAmount{get;set;}

    public DateTime BusinessDay{get;set;}
    public ICollection<OrderDetail> OrderDetails{get;set;}
}