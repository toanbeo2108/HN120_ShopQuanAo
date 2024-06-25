using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly IChiTietHoaDonRepository _repo;
        public ChiTietHoaDonService(IChiTietHoaDonRepository repo)
        {
                _repo = repo;
        }
        public void CreateCTHD(List<HoaDonChiTiet> hdct)
        {
            _repo.CreateCTHD(hdct);
        }
        public void DeleteCTHD(string ma)
        {
            try
            {
                _repo.DeleteCTHD(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<HoaDonChiTiet> GetAllHoaDonChiTiet()
        {
           return _repo.GetAllHoaDonChiTiet();
        }

        public HoaDonChiTiet GetHoaDonChiTietByMa(string ma)
        {
            try
            {
                return _repo.GetHoaDonChiTietByMa(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateCTHD(HoaDonChiTiet hdct)
        {
            try
            {
                _repo.UpdateCTHD(hdct);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
