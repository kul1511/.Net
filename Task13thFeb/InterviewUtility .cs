using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task13thFeb
{
    public class InterviewUtility
    {
        public bool Register(string fullName, string skills)
        {
            if (Program.CandidatesSet.Any(x => x.FullName == fullName))
            {
                return false;
            }
            // else{
            Program.CandidatesSet.Add(new Candidate { FullName = fullName, Skills = skills });
            // }
            return true;
        }
        public bool UpdateCandidateSkills(string fullName, string newSkills)
        {
            var cand = Program.CandidatesSet.FirstOrDefault(x => x.FullName == fullName);
            if (cand == null)
            {
                return false;
            }
            cand.Skills = newSkills;
            return true;
        }
        public List<string> MarkCandidateAsQualified(string requiredSkills)
        {
            List<string> res = new List<string>();
            foreach (var v in Program.CandidatesSet)
            {
                if (v.Skills == requiredSkills)
                {
                    res.Add(v.FullName);
                    v.IsQualified = true;
                }
            }
            return res;
        }
    }
}