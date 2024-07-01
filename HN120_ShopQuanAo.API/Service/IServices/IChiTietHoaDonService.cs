using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.IServices
{
    public interface IChiTietHoaDonService
    {
        IEnumerable<HoaDonChiTiet> GetAllHoaDonChiTiet();
        HoaDonChiTiet GetHoaDonChiTietByMa(string ma);
        void CreateCTHD(List<HoaDonChiTiet> hdct);
        void UpdateCTHD(List<HoaDonChiTiet> hdct);
        void DeleteCTHD(string ma);
    }
}
