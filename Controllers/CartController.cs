using Microsoft.AspNetCore.Mvc;

public class CartController : Controller{
    private readonly IGiftItemService giftItemService;
    public CartController(IGiftItemService giftItemService)
    {
       this.giftItemService=giftItemService;
    }
    public IActionResult GetCartDetails()
    {
        var cartData=this.giftItemService.GetCardGiftItemDetail(User.Identity.Name);
        
        return View("Cart",cartData);
    }
    [HttpPost]
    public IActionResult PlaceOrder(string userName)
    {
        this.giftItemService.PlaceOrder(userName);
        return Redirect("/Cart/GetCartDetails");
    }
}