using BasketBall.Server.Data;
using BasketBall.Server.DTOs;
using BasketBall.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasketBall.Server.Services
{
    public class StartingFiveService
    {
        private readonly ApplicationDbContext _context;

        public StartingFiveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddStartingFive(int matchId, int teamId, List<int> playerIds)
        {
            foreach (var playerId in playerIds)
            {
                var startingFive = new StartingFive { MatchId = matchId, TeamId = teamId, PlayerId = playerId };
                _context.StartingFives.Add(startingFive);
            }
            _context.SaveChanges();
        }

        public List<StartingFive> GetAllStartingFives()
        {
            return _context.StartingFives.ToList();
        }

        public StartingFive? GetStartingFiveById(int id)
        {
            return _context.StartingFives.FirstOrDefault(sf => sf.StartingFiveId == id);
        }
        public List<StartingFive> GetStartingFivesByMatchId(int matchId)
        {
            return _context.StartingFives
                           .Where(sf => sf.MatchId == matchId)
                           .ToList();
        }

        public List<StartingFive> GetPlayersByTeam(int matchId, int teamId)
        {
            return _context.StartingFives
                           .Where(sf => sf.MatchId == matchId && sf.TeamId == teamId)
                           .ToList();
        }
        public void UpdatePlayerInStartingFive(int matchId, int teamId, int playerId, StartingFiveDTO dto)
        {
            var existingRecord = _context.StartingFives
                                         .FirstOrDefault(sf => sf.MatchId == matchId && sf.TeamId == teamId && sf.PlayerId == playerId);
            if (existingRecord != null)
            {
                existingRecord.MatchId = dto.MatchId;
                existingRecord.TeamId = dto.TeamId;
                existingRecord.PlayerId = dto.PlayerIds.FirstOrDefault(); 
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Record not found.");
            }
        }


        public void UpdateStartingFive(int id, StartingFiveDTO dto)
        {
            var existingStartingFive = _context.StartingFives.FirstOrDefault(sf => sf.StartingFiveId == id);
            if (existingStartingFive != null)
            {
                existingStartingFive.MatchId = dto.MatchId;
                existingStartingFive.TeamId = dto.TeamId;
                _context.SaveChanges();
            }
        }

        public void DeleteStartingFive(int id)
        {
            var startingFive = _context.StartingFives.FirstOrDefault(sf => sf.StartingFiveId == id);
            if (startingFive != null)
            {
                _context.StartingFives.Remove(startingFive);
                _context.SaveChanges();
            }
        }
    }
}
