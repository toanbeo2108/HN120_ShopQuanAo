using HN120_ShopQuanAo.Data.ViewModels;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface ICreateAnAccount
    {
        Task<Response> AdminCreateAccount(NewAccountModel NewAccountModel, string role, string? userId = null);
    }
}
