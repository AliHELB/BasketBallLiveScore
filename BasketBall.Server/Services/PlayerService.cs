using BasketBall.Server.Data;
using BasketBall.Server.Models;

namespace BasketBall.Server.Services
{
    public class PlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Player AddPlayer(string firstName, string lastName, int number, int teamId)
        {
            var player = new Player { FirstName = firstName, LastName = lastName, PlayerNumber = number, TeamId = teamId };
            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public Player? GetPlayerById(int id)
        {
            return _context.Players.FirstOrDefault(p => p.PlayerId == id);
        }

        public IEnumerable<Player> GetPlayersByTeamId(int teamId)
        {
            return _context.Players.Where(p => p.TeamId == teamId).ToList();
        }

        public Player? UpdatePlayer(int id, string firstName, string lastName, int number, int teamId)
        {
            var player = _context.Players.FirstOrDefault(p => p.PlayerId == id);
            if (player == null) return null;

            player.FirstName = firstName;
            player.LastName = lastName;
            player.PlayerNumber = number;
            player.TeamId = teamId;

            _context.SaveChanges();
            return player;
        }

        public bool DeletePlayer(int id)
        {
            var player = _context.Players.FirstOrDefault(p => p.PlayerId == id);
            if (player == null) return false;

            _context.Players.Remove(player);
            _context.SaveChanges();
            return true;
        }
    }
}
