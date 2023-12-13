using BUS.IService;
using BUS.Services;
using DAL.Models;
using DTO.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly ShoeStoreContext _context;
        private readonly MailServices _mailServices;
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;
        private IRepositoryShoe_BUS _repositoryShoe_BUS;

        public StatisticalController(ShoeStoreContext context, IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS, IRepositoryShoe_BUS repositoryShoe_BUS)
        {
            _context = context;
            _mailServices = new MailServices();
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
            _repositoryShoe_BUS = repositoryShoe_BUS;
        }
        [HttpGet("GetInformation")]
        public async Task<IActionResult> GetInformation(DateTime startDate, DateTime endDate)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest("Chưa nhập đủ thời gian");
            }
            StatisticalInformationDTO statisticalInformationDTO = new StatisticalInformationDTO();
            var orders = _context.Orders.Where(c => c.Status == 5 && c.CreatedAt.Value.Date >= startDate.Date && c.CreatedAt.Value.Date <= endDate.Date).Include(c => c.OrderItems);
            foreach (var order in orders)
            {
                var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == order.Id);
                var TotalorderItem = 0.0M;
                foreach (var orderItem in order.OrderItems)
                {
                    statisticalInformationDTO.TotalNumberOfProductsSold += orderItem.Quantity.Value;
                    var shoeVariant = await _repositoryShoeVariant_BUS.Getone(orderItem.ProductVariantId.Value);
                    var shoe = await _repositoryShoe_BUS.Getone(orderItem.ProductVariantId.Value);
                    TotalorderItem += ((orderItem.Price - shoe.OldPrice) * orderItem.Quantity).Value;
                }
                if (voucherUserLog != null)
                {
                    statisticalInformationDTO.TotalAmountSold += (TotalorderItem - voucherUserLog.Price.Value);
                }
                else
                {
                    statisticalInformationDTO.TotalAmountSold += TotalorderItem;
                }

            }
            statisticalInformationDTO.TotalNumberOfCustomers = orders.Select(c => c.UserId).Distinct().Count();
            return Ok(statisticalInformationDTO);
        }

        [HttpGet("RevenueChart")]
        public async Task<IActionResult> RevenueChart(int year)
        {
            if (year == 0)
            {
                return BadRequest("Chưa nhập năm");
            }
            var revenueCharts = new List<RevenueChartDTO>();
            var orders = _context.Orders.Where(c => c.Status == 5 && c.CreatedAt.Value.Date.Year == year).Include(c => c.OrderItems);
            for (int i = 1; i <= 12; i++)
            {
                var orderMonth = orders.Where(c => c.CreatedAt.Value.Date.Month == i);
                RevenueChartDTO revenueChartDTO = new RevenueChartDTO();
                revenueChartDTO.Month = i;
                foreach (var order in orderMonth)
                {
                    var voucherUserLog = _context.VouchersUseLogs.FirstOrDefault(c => c.OrderId == order.Id);
                    var TotalorderItem = 0.0M;
                    foreach (var orderItem in order.OrderItems)
                    {
                        var shoeVariant = await _repositoryShoeVariant_BUS.Getone(orderItem.ProductVariantId.Value);
                        var shoe = await _repositoryShoe_BUS.Getone(orderItem.ProductVariantId.Value);
                        TotalorderItem += ((orderItem.Price - shoe.OldPrice) * orderItem.Quantity).Value;
                    }
                    if (voucherUserLog != null)
                    {
                        revenueChartDTO.Value += (TotalorderItem - voucherUserLog.Price.Value);
                    }
                    else
                    {
                        revenueChartDTO.Value += TotalorderItem;
                    }
                }
                revenueCharts.Add(revenueChartDTO);
            }
            return Ok(revenueCharts);
        }

        [HttpGet("CountProductChart")]
        public async Task<IActionResult> CountProductChart(int year)
        {
            if (year == 0)
            {
                return BadRequest("Chưa nhập năm");
            }
            var revenueCharts = new List<RevenueChartDTO>();
            var orders = _context.Orders.Where(c => c.Status == 5 && c.CreatedAt.Value.Date.Year == year).Include(c => c.OrderItems);
            for (int i = 1; i <= 12; i++)
            {
                var orderMonth = orders.Where(c => c.CreatedAt.Value.Date.Month == i);
                RevenueChartDTO revenueChartDTO = new RevenueChartDTO();
                revenueChartDTO.Month = i;
                foreach (var order in orderMonth)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        revenueChartDTO.Value += orderItem.Quantity.Value;
                    }
                }
                revenueCharts.Add(revenueChartDTO);
            }
            return Ok(revenueCharts);
        }

        
    }
}
