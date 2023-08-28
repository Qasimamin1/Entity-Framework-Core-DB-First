using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{ 
    public class MyAppDbContext: DbContext
    {
        public MyAppDbContext(DbContextOptions options) : base(options)
        {




        }
        //create a table that link with sql server 
        public DbSet<Product> Products { get; set; }

    } 
}
