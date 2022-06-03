using System;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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

        //public List<Seller> FindAll()
        //{
        //    //pega todos os sellers e transforma em uma list
        //    return _context.Seller.ToList();
        //}

        //assinc operation
        public async Task<List<Seller>> FindAllAsync()
        {
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return await _context.Seller.ToListAsync();

        }

        //public void Insert(Seller obj)
        //{
        //    _context.Add(obj);
        //    _context.SaveChanges();
        //}

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
           await _context.SaveChangesAsync();
        }

        //public Seller FindById(int id)
        //{
        //    //add esse Include (parte do Microsoft Entity Framework) pq se vermos os detalhes do Seller nao aparecera o Department
        //    //pois o Department eh outro obj e nao um Seller, para isso temos que fazer o Include
        //    return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        //}

        public async Task<Seller> FindByIdAsync(int id)
        {
            //add esse Include (parte do Microsoft Entity Framework) pq se vermos os detalhes do Seller nao aparecera o Department
            //pois o Department eh outro obj e nao um Seller, para isso temos que fazer o Include
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        //public void Remove(int id)
        //{
        //    var obj = _context.Seller.Find(id);
        //    _context.Seller.Remove(obj);
        //    _context.SaveChanges();
        //}

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            //o any serve pra falar se existe algum dado na condicao colocada
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found"); //excecao de nivel de servico
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message); //excecao de nivel de acesso a dados
                //Aqui eh capturado a excecao em nivel de acesso a dados e relancada em nivel de servico
            }

        }
    }
}
