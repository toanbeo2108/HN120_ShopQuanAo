namespace HN120_ShopQuanAo.View.Models
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }  // Danh sách các item (dữ liệu) trong trang hiện tại
        public int TotalItems { get; set; }  // Tổng số lượng item (dữ liệu) trong tất cả các trang
        public int PageNumber { get; set; }  // Số trang hiện tại
        public int PageSize { get; set; }    // Số lượng item trên mỗi trang

        public PagedResultDto(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
