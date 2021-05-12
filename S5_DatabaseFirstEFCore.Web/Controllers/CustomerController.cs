using Microsoft.AspNetCore.Mvc;
using S5_DatabaseFirstEFCore.Web.Models;
using S5_DatabaseFirstEFCore.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S5_DatabaseFirstEFCore.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Listado()
        {
            var customers = await CustomerRepository.ListadoAsincrono();
            return PartialView(customers);
        }

        public async Task<IActionResult> Obtener(int idCliente)
        {
            var customer = await CustomerRepository.Obtener(idCliente);
            return Json(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int idCliente)
        {
            bool exito = await CustomerRepository.Eliminar(idCliente);
            return Json(exito);
        }

        [HttpPost]
        public async Task<IActionResult> Grabar(int idCliente,
            string nombre, string apellido, string ciudad, string telefono,
            string pais)
        {
            var customer = new Customer()
            {
                FirstName = nombre,
                LastName = apellido,
                City = ciudad,
                Country = pais,
                Phone = telefono
            };

            bool exito = true;
            if (idCliente == -1)
                exito = await CustomerRepository.Insertar(customer);
            else
            {
                customer.Id = idCliente;
                exito = await CustomerRepository.Actualizar(customer);
            }
            return Json(exito);
        }

    }
}
