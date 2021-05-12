using Microsoft.EntityFrameworkCore;
using S5_DatabaseFirstEFCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S5_DatabaseFirstEFCore.Web.Repositories
{
    public class CustomerRepository
    {

        public static IEnumerable<Customer> Listado()
        {
            using var data = new SalesContext();
            var customers = data.Customer.OrderBy(x=>x.LastName).ToList();
            return customers;
        }

        public static async Task<IEnumerable<Customer>> ListadoAsincrono()
        {
            using var data = new SalesContext();
            var customers = await data.Customer.ToListAsync();
            return customers;
        }

        public static IEnumerable<Customer> ListadoConSP()
        {
            using var data = new SalesContext();
            var customers = data.Customer.FromSqlRaw("SP_GET_CLIENTES");
            return customers;
        }

        public static async Task<Customer> Obtener(int id)
        {
            using var data = new SalesContext();
            var customers = await data.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();
            return customers;
        }


        public static async Task<bool> Insertar(Customer customer)
        {
            bool exito = true;

            try
            {
                using var data = new SalesContext();
                data.Customer.Add(customer);
                await data.SaveChangesAsync();

            }
            catch (Exception)
            {
                exito = false;
            }

            return exito;
        }


        public static async Task<bool> Actualizar(Customer customer)
        {
            bool exito = true;

            try
            {
                using var data = new SalesContext();
                var customerNow = await data.Customer.Where(x => x.Id == customer.Id).FirstOrDefaultAsync();//await Obtener(customer.Id);

                customerNow.FirstName = customer.FirstName;
                customerNow.LastName = customer.LastName;
                customerNow.City = customer.City;
                customerNow.Country = customer.Country;
                customerNow.Phone = customer.Phone;

                await data.SaveChangesAsync();

            }
            catch (Exception)
            {
                exito = false;
            }

            return exito;
        }

        public static async Task<bool> Eliminar(int id)
        {
            bool exito = true;

            try
            {
                using var data = new SalesContext();
                var customerNow = await Obtener(id);

                data.Customer.Remove(customerNow);
                await data.SaveChangesAsync();
            }
            catch (Exception)
            {
                exito = false;
            }

            return exito;
        }


    }
}
