namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IGHCTResponse
    {
        Task<bool> UpdateGHCT(string MaGHCT, int? soluong);
    }
}
