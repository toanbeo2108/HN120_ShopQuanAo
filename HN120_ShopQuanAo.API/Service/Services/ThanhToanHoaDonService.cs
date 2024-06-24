using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class ThanhToanHoaDonService : IThanhToanHoaDonService
    {
        private readonly IThanhToanHoaDonRepository _repo;
        public ThanhToanHoaDonService(IThanhToanHoaDonRepository repo)
        {
                _repo = repo;
        }
        public void CreateThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                _repo.CreateThanhToan_HoaDon(tt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteThanhToan_HoaDon(string ma)
        {
            try
            {
                _repo.DeleteThanhToan_HoaDon(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<ThanhToan_HoaDon> GetAllThanhToan_HoaDon()
        {
            return _repo.GetAllThanhToan_HoaDon();
        }

        public ThanhToan_HoaDon GetThanhToan_HoaDonByMa(string ma)
        {
            try
            {
                return _repo.GetThanhToan_HoaDonByMa(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                _repo.UpdateThanhToan_HoaDon(tt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
