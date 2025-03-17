using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;
using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.DTO;


namespace FitnessTracker.Repositories
{
    public class MemberRepository : IMember
    {
        private readonly FitnessTrackerContext db;
        public MemberRepository(FitnessTrackerContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<Member>> GetAllMembers()
        {
            return await db.Members.ToListAsync();
        }

        public async Task<bool> AddMember(MemberDTO memberDto)
        {
            var res = await db.Members.FirstOrDefaultAsync(m => m.Id == memberDto.Id);
            if (res != null)
            {
                return false;
            }
            var member = new Member
            {
                Id = memberDto.Id,
                Name = memberDto.Name,
                Email = memberDto.Email,
                MembershipStartDate = memberDto.MembershipStartDate
            };
            await db.Members.AddAsync(member);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMemberEmail(int id, string email)
        {
            var res = await db.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (res != null)
            {
                res.Email = email;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMember(int memberId)
        {
            var res = await db.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (res != null)
            {
                db.Members.Remove(res);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Member>> GetTopMembersWithHighestCaloriesBurned(int top)
        {
            IEnumerable<Member> res = await db.Members.Select(member => new
            {
                Member = member,
                TotalCaloriesBurned = member.WorkoutSessions.Sum(ws => ws.CaloriesBurned)
            }).OrderByDescending(m => m.TotalCaloriesBurned).Take(top).Select(m => m.Member).ToListAsync();
            return res;
        }
    }
}