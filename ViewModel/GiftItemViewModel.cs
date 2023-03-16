public class GiftItemViewModel{
    public int Id{get;set;}
    public string? Name{get;set;}
    public bool InStock{get;set;}

    public IFormFile Photo{get;set;}
    public string ExistingPhoto{get;set;}
    public GiftItemDetailViewModel? giftItemDetails{get;set;}
}