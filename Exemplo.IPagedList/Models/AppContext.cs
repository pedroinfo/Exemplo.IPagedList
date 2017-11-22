using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exemplo.IPagedList.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext():base("DefaultConnection")
        {

        }
        public DbSet<Pessoa> Pessoas { get; set; }
    }
}