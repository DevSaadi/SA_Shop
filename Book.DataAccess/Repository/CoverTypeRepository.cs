using Book.DataAccess.Repository.IRepository;
using Book.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db):base (db) 
        {
            _db = db;
        }
        //public void Save()//will be removed
        //{
        //    _db.SaveChanges();
        //}

        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
