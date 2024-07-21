using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class GHCTResponse : IGHCTResponse
    {
        private readonly AppDbContext appContext;
        public GHCTResponse()
        {
            appContext = new AppDbContext();
        }
        public async Task<bool> UpdateGHCT(string MaGHCT, int? soluong)
        {
            var ghct = await appContext.GioHangChiTiet.FirstOrDefaultAsync(x => x.MaGioHangChiTiet == MaGHCT);
            if(ghct == null)
            {
                return false;
            }
            ghct.SoLuong = soluong;
            ghct.ThanhTien = soluong * ghct.DonGia;
            appContext.GioHangChiTiet.Update(ghct);
            await appContext.SaveChangesAsync();
            return true;
        }
    }
}
