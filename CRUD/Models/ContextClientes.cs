using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using static CRUD.Models.EntidadClientes;


namespace CRUD.Models
{
    public class ContextClientes : DbContext
    {
        public ContextClientes() : base("MiConex") { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}