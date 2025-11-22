using AlamniLMS.BLL.Services.Interfacese;
using AlamniLMS.DAL.Repository.Classes;
using AlamniLMS.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Stripe.Checkout;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionLineItemOptions = Stripe.Checkout.SessionLineItemOptions;
using SessionLineItemPriceDataOptions = Stripe.Checkout.SessionLineItemPriceDataOptions;
using SessionLineItemPriceDataProductDataOptions = Stripe.Checkout.SessionLineItemPriceDataProductDataOptions;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using AlamniLMS.DAL.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using AlamniLMS.DAL.DTO.Responses;
using AlamniLMS.DAL.DTO.Requests;
using Order = AlamniLMS.DAL.Models.Order;


namespace AlamniLMS.BLL.Services.Classes
{
    public class CheckOutService : ICheckOutService    
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        //private readonly IProductRepository _productRepository;

        public CheckOutService(IEnrollmentRepository enrollmentRepository,
                               IOrderRepository orderRepository,
                               IEmailSender emailSender,
                               IOrderItemRepository orderItemRepository
                               //IProductRepository productRepository


                                )
        {
            _enrollmentRepository = enrollmentRepository;
            _orderRepository = orderRepository;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
            //_productRepository = productRepository;
        }

        public async Task<bool> HandlePaymentSuccessAsync(int orderId)
        {
            var order = await _orderRepository.GetUserByOrderAsync(orderId);

            var subject = "";
            var body = "";
            if (order.PaymentMethod == PaymentMethodEnum.Visa)
            {

                order.Status = OrderStatusEnum.Delivered;
                var carts = await _enrollmentRepository.GetUserEnrollmentAsync(order.UserId);
                var orderItems = new List<OrderItem>();
               // var productUpdate = new List<(int productId, int quantity)>();
                foreach (var cartItem in carts)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        CourseId = cartItem.CourseId,
                        totalPrice = cartItem.Course.Price * cartItem.Count,
                        Price = cartItem.Course.Price,
                        Count = cartItem.Count
                    };
                    orderItems.Add(orderItem);
                    //productUpdate.Add((cartItem.ProductId, cartItem.Count));
                }

                await _orderItemRepository.AddRangeAsync(orderItems);
                await _enrollmentRepository.ClearEnrollmentAsync(order.UserId);
                //await _productRepository.DecreaseQuantityAsync(productUpdate);




                subject = "Payment Successful - Alamni LMS";
                body = $"<h1>thank you for your payment</h1>" +
                    $"<p>your payment for order {orderId}</p>" +
                    $"<p>total Amount : ${order.TotalAmount}";
            }
            else if (order.PaymentMethod == PaymentMethodEnum.Cash)
            {
                subject = "order placed successfully";
                body = $"<h1>thank you for your order</h1>" +
                    $"<p>your payment for order {orderId}</p>" +
                    $"<p>total Amount : ${order.TotalAmount}";
            }

            await _emailSender.SendEmailAsync(order.User.Email, subject, body);
            return true;


        }

        public async Task<CheckOutResponse> ProcessPaymentAsync(CheckOutRequest request, string UserId, HttpRequest httpRequest)
        {
            // الحصول على محتويات سلة المستخدم GetUserEnrollment
            var cartItem = await _enrollmentRepository.GetUserEnrollmentAsync(UserId);

            // التحقق من أن السلة ليست فارغة
            if (!cartItem.Any())
            {
                return new CheckOutResponse
                {
                    Success = false,
                    Message = "Enrollment is empty."
                };
            }

            // Order 
            Order order = new Order
            {
                UserId = UserId,
                PaymentMethod = request.PaymentMethod,
                //PaymentMethodEnum.Cash,
                TotalAmount = cartItem.Sum(ci => ci.Course.Price * ci.Count),
                //Status = OrderStatusEnum.Pending,
                //    //OrderDate = DateTime.Now
            };
            await _orderRepository.AddAsync(order);

            // معالجة الدفع بناءً على طريقة الدفع المحددة


            if (request.PaymentMethod == PaymentMethodEnum.Cash)

               // if (request.PaymentMethod == "Cash")
            {
                // يمكن إضافة منطق معالجة الدفع النقدي هنا، مثل تحديث حالة الطلب في قاعدة البيانات.
                return new CheckOutResponse
                {
                    Success = true,
                    Message = "Payment will be collected .Cash."
                };

            }


            if (request.PaymentMethod == PaymentMethodEnum.Visa)

            {
                var options = new SessionCreateOptions
                {
                    // إنشاء قائمة عناصر الدفع (LineItems)
                    PaymentMethodTypes = new List<string> { "card" },
                    // ملء القائمة بعناصر سلة التسوق
                    LineItems = new List<SessionLineItemOptions>
                    {

                    },
                    Mode = "payment",
                    SuccessUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Customer/CheckOuts/Success/{order.Id}",
                   // SuccessUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Customer/CheckOuts/Success",
                    CancelUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/checkout/cancel",
                };

                foreach (var item in cartItem)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Course.Title,
                                Description = item.Course.FullDescription,
                            },
                            UnitAmount = (long)(item.Course.Price * 100),
                        },
                        Quantity = item.Count,
                    });
                }
                var service = new SessionService();
                var session = service.Create(options);

                order.PaymentId = session.Id;

                return new CheckOutResponse
                {
                    Success = true,
                    Message = "Payment processed successfully.",
                    PaymentId = session.Id,
                    Url = session.Url,

                };


            }

            return new CheckOutResponse
            {
                Success = false,
                Message = "Invalid payment method."
            };
        }

     
    }
}
