using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DesignPattern.GenericRepository.IntRep
{
    public interface IRepository<T> where T : BaseEntity
    {
        //List commands
        List<T> GetAll();
        List<T> GetActives();
        List<T> GetModifieds();
        List<T> GetDeleteds();
        

        //modify commands
        void Add(T item);
        void AddRange(List<T> list);
        void Delete(T item);//pasife çekeriz
        void DeleteRange(List<T> list);
        void Update (T item);
        void UpdateRange(List<T> list);
        void Destroy(T item);
        void DestroyRange(List<T> list);
        void SetActive(T item);         // Pasif kaydı aktif yap
        void SetPassive(T item);        // Aktif kaydı pasife çek
        //linq commands
        T GetById(long id);                    // Id ile bul
        T Find(long id);                       // Alternatif Find
        List<T> Where(Expression<Func<T, bool>> expression); // Şarta göre listele
        bool Any(Expression<Func<T, bool>> expression);      // Veri var mı?
        T FirstOrDefault(Expression<Func<T, bool>> expression); // İlk uyumlu kayıt
        int Count(Expression<Func<T, bool>> expression);     // Kaç kayıt var?
        IQueryable<X> Select<X>(Expression<Func<T, X>> selector); // DTO'ya dönüştür


    }
}
