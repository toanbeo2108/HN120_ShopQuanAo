//using HN120_ShopQuanAo.API.Data;
//using HN120_ShopQuanAo.API.IResponsitories;
//using HN120_ShopQuanAo.Data.Models;

//namespace HN120_ShopQuanAo.API.Responsitories
//{
//    public class VoucherRepository : IVoucherRepository
//    {
//        private readonly AppDbContext _context;
//        public VoucherRepository(AppDbContext context)
//        {
//            _context = context;
//        }
//        public void CreateVoucher(Voucher vc)
//        {
//            var total = _context.Voucher.Count();
//            vc.MaVoucher = "VC" + (total + 1);
//            var voucher = _context.Voucher.FirstOrDefault(c => c.MaVoucher == vc.MaVoucher);
//            if (voucher != null)
//            {
//                throw new Exception("VC đã tồn tại");
//            }
//            _context.Voucher.Add(vc);
//            _context.SaveChanges();
//        }

//        public void DeleteVoucher(string ma)
//        {
//            var vc = _context.Voucher.FirstOrDefault(c => c.MaVoucher == ma);
//            if (vc != null)
//            {
//                _context.Remove(vc);
//                _context.SaveChanges();
//            }
//            else
//            {
//                throw new Exception("Không tìm thấy hóa đơn chi tiết ");
//            }
//        }

//        public IEnumerable<Voucher> GetAllVoucher()
//        {
//            return _context.Voucher;
//        }

//        public Voucher GetVoucherByMa(string ma)
//        {
//            var vc = _context.Voucher.FirstOrDefault(c => c.MaVoucher == ma);
//            if (vc != null)
//            {
//                return vc;
//            }
//            else
//            {
//                throw new Exception("Mã hóa đơn chi tiết không tồn tại");
//            }
//        }

//        public void UpdateVoucher(Voucher vc)
//        {
//            var mvc = _context.Voucher.FirstOrDefault(c => c.MaVoucher == vc.MaVoucher);
//            if (mvc == null)
//            {
//                throw new Exception("Mã hóa đơn chi tiết không tồn tại");

//            }
//            mvc.Ten = vc.Ten;
//            mvc.GiaGiamToiThieu = vc.GiaGiamToiThieu;
//            mvc.GiaGiamToiDa = vc.GiaGiamToiDa;
//            mvc.NgayBatDau = vc.NgayBatDau;
//            mvc.NgayKetThuc = vc.NgayKetThuc;
//            mvc.KieuGiamGia = vc.KieuGiamGia;
//            mvc.GiaTriGiam = vc.GiaTriGiam;
//            mvc.SoLuong = vc.SoLuong;
//            mvc.MoTa = vc.MoTa;
//            mvc.TrangThai = vc.TrangThai;
//            _context.SaveChanges();
//        }
//    }
//}
