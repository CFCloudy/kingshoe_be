using DTO.Paging;
using DTO.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class OrderDTO
    {

    }

    public class ResponOrderDTO {
        public int Id { get; set; }
        public string? CodeOrder { get; set; }

        public int? Status { get; set; }

        public int IdKhachHang { get; set; }

        public string? TenKhachHang { get; set; }

        public decimal? TongTien { get; set; }

        public int? TongSanPham { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }

    public class FilterOrder:PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public int? UserId { get; set; }
        public string? OrderCode  { get; set; }

        public int? status { get; set; }

        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "item.CreationTime";
            }
        }
    }

    public class FilterPhieuGiaoHang : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public int? UserId { get; set; }
        public int? OrderId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "item.CreationTime";
            }
        }
    }

    public class ResponseOrder
    {
        public int Id { get; set; }
        public int? IdKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public string? PhoneNumber { get; set; }
        
        public int IdAdress { get; set; }
        public int? Status { get; set; }

        public IList<CartItems> Items { get; set; }  
        public string? GhiChu { get; set; }
        public decimal? SoTienDuocTru { get; set; } = 0.0M;
        public DateTime? CreateAtTime {get; set; }
        public ShippingDetailView ShippingDetailView { get; set; } = new ShippingDetailView();

    }
    public class ResponsePhieuGiaoHang
    {
        public int Id { get; set; }
        public int? IdKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public int OrderId { get; set; }

        public string? ShippingName { get; set; }
        public string? ShippingPhone { get; set; }
        public string? SenderAddress { get; set; }
        public string? OrderNote { get;set; }

        public string? MaShip { get;set; }

        public DateTime? CreatedAt { get; set; }
        public int IdAdress { get; set; }
        public int? Status { get; set; }

        public IList<CartItems> Items { get; set; }
        public string? GhiChu { get; set; }

    }

    public class CartItems
    {

        public string? Img { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TongTien { get; set; }
        public int? ProductVariantId { get; set; }
        
        public string? VariantName { get; set; }
    }

    public class OrderLogResponse
    {

        public int Id { get; set; }
        public int OrderId { get; set; }

        public int? IdKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public string? TenBoss { get; set; }

        public int? IdBoss { get; set; }

        public string? Message { get; set; }
        public DateTime LogTime { get; set; }


    }

}
