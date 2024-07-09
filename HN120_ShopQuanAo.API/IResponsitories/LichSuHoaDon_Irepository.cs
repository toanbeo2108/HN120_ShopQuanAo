using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface LichSuHoaDon_Irepository
    {
        IEnumerable<HoaDon_History> GetAllLichSuHoaDon();
        HoaDonChiTiet GetLichSuHoaDonByMa(string ma);
        void CreateLichSuHoaDon(HoaDon_History lshd);
    }
}
