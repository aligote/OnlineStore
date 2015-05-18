﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineStore.Web.OrderService;
using OnlineStore.Web.ProductService;
// ReSharper disable PossibleNullReferenceException

namespace OnlineStore.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Protected Properties
        protected Guid UserId
        {
            get
            {
                if (Session["UserId"] != null)
                {
                    return (Guid) Session["UserId"];
                }
                else
                {
                    var id = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    Session["UserId"] = id;
                    return id;
                }
            }
        }
        #endregion 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(string categoryId = null)
        {
            ViewData["CategoryId"] = categoryId;
            
            return View();
        }

        public ActionResult ProductDetail(string id)
        {
            using (var proxy = new ProductServiceClient())
            {
                var product = proxy.GetProductById(new Guid(id));
                return View(product);
            }
        }

        [Authorize]
        public ActionResult AddToCart(string productId, string items)
        {
            using (var proxy = new OrderServiceClient())
            {
                int quantity = 0;
                if (!int.TryParse(items, out quantity))
                    quantity = 1;
                proxy.AddProductToCart(UserId, new Guid(productId), quantity);
                return RedirectToAction("ShoppingCart");
            }
        }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            using (var proxy = new OrderServiceClient())
            {
                var model = proxy.GetShoppingCart(UserId);
                return View(model);
            }
        }

        [Authorize]
        public ActionResult UpdateShoppingCartItem(string shoppingCartItemId, int quantity)
        {
            using (var proxy = new OrderServiceClient())
            {
                proxy.UpdateShoppingCartItem(new Guid(shoppingCartItemId), quantity);
                return Json(null);
            }
        }

        [Authorize]
        public ActionResult DeleteShoppingCartItem(string shoppingCartItemId)
        {
            using (var proxy = new OrderServiceClient())
            {
                proxy.DeleteShoppingCartItem(new Guid(shoppingCartItemId));
                return Json(null);
            }
        }

        [Authorize]
        public ActionResult Checkout()
        {
            using (var proxy = new OrderServiceClient())
            {
                var model = proxy.Checkout(this.UserId);
                return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SuccessPage(string pageTitle, string pageMessage = null, string retAction = "Index", string retController = "Home", int waitSeconds = 5)
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.PageMessage = pageMessage;
            ViewBag.RetAction = retAction;
            ViewBag.RetController = retController;
            ViewBag.WaitSeconds = waitSeconds;
            return View();
        }
    }
}
