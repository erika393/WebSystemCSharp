using System;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        //readonly faz com que nao seja alterado
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            //pega todos os sellers e transforma em uma list
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            //obj.Department = _context.Department.First();
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
    }
}
