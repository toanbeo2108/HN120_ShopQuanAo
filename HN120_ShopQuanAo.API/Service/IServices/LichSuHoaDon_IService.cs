using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.IServices
{
    public interface LichSuHoaDon_IService
    {
        IEnumerable<HoaDon_History> GetAllLichSuHoaDon();
        HoaDonChiTiet GetLichSuHoaDonByMa(string ma);
        void CreateLichSuHoaDon(HoaDon_History lshd);
    }
}
