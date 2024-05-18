namespace HN120_ShopQuanAo.API.Ireponsitory
{
    public interface IAllResponsitories<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(string id);
        Task<bool> CreateItem(T item);
        Task<bool> DeleteItem(T item);
        Task<bool> UpdateItem(T item);
    }
}
