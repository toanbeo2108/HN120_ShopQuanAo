using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IHoaDon_Respository
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
