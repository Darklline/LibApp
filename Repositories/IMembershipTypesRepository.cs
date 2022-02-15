using LibApp.Models;
using System.Collections.Generic;

namespace LibApp.Repositories
{
    public interface IMembershipTypesRepository
    {
        IEnumerable<MembershipType> GetMembershipTypes();
    }
}