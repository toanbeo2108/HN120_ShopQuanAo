using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IThanhToanHoaDonRepository
    {
        IEnumerable<ThanhToan_HoaDon> GetAllThanhToan_HoaDon();
        ThanhToan_HoaDon GetThanhToan_HoaDonByMa(string ma);
        void CreateThanhToan_HoaDon(ThanhToan_HoaDon tt);
        void UpdateThanhToan_HoaDon(ThanhToan_HoaDon tt);
        void DeleteThanhToan_HoaDon(string ma);

    }
}
