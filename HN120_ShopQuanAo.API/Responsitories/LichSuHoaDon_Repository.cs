using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class LichSuHoaDon_Repository : LichSuHoaDon_Irepository
    {
        private readonly AppDbContext _context;
        public LichSuHoaDon_Repository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateLichSuHoaDon(HoaDon_History lshd)
        {
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyyMMddHHmmss");
            var hd = _context.HoaDon_History.FirstOrDefault(c => c.LichSuHoaDonID == lshd.LichSuHoaDonID);
            if (hd != null)
            {
                throw new Exception("Hóa đơn đã tồn tại");
            }
            else
            {
                _context.HoaDon_History.Add(lshd);
                _context.SaveChanges();
            }
           
           
        }

        public IEnumerable<HoaDon_History> GetAllLichSuHoaDon()
        {
            return _context.HoaDon_History;
        }

        public HoaDonChiTiet GetLichSuHoaDonByMa(string ma)
        {
            throw new NotImplementedException();
        }
    }
}
