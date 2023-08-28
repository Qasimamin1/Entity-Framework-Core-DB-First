using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDbContext _context;
        // we can communicate with the database
        public ProductRepository(MyAppDbContext context)
        {

            _context = context;

        }
        //read the data from the DB
        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task Update(Product model)
        {
          //check product is available on db table
          var product = await _context.Products.FindAsync(model.Id);
            if(product != null) {
                //if model is available then it assign product model properties

                //These lines update the properties of the found product with the properties from the provided model.
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Qty = model.Qty;

                _context.Update(product);
               //commit the changes to the db
                await Save();

            }
        }

        public async Task Add(Product model)
        {
            //receiving model 
            await _context.Products.AddAsync(model);
            Save();
        }

      

        public async Task Delete(int id)
        {
            // check id is valid or not check product table 
            var product = await _context.Products.FindAsync(id);
            if(product != null) { 
               _context.Products.Remove(product);
                await Save();
            }
        }

        private async Task Save()
        {
            _context.SaveChangesAsync();
        }
    }
}
