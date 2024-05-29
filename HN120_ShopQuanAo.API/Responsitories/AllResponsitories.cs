using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class AllResponsitories<T> : IAllResponsitories<T> where T : class
    {
        private AppDbContext context;
        private DbSet<T> dbset;
        public AllResponsitories()
        {

        }
        public AllResponsitories(AppDbContext context, DbSet<T> dbset)
        {
            this.context = context;
            this.dbset = dbset;
        }
        public async Task<bool> CreateItem(T item)
        {
            try
            {
                await dbset.AddAsync(item);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(T item)
        {
            try
            {
                dbset.Remove(item);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbset.ToListAsync();
        }
        public async Task<T> GetByID(string id)
        {
            return await dbset.FindAsync(id);
        }

        public async Task<bool> UpdateItem(T item)
        {
            try
            {
                dbset.Update(item);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
