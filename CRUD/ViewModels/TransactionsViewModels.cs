using System;
using System.Collections.Generic;
using static CRUD.Models.EntidadClientes;

namespace CRUD.Validations
{
    public class TransactionsViewModels
    {
        public int IdTransaction { get; set; }
        public int IdCliente { get; set;}
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }

        public List<Transaction> transactions { get; set; }
    }
}