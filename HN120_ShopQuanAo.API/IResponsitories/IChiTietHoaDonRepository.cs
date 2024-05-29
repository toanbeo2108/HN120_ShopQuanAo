using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IChiTietHoaDonRepository
    {
        IEnumerable<HoaDonChiTiet> GetAllHoaDonChiTiet();
        HoaDonChiTiet GetHoaDonChiTietByMa(string ma);
        void CreateCTHD(HoaDonChiTiet hdct);
        void UpdateCTHD(HoaDonChiTiet hdct);
        void DeleteCTHD(string ma);
    }
}
