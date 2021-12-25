using EduhomeTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduhomeTemplate.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BoardController : Controller
    {
        private DataContext _context;

        public BoardController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Boards.ToList());

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Board board)
        {
            _context.Boards.Add(board);
            _context.SaveChanges();


            return RedirectToAction("index", "board");
        }

        public IActionResult Edit(int id)
        {
            Board board = _context.Boards.FirstOrDefault(x => x.Id == id);
            return View(board);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Board board)
        {
            Board existsboard = _context.Boards.FirstOrDefault(x => x.Id == board.Id);
            if (existsboard == null) return View();
            if (!ModelState.IsValid) return View();

            existsboard.Date = board.Date;
            existsboard.Desc = board.Desc;

            _context.SaveChanges();

            return RedirectToAction("index", "board");
        }

        public IActionResult Delete(int id)
        {
            Board board = _context.Boards.FirstOrDefault(x => x.Id == id);
            if (board == null) return View();

            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Board board)
        {
            Board existsboard = _context.Boards.FirstOrDefault(x => x.Id == board.Id);
            if (existsboard == null) return View();

            _context.Boards.Remove(existsboard);
            _context.SaveChanges();

            return RedirectToAction("index", "board");

        }


    }
}
