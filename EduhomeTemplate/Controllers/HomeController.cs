using EduhomeTemplate.Models;
using EduhomeTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduhomeTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel
            {
                Courses = _context.Courses.ToList(),
                Boards = _context.Boards.ToList()
            };
            return View(homeVM);
        }

        public IActionResult GetPartial(int id)
        {
            Course Course = _context.Courses.FirstOrDefault(x => x.Id == id);
            return PartialView("_CoursePartialView", Course);
        }

        public ActionResult AddBasket(int courseId)
        {
            if (!_context.Courses.Any(x => x.Id == courseId))
            {
                return NotFound();
            }

            List<BasketItemViewModal> basketItems = new List<BasketItemViewModal>();

            string existBasketItem = HttpContext.Request.Cookies["basketItemList"];

            if (existBasketItem != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModal>>(existBasketItem);
            }
            BasketItemViewModal item = basketItems.FirstOrDefault(x => x.CourseId == courseId);
            if (item == null)
            {
                item = new BasketItemViewModal
                {
                    CourseId = courseId,
                    Count = 1
                };
                basketItems.Add(item);
            }
            else
            {
                item.Count++;
            }
            var bookIdStr = JsonConvert.SerializeObject(basketItems);
            HttpContext.Response.Cookies.Append("basketItemList", bookIdStr);
            return Ok();
        }

        public IActionResult ShowBasket()
        {
            var bookIdStr = HttpContext.Request.Cookies["basketItemList"];
            List<BasketItemViewModal> basketItem = new List<BasketItemViewModal>();

            if (bookIdStr != null)
            {
                basketItem = JsonConvert.DeserializeObject<List<BasketItemViewModal>>(bookIdStr);
            }
            return Json(basketItem);
        }
    }
}
