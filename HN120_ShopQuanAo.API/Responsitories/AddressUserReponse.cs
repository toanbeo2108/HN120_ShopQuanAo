using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class AddressUserReponse : IAddressUserReponse
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AddressUserReponse(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, string ErrorMessage)> Create(DeliveryAddressModel item)
        {
            try
            {
                // Kiểm tra UserID có chính xác không
                var checkUser = await _userManager.FindByIdAsync(item.UserID);
                if (checkUser == null)
                {
                    return (false, "UserID truyền vào không đúng.");
                }
                // Lấy tổng số lượng địa chỉ
                var TongDiaChi = await _context.DeliveryAddress.CountAsync();

                // Thêm mới địa chỉ giao hàng vào cơ sở dữ liệu
                var newAddress = new DeliveryAddress
                {
                    // Tạo id địa chỉ mới bằng cách thêm 1 vào tổng số lượng Địa chỉ và thêm tiền tố "HD"
                    DeliveryAddressID = "Dc" + (TongDiaChi + 1),
                    UserID = item.UserID,
                    Consignee = item.Consignee,
                    PhoneNumber = item.PhoneNumber,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    Status = item.Status,
                };

                _context.DeliveryAddress.Add(newAddress);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi ở đây nếu cần
                return (false, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> Delete(string id)
        {
            try
            {
                var address = await _context.DeliveryAddress.FindAsync(id);
                if (address == null)
                {
                    return (false, "Không tìm thấy địa chỉ.");
                }

                _context.DeliveryAddress.Remove(address);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        public async Task<List<DeliveryAddress>> GetAll()
        {
            return await _context.DeliveryAddress.ToListAsync();
        }

        public async Task<DeliveryAddress> GetByID(string id)
        {
            return await _context.DeliveryAddress.FindAsync(id);
        }

        public async Task<(bool Success, string ErrorMessage)> Update(string id,DeliveryAddressModel item)
        {
            try
            {
                var address = await _context.DeliveryAddress.FindAsync(id);
                if (address == null)
                {
                    return (false, "Không tìm thấy địa chỉ.");
                }
                address.Consignee = item.Consignee;
                address.PhoneNumber = item.PhoneNumber;
                address.City = item.City;
                address.District = item.District;
                address.Ward = item.Ward;
                address.Street = item.Street;
                address.Status = item.Status;

                _context.DeliveryAddress.Update(address);
                await _context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
    }
}
