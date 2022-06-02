using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); //busca do db todos os departamentos
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
            //agr a nossa tela ja vai receber o department quando cadastrar a 1x
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

        public IActionResult Delete(int? id) //int? = opcional
        {
            if(id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); //pra pegar o valor dele caso existe, por ser opcional
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); //pra pegar o valor dele caso existe, por ser opcional
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }
}
