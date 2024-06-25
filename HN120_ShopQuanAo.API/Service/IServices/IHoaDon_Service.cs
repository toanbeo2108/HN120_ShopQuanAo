using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.IServices
{
    public interface IHoaDon_Service
    {
        IEnumerable<HoaDon> GetAllHoaDon();
        IEnumerable<HoaDon> GetHoaDonByTrangthai(int stt);
        HoaDon GetHoaDonByMa(string ma);
        void CreateHoaDon(HoaDon hoaDon);
        void UpdateHoaDon(HoaDon hoaDon);
        void DeleteHoaDon(string ma);
        IEnumerable<dynamic> GetHoaDonWithDetails(string ma);
    }
}
