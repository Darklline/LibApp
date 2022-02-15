using LibApp.Data;
using LibApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibApp.Repositories
{
    public class MembershipTypesRepository : IMembershipTypesRepository
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MembershipType> GetMembershipTypes()
        {
            return _context.MembershipTypes.ToList();
        }
    }
}
