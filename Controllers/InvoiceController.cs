using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Database;
using PruebaTecnica.Entities;
using PruebaTecnica.Models;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private decimal exchangeRate = 0;

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public InvoiceController(ApplicationDbContext context, IConfiguration configuration)
        {          
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Invoice invoice)
        {
            await GetExchangeRate();
            foreach (Line line in invoice.Lines)
            {
                line.Price = line.Currency != _configuration["DefaultCurrency"] ? line.Price * exchangeRate : line.Price;
                line.Currency = _configuration["DefaultCurrency"];
                invoice.Tax_total += ((line.Price * line.Quantity) * (line.Tax_rate / 100));
                invoice.Discount_total += ((line.Price * line.Quantity) * (line.Discount_rate / 100));
                invoice.Subtotal += line.Price * line.Quantity;                
            }
            invoice.Total += (invoice.Subtotal + invoice.Tax_total) - invoice.Discount_total;
            if (await _context.Clients.Where(x => x.Id == invoice.Client.Id).CountAsync() > 0)
                invoice.Client = await _context.Clients.Where(x => x.Id == invoice.Client.Id).FirstAsync();

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Pay")]
        public async Task<ActionResult> Pay([FromBody] Pay pay)
        {
            Invoice invoice = await _context.Invoices.Where(x => x.id == pay.Invoice_id).FirstOrDefaultAsync();
            if (invoice.Total >= pay.amount && (invoice.Balance + pay.amount) <= invoice.Total)
            {
                Payment payment = new Payment()
                {
                    Total = pay.amount
                };
                await _context.AddAsync(payment);

                invoice.Payments = invoice.Payments == null ? new List<Payment>(): invoice.Payments ;
                invoice.Payments.Add(payment);

                invoice.Balance = invoice.Balance + pay.amount;

                _context.Entry(invoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if (invoice.Balance == invoice.Total)
                    return BadRequest("Invoices can't be overpaid");
            }
            return BadRequest("The amount is greater than the total");
        }

        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> Get()
        {
            return await _context.Invoices.Include(x => x.Lines).Include(x => x.Client).Include(x => x.Payments).ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Invoice invoice = await _context.Invoices.Include(x => x.Lines).Include(x => x.Payments).Where(x => x.id == id).FirstOrDefaultAsync();
            if (invoice == null)
                return BadRequest("Invoice not found");
            _context.Lines.RemoveRange(invoice.Lines);
            _context.Payments.RemoveRange(invoice.Payments);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task GetExchangeRate()
        {
            string url = string.Format(_configuration["BCCR:URL"], _configuration["BCCR:Indicador"], DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), _configuration["BCCR:Nombre"], _configuration["BCCR:SubNiveles"]);
            using (var httpClient = new HttpClient())
            {
                using (var tipoCambio = await httpClient.GetAsync(url))
                {
                    string apiResponse = await tipoCambio.Content.ReadAsStringAsync();
                    int position1 = apiResponse.IndexOf(_configuration["BCCR:PrimerValor"]);
                    int position2 = apiResponse.IndexOf(_configuration["BCCR:SegundoValor"]);
                    exchangeRate = decimal.Parse(apiResponse.Substring(position1 + 17, (position2 - position1) - 18));
                }
            }
        }
    }
}