using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class EntidadClientes
    {
        public class Customer
        {
            [Key]
            public int IdCliente { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string RFC { get; set; }
            public string Direccion { get; set; }
            public string CP { get; set; }

            public string Email { get; set; }

            public int Estado { get; set; }
        }

        public class Transaction
        {
            [Key]
            public int IdTransaction { get; set; }
            public int IdCliente { get; set; }
            public int Cantidad { get; set;}
            public string Descripcion { get; set; }
            public string PrecioUnitario { get; set;}
            public decimal Total { get; set; }
            public DateTime FechaRegistro { get; set; }

            public virtual Customer Customers { get; set; }

        }
    }
}