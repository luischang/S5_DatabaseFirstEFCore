using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using S5_DatabaseFirstEFCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            //using var data = new SalesContext();
            //var customers = await data.Customer.ToListAsync();

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:41984/api/Customer/GetCustomer");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(apiResponse);


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
            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:41984/api/Customer/GetCustomerById/"+id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<Customer>(apiResponse);

            //using var data = new SalesContext();
            //var customers = await data.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();
            return customers;
        }


        public static async Task<bool> Insertar(Customer customer)
        {
            bool exito = true;

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .PostAsync("http://localhost:41984/api/Customer/PostCustomer", data);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<Customer>(apiResponse);
            if (customers == null)
                exito = false;
            //try
            //{
            //    using var data = new SalesContext();
            //    data.Customer.Add(customer);
            //    await data.SaveChangesAsync();

            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}
            return exito;
        }


        public static async Task<bool> Actualizar(Customer customer)
        {
            bool exito = true;

            try
            {
                //Se obtiene el customer por ID 
                using var httpClient = new HttpClient();
                using var response = await httpClient
                    .GetAsync("http://localhost:41984/api/Customer/GetCustomerById/" + customer.Id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var customerByID = JsonConvert.DeserializeObject<Customer>(apiResponse);

                //Se realizar la actualización del customer

                var json = JsonConvert.SerializeObject(customer);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using var responsePut = await httpClient
                    .PutAsync("http://localhost:41984/api/Customer/PutCustomer", data);

                string apiResponsePut = await responsePut.Content.ReadAsStringAsync();
                var customerResponse = JsonConvert.DeserializeObject<Customer>(apiResponsePut);
                if (customerResponse == null)
                    exito = false;

                //using var data = new SalesContext();
                //var customerNow = await data.Customer.Where(x => x.Id == customer.Id).FirstOrDefaultAsync();//await Obtener(customer.Id);

                //customerNow.FirstName = customer.FirstName;
                //customerNow.LastName = customer.LastName;
                //customerNow.City = customer.City;
                //customerNow.Country = customer.Country;
                //customerNow.Phone = customer.Phone;

                //await data.SaveChangesAsync();

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
                ////Obtener el customer by ID
                using var httpClient = new HttpClient();
                //using var response = await httpClient
                //    .GetAsync("http://localhost:41984/api/Customer/GetCustomerById/" + id);
                //string apiResponse = await response.Content.ReadAsStringAsync();
                //var customer = JsonConvert.DeserializeObject<Customer>(apiResponse);

                //Eliminar by ID

                using var responseDelete = await httpClient
                  .DeleteAsync("http://localhost:41984/api/Customer/DeleteCustomer/" + id);
                string apiResponseDelete = await responseDelete.Content.ReadAsStringAsync();
                if ((int)responseDelete.StatusCode == 404)
                    exito = false;

                //using var data = new SalesContext();
                //var customerNow = await Obtener(id);

                //data.Customer.Remove(customerNow);
                //await data.SaveChangesAsync();
            }
            catch (Exception)
            {
                exito = false;
            }

            return exito;
        }


    }
}
