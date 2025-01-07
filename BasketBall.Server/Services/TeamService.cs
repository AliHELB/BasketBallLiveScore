using BasketBall.Server.Data;
using BasketBall.Server.Models;

namespace BasketBall.Server.Services
{
    public class TeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Team AddTeam(string name, string coach)
        {
            var team = new Team { TeamName = name, CoachName = coach };
            _context.Teams.Add(team);
            _context.SaveChanges();
            return team;
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }

        public Team? GetTeamById(int id)
        {
            return _context.Teams.FirstOrDefault(t => t.TeamId == id);
        }

        public Team? UpdateTeam(int id, string name, string coach)
        {
            var team = _context.Teams.FirstOrDefault(t => t.TeamId == id);
            if (team == null) return null;

            team.TeamName = name;
            team.CoachName = coach;

            _context.SaveChanges();
            return team;
        }

        public bool DeleteTeam(int id)
        {
            var team = _context.Teams.FirstOrDefault(t => t.TeamId == id);
            if (team == null) return false;

            _context.Teams.Remove(team);
            _context.SaveChanges();
            return true;
        }
    }
}
