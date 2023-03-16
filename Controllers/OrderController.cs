using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller {
    private readonly IGiftItemService giftItemService;
    public OrderController(IGiftItemService giftItemService){
       this.giftItemService=giftItemService;
    }
    public IActionResult GetOrder(){
        var Orders=this.giftItemService.GetOrders(User.Identity.Name);
        return View("Order",Orders);
    }
    [HttpGet]
    public IActionResult Details(int id){
        var OrderDetails=this.giftItemService.GetOrderDetails(id);
        return View("Detail",OrderDetails);
    }
    
}