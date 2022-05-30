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
    }
}
