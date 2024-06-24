using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IThanhToanRepository
    {
        IEnumerable<ThanhToan> GetAllThanhtoan();
        ThanhToan GetThanhToanByMa(string ma);
        void CreatThanhToan(ThanhToan tt);
        void UpdateThanhToan(ThanhToan tt);
        void DeleteThanhToan(string ma);
    }
}
