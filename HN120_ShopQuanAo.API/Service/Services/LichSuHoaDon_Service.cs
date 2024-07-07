using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class LichSuHoaDon_Service : LichSuHoaDon_IService
    {
        private readonly LichSuHoaDon_Irepository _respon;
        public LichSuHoaDon_Service(LichSuHoaDon_Irepository respon)
        {
                _respon = respon;
        }
        public void CreateLichSuHoaDon(HoaDon_History lshd)
        {
             _respon.CreateLichSuHoaDon(lshd);
        }

        public IEnumerable<HoaDon_History> GetAllLichSuHoaDon()
        {
           return _respon.GetAllLichSuHoaDon();
        }

        public HoaDonChiTiet GetLichSuHoaDonByMa(string ma)
        {
            throw new NotImplementedException();
        }
    }
}
