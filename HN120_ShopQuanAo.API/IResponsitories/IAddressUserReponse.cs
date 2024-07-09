using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IAddressUserReponse
    {
        Task<List<DeliveryAddress>> GetAll();
        Task<DeliveryAddress> GetByID(string id);
        Task<(bool Success, string ErrorMessage)> Create(DeliveryAddressModel item);
        Task<(bool Success, string ErrorMessage)> Delete(string id);
        Task<(bool Success, string ErrorMessage)> Update(string id,DeliveryAddressModel item);
        Task<(bool Success, string ErrorMessage)> SetasDefault(string id);
    }
}
