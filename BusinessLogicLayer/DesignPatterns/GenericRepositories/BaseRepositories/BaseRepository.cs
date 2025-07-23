
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.DesignPatterns.SingletonPattern;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories
{
    public abstract class BaseRepositoriesository<T> : IRepository<T> where T : BaseEntity
    {
        MyContext _db;
        public BaseRepositoriesository()
        {
            _db=DbTool.DbInstance;
        }

        void Save()
        {
            _db.SaveChanges();

        }
        public void Add(T item)
        {
            _db.Set<T>().Add(item);//generic olduğu için T neyse _db ye set et kendini ve add yap
            Save();
            
        }

        public void AddRange(List<T> list)
        {
            _db.Set<T>().AddRange(list);//addrange add vs bu tarz komutlar EF de var onları kullanırız
            Save();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Any(expression);
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Count(expression);
        }

        public void Delete(T item)
        {
            //delete aslında veriyi yok etmek değil pasife çekmek bu yüzden durumunu değiştirecez.
            item.Status = Status.Passive;
            item.UpdatedDate = DateTime.Now;
            _db.Set<T>().Update(item);
            Save();


        }

        public void DeleteRange(List<T> list)
        {
            foreach (var item in list)
            {
                item.Status = Status.Passive;
                item.UpdatedDate = DateTime.Now;
            }
            _db.Set<T>().UpdateRange(list);
            Save();
        }
        

        public void Destroy(T item)
        {
           _db.Set<T>().Remove(item);
            Save();

        }

        public void DestroyRange(List<T> list)
        {
            _db.Set<T>().RemoveRange(list);
            Save();
        }

        public T Find(long id)
        {
            return _db.Set<T>().Find(id);

        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().FirstOrDefault(expression);
        }

        public List<T> GetActives()
        {
            return _db.Set<T>().Where(x => x.Status == Status.Active).ToList();
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public T GetById(long id)
        {
            return _db.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetDeleteds()
        {
            return _db.Set<T>().Where(x => x.Status == Status.Passive).ToList();
        }
        public List<T> GetModifieds()
        {
            return _db.Set<T>().Where(x => x.UpdatedDate != null).ToList();

        }

        public IQueryable<X> Select<X>(Expression<Func<T, X>> selector)
        {
            return _db.Set<T>().Select(selector);

        }

        public void SetActive(T item)
        {
            if (item.Status != Status.Active)
            {
                item.Status = Status.Active;
                item.UpdatedDate = DateTime.UtcNow;
                Update(item);
            }

        }

        public void SetPassive(T item)
        {
            if (item.Status != Status.Passive)
            {
                item.Status = Status.Passive;
                item.UpdatedDate = DateTime.UtcNow;
                Update(item);
            }

        }

        public void Update(T item)
        {
            var existingItem = _db.Set<T>().Find(item.Id);

            if (existingItem != null)
            {
                // Status değiştiyse güncelleme tarihi ver
                if (existingItem.Status != item.Status)
                {
                    item.UpdatedDate = DateTime.UtcNow;
                }
                else
                {
                    // Diğer alanlarda değişiklik varsa yine UpdatedDate verilebilir
                    // Bu kısmı senin projene göre kontrol et
                }

                _db.Entry(existingItem).CurrentValues.SetValues(item);
                Save();
            }
        }

        public void UpdateRange(List<T> list)
        {
            foreach (var item in list)
            {
                item.UpdatedDate = DateTime.UtcNow;
                _db.Set<T>().Update(item);
            }
            Save();
        }

        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Where(expression).ToList();
        }
    }

}
