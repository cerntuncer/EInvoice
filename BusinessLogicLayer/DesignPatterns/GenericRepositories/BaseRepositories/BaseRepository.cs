
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.DesignPatterns.SingletonPattern;
using DatabaseAccessLayer.Contexts;
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
            throw new NotImplementedException();
        }

        public void Delete(T item)
        {
           //delete aslında veriyi yok etmek değil pasife çekmek bu yüzden durumunu değiştirecez.
           
        }

        public void DeleteRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public void Destroy(T item)
        {
            throw new NotImplementedException();
        }

        public void DestroyRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public T Find(long id)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public List<T> GetActives()
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(long id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetDeleteds()
        {
            throw new NotImplementedException();
        }

        public List<T> GetModifieds()
        {
            throw new NotImplementedException();
        }

        public IQueryable<X> Select<X>(Expression<Func<T, X>> selector)
        {
            throw new NotImplementedException();
        }

        public void SetActive(T item)
        {
            throw new NotImplementedException();
        }

        public void SetPassive(T item)
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }

}
