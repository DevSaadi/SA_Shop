
using Book.DataAccess.Repository;
using Book.DataAccess.Repository.IRepository;
using Book.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

   

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperty: "Category,CoverType");

            return View(productList);
        }
		public IActionResult Detail(int productId)
		{
            ShoppingCart cartobj = new()
            {
                ProductId= productId,
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperty: "Category,CoverType")
            };
			return View(cartobj);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  //making sure only login can use
        public IActionResult Detail(ShoppingCart shoppingCart)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);  //extract user identity
            shoppingCart.ApplicationUserId = claim.Value; //check if user is null
            ShoppingCart cartformdb=_unitOfWork.ShoppingCart.GetFirstOrDefault(
                u=>u.ApplicationUserId==claim.Value && u.ProductId==shoppingCart.ProductId);
           
            if(cartformdb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
				_unitOfWork.ShoppingCart.IncrementCount(cartformdb, shoppingCart.Count);
			}

          
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
