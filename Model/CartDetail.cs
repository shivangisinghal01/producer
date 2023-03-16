using Microsoft.AspNetCore.Identity;

public class CartDetail{
public int ID{get;set;}
public string UserId{get;set;}
    public DateTime BusinessDay{get;set;}
    public int GiftItemID{get;set;}
    public decimal Units{get;set;}
    public decimal CostPerUnit{get;set;}
 
}
