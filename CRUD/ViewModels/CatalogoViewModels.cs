using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CRUD.Models.EntidadClientes;

namespace CRUD.ViewModels
{
    public class CatalogoViewModels
    {
        public class Catalogo
        {
            public Catalogo() 
            {
                ListaCliente = new List<Customer>();
            }
            public List<Customer> ListaCliente { get; set; }
        }

        //Validaciones para el formulario de crear cliente
        public class CrearCliente
        {
            public CrearCliente()
            {
                ListaCliente = new List<Customer>();
            }

            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int IdCliente { get; set; }

            [Required(ErrorMessage = "Campo Requerido")]
            public string Nombre { get; set; }

            public string Apellidos { get; set; }

            [Required(ErrorMessage = "Campo Requerido")]
            public string RFC { get; set; }

            [Required(ErrorMessage = "Campo Requerido")]
            public string Direccion { get; set; }

            public string CP { get; set; }

            [Required(ErrorMessage = "Campo Requerido")]
            [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Email inválido")]
            public string Email { get; set; }

            public int Estado { get; set; }

            public List<Customer> ListaCliente { get; private set; }
        }

        public class TransactionsVM
        {
            public TransactionsVM()
            {
                transactions = new List<Transaction>();
            }
            
            public int IdTransaction { get; set; }
            public int IdCliente { get; set; }
            public int Cantidad { get; set; }
            public string Descripcion { get; set; }
            public string PrecioUnitario { get; set; }
            public decimal Total { get; set; }
            public DateTime FechaRegistro { get; set; }

            public List<Transaction> transactions { get; set; }
        }
    }
}