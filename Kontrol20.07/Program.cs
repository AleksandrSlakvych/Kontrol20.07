﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrol20._07
{
    public class TeamContext : DbContext
    {
        public TeamContext()
          : base("teamplayerconnection")
        {
            //Database.Log += (str) => Console.WriteLine(str);
            Database.SetInitializer(new PlayerDbInitializer());
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Goal> Goals { get; set; }
    }

    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public int GoalTime { get; set; }
        public virtual Team GoalTeam { get; set; }
        public virtual Player GoalPlayer { get; set; }
    }

    public class Match
    {
        [Key]
        public int Id { get; set; }
        public int TeamARresult { get; set; }
        public int TeamBRresult { get; set; }
        public virtual Team TeamA { get; set; }
        public virtual Team TeamB { get; set; }
        public DateTime Time { get; set; }
        public virtual List<Goal> GoalsInMatch { get; set; }
    }

    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Player> TeamPlayers { get; set; }
    }

    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SureName { get; set; }
        public string TeamID { get; set; }
        public virtual Team Team { get; set; }

    }

    class PlayerDbInitializer : DropCreateDatabaseIfModelChanges<TeamContext>
    {
        protected override void Seed(TeamContext context)
        {
            var player1 = context.Players.Add(new Player() { FirstName = "Bob", SureName = "Felt" });
            var player2 = context.Players.Add(new Player() { FirstName = "John", SureName = "Derive" });
            var player3 = context.Players.Add(new Player() { FirstName = "Arty", SureName = "Messy" });
            var player4 = context.Players.Add(new Player() { FirstName = "Bob", SureName = "Gery" });
            var player5 = context.Players.Add(new Player() { FirstName = "Len", SureName = "Cody" });
            var player6 = context.Players.Add(new Player() { FirstName = "John", SureName = "Maksimus" });
            var player7 = context.Players.Add(new Player() { FirstName = "Leon", SureName = "Marley" });
            var player8 = context.Players.Add(new Player() { FirstName = "Lee", SureName = "Cooper" });
            var player9 = context.Players.Add(new Player() { FirstName = "Michael", SureName = "Shumaher" });
            var player10 = context.Players.Add(new Player() { FirstName = "Maks", SureName = "Fedorish" });
            var player11 = context.Players.Add(new Player() { FirstName = "Vasiliy", SureName = "Vakulenko" });
            var player12 = context.Players.Add(new Player() { FirstName = "Boris", SureName = "Gorbachev" });

            var team1 = context.Teams.Add(new Team() { Name = "Dinamo", TeamPlayers = new List<Player> { player1, player2 } });
            var team2 = context.Teams.Add(new Team() { Name = "Shakhtar", TeamPlayers = new List<Player> { player3, player4 } });
            var team3 = context.Teams.Add(new Team() { Name = "PSG", TeamPlayers = new List<Player> { player5, player6 } });
            var team4 = context.Teams.Add(new Team() { Name = "Shakhtar", TeamPlayers = new List<Player> { player7, player8 } });
            var team5 = context.Teams.Add(new Team() { Name = "Chelsey", TeamPlayers = new List<Player> { player9, player10 } });
            var team6 = context.Teams.Add(new Team() { Name = "Barselona", TeamPlayers = new List<Player> { player11, player12 } });

            var goals1 = context.Goals.Add(new Goal() { GoalTime = 39, GoalPlayer = player2, GoalTeam = team1 });
            var goals2 = context.Goals.Add(new Goal() { GoalTime = 9, GoalPlayer = player6, GoalTeam = team3 });
            var goals3 = context.Goals.Add(new Goal() { GoalTime = 59, GoalPlayer = player8, GoalTeam = team4 });
            var goals4 = context.Goals.Add(new Goal() { GoalTime = 4, GoalPlayer = player9, GoalTeam = team5 });
            var goals5 = context.Goals.Add(new Goal() { GoalTime = 9, GoalPlayer = player10, GoalTeam = team5 });
            var goals6 = context.Goals.Add(new Goal() { GoalTime = 99, GoalPlayer = player9, GoalTeam = team5 });
            var goals7 = context.Goals.Add(new Goal() { GoalTime = 6, GoalPlayer = player11, GoalTeam = team6 });
            var goals8 = context.Goals.Add(new Goal() { GoalTime = 57, GoalPlayer = player11, GoalTeam = team6 });

            var match1 = context.Matches.Add(new Match() { TeamA = team1, TeamARresult = 1, TeamB = team2, TeamBRresult = 0, Time = DateTime.Now, GoalsInMatch = new List<Goal> { goals1 } });
            var match2 = context.Matches.Add(new Match() { TeamA = team3, TeamARresult = 1, TeamB = team4, TeamBRresult = 1, Time = DateTime.Now, GoalsInMatch = new List<Goal> { goals2, goals3 } });
            var match3 = context.Matches.Add(new Match() { TeamA = team5, TeamARresult = 3, TeamB = team6, TeamBRresult = 2, Time = DateTime.Now, GoalsInMatch = new List<Goal> { goals4, goals5, goals6, goals7, goals8 } });
            var match4 = context.Matches.Add(new Match() { TeamA = team1, TeamARresult = 0, TeamB = team6, TeamBRresult = 0, Time = DateTime.Now, GoalsInMatch = null } );

            context.SaveChanges();
            base.Seed(context);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TeamContext())
            {
                var matches = context.Matches.Include(p => p.TeamA).Include(p => p.TeamB).ToList();
                var goals = context.Goals.Include(p => p.GoalTeam).Include(p => p.GoalPlayer).ToList();
                

                foreach (var match in matches)
                {
                    Console.WriteLine($"{match.TeamA.Name}-{match.TeamB.Name} with " +
                        $"{match.TeamARresult}:{match.TeamBRresult} in {match.Time}");

                    var players = context.Players.Include(p => p.Team). ToList();
                    var plaers = match.TeamA.TeamPlayers.Select(x => x).ToList();

                    var allPL = context.Players.Join(context.Teams, o => o.Id, i => i.Id, (p, i) => new { i.Name, p.FirstName, p.SureName }).ToList();

                    foreach (var all in allPL)
                    {
                        Console.WriteLine($"{all.FirstName} {all.SureName}");
                    }
                    //Console.WriteLine($"{match.TeamA.Name} players in this match:");
                    //Console.WriteLine($"{match.TeamA.TeamPlayers.Select(x => x).ToString()}");

                }



                //var matches = context.Matches.Where(x => x.TeamARresult > x.TeamBRresult).ToList();
                //var matches1 = context.Matches.Select(x => x).ToList();

                //foreach (var match in matches1)
                //{
                //    Console.WriteLine($"{match.TeamA.Name}-{match.TeamB.Name} with " +
                //        $"{match.TeamARresult}:{match.TeamBRresult} in {match.Time}");

                //    if (match.GoalsInMatch == null)
                //    {
                //        Console.WriteLine("");
                //    }
                //    else
                //    {
                //        var goals1 = match.GoalsInMatch.Where(x => x.GoalTime > 0).OrderBy(u => u.GoalTime);
                //        foreach (var goal in goals1)
                //        {
                //            if (goal == null)
                //                Console.WriteLine("");

                //            Console.WriteLine($"                 {goal.GoalTeam.Name}-{goal.GoalTime}' {goal.GoalPlayer.FirstName} {goal.GoalPlayer.SureName}");
                //        }
                //    }

                //    var playersA1 = match.TeamA.TeamPlayers.Select(x => x).ToList();
                //    var playersB1 = match.TeamB.TeamPlayers.Select(x => x).ToList();

                //    Console.WriteLine($"{match.TeamA.Name} players in this match:");

                //    foreach (var player in playersA1)
                //    {
                //        Console.WriteLine($"                     {player.FirstName} {player.SureName}");
                //    }

                //    Console.WriteLine($"{match.TeamB.Name} players in this match:");
                //    foreach (var player in playersB1)
                //    {
                //        Console.WriteLine($"                     {player.FirstName} {player.SureName}");
                //    }
                //}
            }
            Console.ReadLine();
        }
    }
}
