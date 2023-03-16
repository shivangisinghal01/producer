public class GiftItem{
    public int Id{get;set;}
    public string? Name{get;set;}
    public bool InStock{get;set;}
    public string? Photo{get;set;}
    public GiftItemDetail? giftItemDetails{get;set;}
}