using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Diagnostics;

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
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }
                return View(viewModel); //se o js for desabilitado e n puder fazer a validacao, ele manda voltar e terminar essa validacao antes de continuar retornando a mesma view
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
                return RedirectToAction(nameof(Error), new { message = "Id not provided"});
            }

            var obj = _sellerService.FindById(id.Value); //pra pegar o valor dele caso existe, por ser opcional
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
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
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value); //pra pegar o valor dele caso existe, por ser opcional
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" }); ;
            }

            return View(obj);
        }

        public IActionResult Edit(int? id) //esse ? ao lado do int é só pra nao dar erro de execucao, mas ele eh obrigatorio
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); //se o js for desabilitado e n puder fazer a validacao, ele manda voltar e terminar essa validacao antes de continuar retornando a mesma view
            }
            if(id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e) //o App eh um super das excecoes personalizadas
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }
        }

        public IActionResult Error(string message) //justamente para retornar a view Error.cshtml
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //serve para pegarmos o id interno da requisicao
                                                                                //?? significa que se for nulo, ira substituir pelo HttpContext
            };
            return View(viewModel);
        }
    }
}
