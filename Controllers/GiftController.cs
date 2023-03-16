using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
public class GiftController : Controller
{
  private readonly IGiftItemService giftItemService;
  private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
  private readonly SignInManager<IdentityUser> signInManger;
  private readonly UserManager<IdentityUser> userManager;
    public GiftController(IGiftItemService giftItemService,UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
  {
    this.giftItemService=giftItemService;
    this.hostingEnvironment=hostingEnvironment;
    this.signInManger=signInManager;
    this.userManager=userManager;
  }
  [HttpGet]
  public IActionResult Create()
  {

     return View();
    
  }
  [Authorize(Roles="Admin")]
  [HttpPost]
  public IActionResult Create(GiftItemViewModel item)
  {
    var isExisitingItem=this.giftItemService.GetGiftItems().Any(x=>x.Id==item.Id);
    string uniqueFileName=null;
    if(!isExisitingItem)
    {
      if (item.Photo != null)
                {
                    
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                   
                   uniqueFileName = Guid.NewGuid().ToString() + "_" + item.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    item.Photo.CopyTo(new FileStream(filePath, FileMode.Create,FileAccess.ReadWrite));    
                }
                GiftItemDetail giftItemDetail=new GiftItemDetail{
                  Price=item.giftItemDetails.Price,
                  Manufacturer=item.giftItemDetails.Manufacturer,
                  SalePrice=(item.giftItemDetails.Price*10/100)+item.giftItemDetails.Price
                };
               GiftItem giftItem=new GiftItem{
                   Name=item.Name,
                   Photo=uniqueFileName,
                   InStock=item.InStock,
                   giftItemDetails = giftItemDetail
               }; 
      this.giftItemService.Create(giftItem);
    }
    return Redirect("/Gift/GetGiftItems");
  }
   [AllowAnonymous]
  [HttpGet]
   public ActionResult<IEnumerable<GiftItem>> GetGiftItems()
   {
     return View("Gift",this.giftItemService.GetGiftItems().ToList()); 
   }
  [Authorize]
   [HttpGet]
   public IActionResult UpdateGiftItem(int id)
   { 
    var item=this.giftItemService.GetGiftItemDetail(id);
    if(item!=null)
    {
GiftItemDetailViewModel detailViewModel=new GiftItemDetailViewModel{
           Price=item.giftItemDetails.Price,
           Manufacturer=item.giftItemDetails.Manufacturer,
           GiftItemId=item.giftItemDetails.GiftItemId,
           GiftItemDetailID=item.giftItemDetails.GiftItemDetailID,
           SalePrice=item.giftItemDetails.SalePrice
    };
    GiftItemViewModel viewModel=new GiftItemViewModel{
      Id=item.Id,
      Name=item.Name,
      ExistingPhoto=item.Photo,
      InStock=item.InStock,
      giftItemDetails=detailViewModel
    };
        return View("UpdateGiftItem",viewModel);
    }
    return NotFound();
   }
   [Authorize]
   [HttpPost]
   public IActionResult UpdateGiftItem(GiftItemViewModel model)
   {
    if(model!=null)
    {
      bool isExisitingItem=this.giftItemService.GetGiftItems().Any(x=>x.Id==model.giftItemDetails.GiftItemId);
      if(isExisitingItem)
      { 
        string uniqueFileName=null;
        if (model.Photo != null)
                {
                    
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                   
                   uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    
                }
        GiftItemDetail giftItemDetail=new GiftItemDetail{
           GiftItemId=model.giftItemDetails.GiftItemId,
           GiftItemDetailID=model.giftItemDetails.GiftItemDetailID,
           Manufacturer=model.giftItemDetails.Manufacturer,
           Price=model.giftItemDetails.Price,
          SalePrice=(model.giftItemDetails.Price*10/100)+model.giftItemDetails.Price
        };
        GiftItem giftItem=new GiftItem{
            Name=model.Name,
            Photo=uniqueFileName,
            InStock=model.InStock,
            Id=model.giftItemDetails.GiftItemId,
            giftItemDetails=giftItemDetail

        };
        this.giftItemService.UpdateGiftItem(giftItem);
      }
    }
    return Redirect("/Gift/GetGiftItems");
   }
   [HttpPost]
   public IActionResult DeleteGiftItem(int id)
   {
    GiftItem item=this.giftItemService.GetGiftItems().Where(x=>x.Id==id).FirstOrDefault();
    if(item!=null)
    {
      this.giftItemService.DeleteGiftItem(id);
    }
    return Redirect("/Gift/GetGiftItems");
   }
   [HttpGet]
   public IActionResult GetGiftItemDetail(int id,int quantity)
   {
    var item=this.giftItemService.GetGiftItemDetail(id);
    List<CartDetail> existingCardEntry=this.giftItemService.GetCardEntry(User.Identity.Name);
    if(item!=null)
    {
GiftItemDetailViewModel detailViewModel=new GiftItemDetailViewModel{
           Price=item.giftItemDetails.Price,
           SalePrice=item.giftItemDetails.SalePrice,
           Manufacturer=item.giftItemDetails.Manufacturer,
           GiftItemId=item.giftItemDetails.GiftItemId,
           GiftItemDetailID=item.giftItemDetails.GiftItemDetailID
    };
    GiftItemViewModel viewModel=new GiftItemViewModel{
      Id=item.Id,
      Name=item.Name,
      ExistingPhoto=item.Photo,
      InStock=item.InStock,
      giftItemDetails=detailViewModel
    };
    var quan=0;
    quan=(int)((existingCardEntry!=null && existingCardEntry.FindAll(x=>x.GiftItemID==viewModel.Id).Count()>0)?
      existingCardEntry.Where(x=>x.GiftItemID==viewModel.Id).FirstOrDefault().Units:0);
    if(quantity>0)
    {
      quan=quantity;
    }
    DetailViewModel vm=new DetailViewModel{
      giftItem=viewModel,
      Quantity=quan
    };
        return View("Detail",vm);
    }
    return NotFound();
   }

  [HttpPost]
   public  IActionResult AddToCart(int id,int quantity)
   {
   CartDetail cartDetail=null;
    var giftItem=this.giftItemService.GetGiftItemDetail(id);
    var existingCardEntry=this.giftItemService.GetCardEntry(User.Identity.Name);
    var existinggiftItemInCart=(existingCardEntry!=null && existingCardEntry.FindAll(x=>x.GiftItemID==id).Count()>0)?existingCardEntry.Where(x=>x.GiftItemID==id).FirstOrDefault():null;
    if(giftItem!=null )
    {
       if(existinggiftItemInCart!=null)
       {
        existinggiftItemInCart.Units=quantity;
        cartDetail=existinggiftItemInCart;
       }
       else{
cartDetail=new CartDetail{
        UserId= User.Identity.Name,
        BusinessDay=DateTime.Now,
        GiftItemID=giftItem.Id,
        Units=quantity==0?1:quantity,
        CostPerUnit=giftItem.giftItemDetails.SalePrice
       };
       }
       
       this.giftItemService.AddToCart(cartDetail);
    }

     return Redirect(String.Format("/Gift/GetGiftItemDetail/{0}",id));
   }

   public IActionResult AddQuantity(int id,int quantity)
   {
quantity=quantity+1;
return Redirect(String.Format("/Gift/GetGiftItemDetail/{0}/{1}",id,quantity));
//return Redirect(String.Format("/Gift/AddToCart/{0}?quantity={1}",id,quantity)) ;    
   }
   public IActionResult DeductQuantity(int id,int quantity)
   {
     if(quantity>0)
     {
     quantity=quantity-1;
     }
     return Redirect(String.Format("/Gift/GetGiftItemDetail/{0}/{1}",id,quantity));
     // RedirectToAction("AddToCart",new {param=new {id=id,quantity=quantity}}); ;    
   }
}

