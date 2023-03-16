using System.ComponentModel.DataAnnotations;
public class GiftItemDetail{
    public int GiftItemId{get;set;}
    public int GiftItemDetailID{get;set;}
    public decimal Price{get;set;}

    public decimal SalePrice{get;set;}
    public string? Manufacturer{get;set;}
}