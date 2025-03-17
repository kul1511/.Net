using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;
using FitnessTracker.DTO;

namespace FitnessTracker.Repositories
{
    public interface IMember
    {
        Task<IEnumerable<Member>> GetAllMembers();
        Task<bool> AddMember(MemberDTO member);
        Task<bool> UpdateMemberEmail(int id, string email);
        Task<bool> DeleteMember(int memberId);
        Task<IEnumerable<Member>> GetTopMembersWithHighestCaloriesBurned(int topN);

    }
}