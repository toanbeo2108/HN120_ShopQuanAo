using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IHoaDonRepository
    {
        IEnumerable<HoaDon> GetAllHoaDon();
        HoaDon GetHoaDonByMa(string ma);
        void CreateHoaDon(HoaDon hoaDon);
        void UpdateHoaDon(HoaDon hoaDon);
        void DeleteHoaDon(string ma);
    }
}
