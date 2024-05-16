using Book.DataAccess.Repository.IRepository;
using Book.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromdb=_db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromdb != null)
            {
                orderFromdb.OrderStatus=orderStatus;
                if(paymentStatus != null)
                {
                    orderFromdb.PaymentStatus=paymentStatus;
                }
            }
        }
        public void UpdateStripePayId(int id, string sessionId, string paymentItemId)
        {
            var orderFromdb=_db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            orderFromdb.PaymentDate = DateTime.Now;
            orderFromdb.SessionId=sessionId;
            orderFromdb.PaymentIntentId=paymentItemId;
        }
       
    }
}

