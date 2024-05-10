using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task Add(T entity)
        {
            var result = dbSet.Add(entity);
            await SaveChanges();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> filter, string? includePropeties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includePropeties))
            {
                foreach (var includeProp in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                    
                }
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropeties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includePropeties))
            {
                foreach (var includeProp in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();

        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await SaveChanges();
        }
        
        public async Task RemoveMany(IEnumerable<T> list)
        {
            dbSet.RemoveRange(list);
            await SaveChanges();
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await SaveChanges();
        }



        private async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

    }  
}
