using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Service.Services
{
    public class VoucherServices : IVoucherServices
    {
        private readonly IVoucherRepository _repo;
        public VoucherServices(IVoucherRepository repo)
        {
            _repo = repo;
        }
        public void CreateVoucher(Voucher voucher)
        {
            _repo.CreateVoucher(voucher);
        }

        public void DeleteVoucher(string ma)
        {
            try
            {
                _repo.DeleteVoucher(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Voucher> GetAllVoucher()
        {
            return _repo.GetAllVoucher();
        }

        public Voucher GetVoucherByMa(string ma)
        {
            try
            {
                return _repo.GetVoucherByMa(ma);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateVoucher(Voucher voucher)
        {
            try
            {
                _repo.UpdateVoucher(voucher);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
