using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //para dizermos q eh um metodo post, se fosse get n precisaria colocar
        [ValidateAntiForgeryToken] //metodo de segurança p q n usem o token de validacao para
        //enviar dados maliciosos
        public IActionResult Create(Seller obj)
        {
            //inserimos o obj
            _sellerService.Insert(obj);
            //redirecionamos para a acao index
            //obs: poderiamos colocar Red...("Index"), porem com o nameof, se 
            //mudarmos 
            return RedirectToAction(nameof(Index));
        }
    }
}
