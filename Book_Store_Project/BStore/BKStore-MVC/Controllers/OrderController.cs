using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BKStore_MVC.Controllers
{
    public class OrderController : Controller
    {
        IOrderBookRepository orderBookRepository;
        IDeliveryClientRepository deliveryClientRepository;
        IOrderRepository orderRepository;
        ICustomerRepository customerRepository;
        IBookRepository bookRepository;
        IGovernorateRepository governorateRepository;
        IShippingRepository shippingRepository;
        IShippingMethodRepository shippingMethodRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public OrderController(SignInManager<ApplicationUser> signInManager
            , IOrderRepository orderRepository,IShippingRepository shippingRepository,
            ICustomerRepository customerRepository, IBookRepository bookRepository,
            IOrderBookRepository orderBookRepository, IDeliveryClientRepository deliveryClientRepository
            , IGovernorateRepository governorateRepository, IShippingMethodRepository shippingMethodRepository)
        {
            this.orderBookRepository = orderBookRepository;
            this.deliveryClientRepository = deliveryClientRepository;
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.bookRepository = bookRepository;
            this.governorateRepository = governorateRepository;
            _signInManager = signInManager;
            this.shippingRepository = shippingRepository;
            this.shippingMethodRepository = shippingMethodRepository;
        }
        [Authorize(Roles = "Delivery, Admin")]
        public IActionResult GetAll()
        {
            return View("GetAll", orderRepository.GetAll());
        }
        public IActionResult GetAllByCustomerID()
        {
            return View("GetAll", orderRepository.GetByCustomersID(int.Parse(GetCustomerID())));
        }
        public IActionResult DetailedOrder(int OrderId)
        {
            List<OrderBook> orderBook= orderBookRepository.GetByID(OrderId);
            List<BookCartItem> bookCartItems= new List<BookCartItem>();
            OrderDetailVM orderDetailVM = new OrderDetailVM();
            if (orderBook != null)
            {
                foreach (var item in orderBook.ToList())
                {
                    BookCartItem bookCart = new BookCartItem();
                    bookCart.Title = bookRepository.GetByID(item.BookID??0).Title;
                    bookCart.Quantity = item.Quantity;
                    bookCart.Price = bookRepository.GetByID(item.BookID ?? 0).Price;
                    bookCart.ImagePath = bookRepository.GetByID(item.BookID ?? 0).ImagePath;
                    bookCart.BookId = item.BookID;
                    
                    bookCartItems.Add(bookCart);
                }
            }

            orderDetailVM.bookCartItems = bookCartItems ;
            orderDetailVM.CustomerName = customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID ?? 0).Name;
            orderDetailVM.TotalPrice = orderRepository.GetByID(OrderId).TotalAmount ?? 0;
            orderDetailVM.CustomerAddress = customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID ?? 0).Address;
            orderDetailVM.Governorate = governorateRepository.GetByID(customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID ?? 0).GovernorateID ?? 0).Name;
            orderDetailVM.CustomerID = orderRepository.GetByID(OrderId).CustomerID;
            orderDetailVM.OrderID = OrderId;
            orderDetailVM.PaymentFees = shippingMethodRepository.GetByID(1).PaymentFees;
            return View("DetailedOrder", orderDetailVM);
        }
        public IActionResult DetailedOrderForUser()
        {
            int OrderId;
            string customerID = GetCustomerID();

            if (string.IsNullOrEmpty(customerID))
            {
                return BadRequest("Customer ID cannot be null or empty.");
            }

            OrderId = orderRepository.GetByCustomerID(int.Parse(customerID)).OrderId;

            List<OrderBook> orderBook = orderBookRepository.GetByID(OrderId);
            List<BookCartItem> bookCartItems = new List<BookCartItem>();
            OrderDetailVM orderDetailVM = new OrderDetailVM();

            if (orderBook != null)
            {
                foreach (var item in orderBook)
                {
                    var book = bookRepository.GetByID(item.BookID ?? 0);
                    BookCartItem bookCart = new BookCartItem
                    {
                        Title = book.Title,
                        Quantity = item.Quantity,
                        Price = book.Price,
                        ImagePath = book.ImagePath,
                        BookId = item.BookID
                    };

                    bookCartItems.Add(bookCart);
                  
                }
                //orderDetailVM.bookCartItems = bookCartItems;
            }
            
            var order = orderRepository.GetByID(OrderId);
            var customer = customerRepository.GetByID(order.CustomerID ?? 0);
            var governorate = governorateRepository.GetByID(customer.GovernorateID ?? 0);

            orderDetailVM.bookCartItems = bookCartItems;
            orderDetailVM.CustomerName = customer.Name;
            orderDetailVM.TotalPrice = order.TotalAmount ?? 0;
            orderDetailVM.CustomerAddress = customer.Address;
            orderDetailVM.Governorate = governorate.Name;
            orderDetailVM.CustomerID = order.CustomerID;

            return View("DetailedOrder", orderDetailVM);
        }
        private string GetCustomerID()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    return customerRepository.GetByUserID(userId).ID.ToString();
                }
            }

            var customerIDCookie = Request.Cookies["CustomerID"];
            if (customerIDCookie != null)
            {
                return JsonConvert.DeserializeObject<string>(customerIDCookie);
            }

            return string.Empty;
        }
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> DeliverOrder(int orderID)
        {

            Order order = orderRepository.GetByID(orderID);
            order.DelivaryStatus = "Delivering";
            var cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            if (cookie != null)
            {
                var ticket = await _signInManager.Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
                if (ticket != null)
                {
                    var userId = ticket.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    order.DeliveryClientsID = deliveryClientRepository.GetByUserID(userId).ID;
                    orderRepository.Update(order);
                    orderRepository.Save();
                    return RedirectToAction("GetAll");
                }

            }
            //order.DeliveryClientsID = deliveryClientRepository.GetByUserID(userIdCookie).ID;
            //orderRepository.Update(order);
            //orderRepository.Save();
            //return View("GetAll", orderRepository.GetAll());
            return Content("Error");
        }
        [HttpPost]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> UpdateStatus(int orderID)
        {
            var cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            if (cookie != null)
            {
                var ticket = await _signInManager.Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
                if (ticket != null)
                {
                    var userId = ticket.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    Order UpdateOrder = orderRepository.GetBydeliveryID(deliveryClientRepository.GetByUserID(userId).ID, orderID);
                    if (UpdateOrder != null)
                    {
                        UpdateOrder.DelivaryStatus = "Delivered";
                        orderRepository.Update(UpdateOrder);
                        orderRepository.Save();
                        return Json(new { success = true });
                    }

                }
            }
            return Json(new { success = false });
        }

    }
}
