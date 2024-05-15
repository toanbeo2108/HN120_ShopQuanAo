using HN120_ShopQuanAo.Data.ViewModels;

namespace HN120_ShopQuanAo.API.IResponsitories
{
	public interface ILoginServices
	{
        Task<Response> LoginAsync(LoginUser loginUser);
	}
}
