using System;
using CRUD.Models;
using CRUD.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static CRUD.Models.EntidadClientes;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace CRUD.Controllers
{
    public class ClienteController : Controller
    {
        
        ContextClientes _clientes = new ContextClientes();

        //Obtener lista de clientes ACTIVOS
        public ActionResult Detalle() 
        {
            //var client = _clientes.Infos.ToList();
            var client = _clientes.Customers.Where(x => x.Estado == 1).ToList();
            
            var model = new CatalogoViewModels.Catalogo
            { ListaCliente = client};
            return View(model); 
        }

        //Obtener lista de TODOS los clientes
        public ActionResult DetalleTodos()
        {
            var client = _clientes.Customers.ToList();
            //var client = _clientes.Infos.Where(x => x.Estado == 1).ToList();

            var model = new CatalogoViewModels.Catalogo
            { ListaCliente = client };
            return View(model);
        }

        //Controlador para Crear clientes
        public ActionResult Create() 
        {
            var model = new CatalogoViewModels.CrearCliente();

            Console.WriteLine(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CatalogoViewModels.CrearCliente model)
        {
            var Cliente = new Customer
            {
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                RFC = model.RFC,
                Direccion = model.Direccion,
                CP = model.CP,
                Email = model.Email,
                Estado = 1,
            };
            Console.WriteLine(Cliente);
            _clientes.Customers.Add(Cliente);
            _clientes.SaveChanges();

            return RedirectToAction("Detalle");
        }

        //Controlador para Editar clientes
        public ActionResult Edit(int id)
        {
            var x = _clientes.Customers.Find(id);
                var model = new CatalogoViewModels.CrearCliente 
                { 
                    IdCliente = x.IdCliente,
                    Nombre = x.Nombre,
                    Apellidos = x.Apellidos,
                    RFC = x.RFC,
                    Direccion = x.Direccion,
                    CP = x.CP,
                    Email = x.Email,
                    Estado = x.Estado,
                };
                return View(model);
            
        }

        [HttpPost]
        public ActionResult Edit(CatalogoViewModels.CrearCliente model)
        {
            var cliente = _clientes.Customers.Find(model.IdCliente);
            if (cliente != null)
            {
                cliente.IdCliente = model.IdCliente;
                cliente.Nombre = model.Nombre;
                cliente.Apellidos = model.Apellidos;
                cliente.RFC = model.RFC;
                cliente.Direccion = model.Direccion;
                cliente.CP = model.CP;
                cliente.Email = model.Email;
                cliente.Estado = model.Estado;                
            }
            _clientes.SaveChanges();
            return RedirectToAction("Detalle");
        }


        //Controlador para Eliminar clientes
        public ActionResult Delete(int id)
        {
            var cliente = _clientes.Customers.Find(id);
            cliente.Estado = 0;

            _clientes.SaveChanges();
            return RedirectToAction("Detalle");
        }
    }
}
