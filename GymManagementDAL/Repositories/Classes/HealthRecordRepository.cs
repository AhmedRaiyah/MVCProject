using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class HealthRecordRepository
    {
        private readonly GymDBContext _context;

        public HealthRecordRepository(GymDBContext context)
        {
            _context = context;
        }
        public int Add(HealthRecord healthRecord)
        {
            _context.Add(healthRecord);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var healthRecord = GetById(id);

            if (healthRecord is null)
                return 0;

            _context.Remove(healthRecord);
            return _context.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetAll() => _context.HealthRecords.ToList();

        public HealthRecord? GetById(int id) => _context.HealthRecords.Find(id);

        public int Update(HealthRecord healthRecord)
        {
            _context.Update(healthRecord);
            return _context.SaveChanges();
        }
    }
}
