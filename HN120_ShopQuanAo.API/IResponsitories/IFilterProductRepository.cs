using HN120_ShopQuanAo.API.Model;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IFilterProductRepository
    {
        IQueryable<SanPham> GetFilteredProducts(FilterProductDto filterDto);
        PagedResultDto<SanPham> GetPagedProducts(FilterProductDto filterDto, int pageNumber, int pageSize, string? sortBy);
        MinMaxPriceDto GetMinMaxPrice();
    }
}
