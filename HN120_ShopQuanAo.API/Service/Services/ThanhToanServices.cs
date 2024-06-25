using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class ThanhToanServices : IThanhToanServices
    {
        private readonly IThanhToanRepository _repo;
        public ThanhToanServices(IThanhToanRepository repo)
        {
            _repo = repo;
        }
        public void CreatThanhToan(ThanhToan tt)
        {
            _repo.CreatThanhToan(tt);
        }

        public void DeleteThanhToan(string ma)
        {
            try
            {
                _repo.DeleteThanhToan(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public IEnumerable<ThanhToan> GetAllThanhtoan()
        {
           return _repo.GetAllThanhtoan();
        }

        public ThanhToan GetThanhToanByMa(string ma)
        {
            try
            {
                return _repo.GetThanhToanByMa(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public void UpdateThanhToan(ThanhToan tt)
        {
            try
            {
                _repo.UpdateThanhToan(tt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }
    }
}
