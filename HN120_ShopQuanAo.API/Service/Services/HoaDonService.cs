using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _repo;
        public HoaDonService(IHoaDonRepository repo)
        {
                _repo = repo;
        }
        public void CreateHoaDon(HoaDon hoaDon)
        {
            _repo.CreateHoaDon(hoaDon);
        }

        public void DeleteHoaDon(string ma)
        {
            try
            {
                _repo.DeleteHoaDon(ma);
            }
            catch (Exception ex)
            {
              
                throw ex; 
            }
        }

        public IEnumerable<HoaDon> GetAllHoaDon()
        {
           return _repo.GetAllHoaDon();
        }

        public HoaDon GetHoaDonByMa(string ma)
        {
            try
            {
                return _repo.GetHoaDonByMa(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public void UpdateHoaDon(HoaDon hoaDon)
        {
            try
            {
                _repo.UpdateHoaDon(hoaDon);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
