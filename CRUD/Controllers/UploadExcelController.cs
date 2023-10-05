using ClosedXML.Excel;
using CRUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CRUD.Models;
using System.Linq;
using static CRUD.Models.EntidadClientes;
using Antlr.Runtime;
using System.Net;
using System.Web.Services;
using System.Web.Util;
using Microsoft.Ajax.Utilities;

namespace CRUD.Controllers
{
    public class UploadExcelController : Controller
    {
        ContextClientes _contextClientes = new ContextClientes();
        public ActionResult FormUpload()
        {
            return View("FormUpload");
        }

        [HttpPost]
        public ActionResult FormUpload(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            //Verifica la longitud del contenido del archivo y la extensión debe ser .xlsx.
            if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx")
            {
                string path = Path.Combine(Server.MapPath("~/Doc"), Path.GetFileName(file.FileName));
                //Guarda el archivo  
                file.SaveAs(path);
                //Leer el archivo Excel.
                using (XLWorkbook workbook = new XLWorkbook(path))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    bool FirstRow = true;
                    bool Validador = true;
                    //Rango para leer las celdas según la última celda utilizada.
                    string readRange = "1:1";
                    foreach (IXLRow row in worksheet.RowsUsed())
                    {
                        //Si lee la primera fila (usada), agréguela como nombre de columna.
                        if (FirstRow)
                        {
                            //Comprobando la última celda utilizada para la generación de columnas en la tabla de datos.
                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);

                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                var celda = cell.Address.ToString();
                                var Columnas = cell.Value.ToString();// (1,5)


                                if (celda == "A1" && Columnas != "IdCliente")
                                {
                                    ViewBag.Message = "No se encuentra el campo IdCliente en la celda A1";
                                    Validador = false;
                                    dt = null;
                                    break;
                                }
                                else if (celda == "B1" && Columnas != "Cantidad")
                                {
                                    ViewBag.Message = "No se encuentra el campo Cantidad en la celda B1";
                                    Validador = false;
                                    dt = null;
                                    break;
                                }
                                else if (celda == "C1" && Columnas != "Descripcion")
                                {
                                    ViewBag.Message = "No se encuentra el campo Descripción en la celda C1";
                                    Validador = false;
                                    dt = null;
                                    break;
                                }
                                else if (celda == "D1" && Columnas != "PrecioUnitario")
                                {
                                    ViewBag.Message = "No se encuentra el campo PrecioUnitario en la celda D1";
                                    Validador = false;
                                    dt = null;
                                    break;
                                }
                                else if (celda == "E1" && Columnas != "Total")
                                {
                                    ViewBag.Message = "No se encuentra el campo Total en la celda E1";
                                    Validador = false;
                                    dt = null;
                                    break;
                                }
                                else if (celda == "A1" && Columnas == "IdCliente" || celda == "B1" && Columnas == "Cantidad" || celda == "C1" && Columnas == "Descripcion" || celda == "D1" && Columnas == "PrecioUnitario" || celda == "E1" && Columnas == "Total")
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                else
                                {
                                    Validador = false;
                                    dt = null;
                                }
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            //Agregar una fila en la tabla de datos.
                            if (Validador) { 
                                dt.Rows.Add();
                                int cellIndex = 0;
                                //Actualizando los valores de la tabla de datos. 
                                foreach (IXLCell cell in row.Cells(readRange))
                                {
                                    dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                    cellIndex++;
                                }
                            }
                            else
                            {
                                /*ViewBag.Message = "Excel con formato incorrecto";*/

                            }
                        }
                    }
                    //Si no hay datos en el archivo Excel.
                    if (FirstRow)
                    {
                        ViewBag.Message = "Archivo de Excel vacío!";
                    }
                }
            }
            else
            {
                //Si la extensión del archivo cargado es diferente, entonces .xlsx.
                ViewBag.Message = "Seleccione el archivo con la extensión .xlsx!";
            }
            return View(dt);
        }

        [HttpPost]
        public JsonResult enviarDatos(List<CatalogoViewModels.TransactionsVM> lista)
        {
            //var result = new HttpStatusCodeResultWithBody 
            var mensaje = "Sin datos";

            if (lista == null)
            {
                mensaje = "La lista es nula";
            }
            if (ModelState.IsValid)
            {
                var contador = 0;
                if (lista.Count < 1)
                {
                    ViewBag.Message = ("Archivo está vacío");
                } else
                {
                    foreach (var item in lista)
                    {
                        //Buscamos si existe el cliente
                        var customers = _contextClientes.Customers.FirstOrDefault(x => x.IdCliente == item.IdCliente);

                        if (customers != null)
                        {
                            var transaction = new Transaction
                            {
                                IdCliente = Convert.ToInt16(item.IdCliente.ToString()),
                                Cantidad = Convert.ToInt16(item.Cantidad.ToString()),
                                Descripcion = item.Descripcion.ToString(),
                                PrecioUnitario = item.PrecioUnitario.ToString(),
                                Total = Convert.ToDecimal(item.Total.ToString()),
                                FechaRegistro = DateTime.Now,
                            };
                            _contextClientes.Transactions.Add(transaction);
                            contador += 1;
                        }

                    }
                }
                _contextClientes.SaveChanges();
                mensaje = "Se ha insertado correctamente " + contador + " registros";

            }

            return Json(mensaje);
        }

        [HttpGet]
        public JsonResult BuscarPorId(int id)
        {
            var transaction = _contextClientes.Transactions.Where(x => x.IdCliente == id).ToList();
            /*var model = new CatalogoViewModels.TransactionsVM
            {
                transactions = transaction
            };*/
            if (transaction.Count == 0)
            {
                var response = "No hay datos";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                /*foreach (var transaction in transactions)
                {
                    transaction.PrecioUnitario = decimal.Parse(transaction.PrecioUnitario.ToString());
                }

                // Prepara el modelo con las transacciones corregidas
                var model = new CatalogoViewModels.TransactionsVM
                {
                    transactions = transactions
                };*/

                return Json(transaction, JsonRequestBehavior.AllowGet);
            }

            // Preparar los datos para mostrar en la tabla de búsqueda
            //var model = new List<Transaction> { transaction };

            
        }
    }

        
}