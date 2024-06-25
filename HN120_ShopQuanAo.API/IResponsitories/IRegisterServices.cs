using HN120_ShopQuanAo.Data.ViewModels;

namespace HN120_ShopQuanAo.API.IResponsitories
{
	public interface IRegisterServices
	{
		Task<Response> RegisterAsync(RegisterUser registerUser, string role);
		Task<Response> CreateAnAccount(NewAccountModel NewAccountModel, string role);
    }
}
