﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WindowsFormsApplication2
{
	class Database_Connector
	{
		private string connect_string;
		private SqlConnection con;
		private int region_id;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connect_string">da thing to connect to the database</param>
		/// <param name="region_id">need to pass in a region id to get right season and whatnot</param>
		public Database_Connector(string connect_string, int region_id)
		{
			this.connect_string = connect_string;
			this.con = new SqlConnection(this.connect_string);
			this.region_id = region_id;
		}

		public void cleanPlayerStatsTable(int region_id)
		{
			string playerQuery = "delete from LeagueOfLegendsStats.dbo.league_player_split_stats " +
									"where season_id = (select distinct sea.season_id " +
															"from LeagueOfLegendsStats.dbo.global_season sea, LeagueOfLegendsStats.dbo.league_player_split_stats ps " +
															"where sea.SEASON_ID = ps.season_id " +
															"and sea.REGION_ID = @region_id " +
															"and sea.IS_CURRENT_SEASON = 'Y'); ";
			SqlCommand command = new SqlCommand(playerQuery, this.con);
			this.con.Open();
			command.Parameters.AddWithValue("@region_id", region_id);

			command.ExecuteNonQuery();
			this.con.Close();
		}

		public DataTable getTeams()
		{
			DataTable teamTable = new DataTable();
			string teamQuery = "select distinct SHORT_NAME from LeagueOfLegendsStats.dbo.league_team";
			SqlCommand command = new SqlCommand(teamQuery, this.con);
			this.con.Open();

			SqlDataAdapter data = new SqlDataAdapter(command);
			data.Fill(teamTable);
			this.con.Close();
			data.Dispose();
			return teamTable;
		}

		public int getPlayerIdByName(string playerName)
		{
			int playerId;
			string playerNameQuery = "select PLAYER_ID from LeagueOfLegendsStats.dbo.League_Player where PLAYER_NAME like @playerName";
			SqlCommand command = new SqlCommand(playerNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@playerName", playerName);
			playerId = Convert.ToInt32(command.ExecuteScalar());
			this.con.Close();
			return playerId;
		}

		public int getTeamIdByName(string teamName)
		{
			int teamId;
			string teamNameQuery = "select TEAM_ID from LeagueOfLegendsStats.dbo.league_team where TEAM_NAME like @teamName";
			SqlCommand command = new SqlCommand(teamNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@teamName", teamName);
			teamId = Convert.ToInt32(command.ExecuteScalar());
			this.con.Close();
			return teamId;
		}

		public int getPosIdByName(string posName)
		{
			System.Diagnostics.Debug.WriteLine(posName);
			int posId;
			string posNameQuery = "select POSITION_ID from LeagueOfLegendsStats.dbo.league_position where POSITION_DISP_NAME = @posName";
			SqlCommand command = new SqlCommand(posNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@posName", posName);
			posId = Convert.ToInt32(command.ExecuteScalar());
			this.con.Close();
			return posId;
		}

		public int getCurrentSplitID()
		{
			int splitID;
			string splitQuery = "select SPLIT_ID from LeagueOfLegendsStats.dbo.SEASON_SPLIT " +
								"where IS_CURRENT_SPLIT = 'Y'";
			SqlCommand command = new SqlCommand(splitQuery, this.con);
			this.con.Open();

			splitID = Convert.ToInt32(command.ExecuteScalar());
			this.con.Close();
			return splitID;
		}

		public int getSeasonID()
		{
			int seasonID;
			string seasonQuery = "SELECT SEASON_ID FROM LeagueOfLegendsStats.dbo.global_season " +
								 " where REGION_ID = @regionID and IS_CURRENT_SEASON = 'Y'";
			SqlCommand command = new SqlCommand(seasonQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@regionID", this.region_id);
			seasonID = Convert.ToInt32(command.ExecuteScalar());
			this.con.Close();
			return seasonID;
		}

		public DataTable getPlayers()
		{
			DataTable playersTable = new DataTable();
			string playerQuery = "select distinct PLAYER_NAME from LeagueOfLegendsStats.dbo.League_Player";
			SqlCommand command = new SqlCommand(playerQuery, this.con);
			this.con.Open();

			SqlDataAdapter data = new SqlDataAdapter(command);
			data.Fill(playersTable);
			this.con.Close();
			data.Dispose();
			return playersTable;
		}

		public void fillPlayers(string playerName)
		{
			string playerQuery = "insert into LeagueOfLegendsStats.dbo.League_Player";
			playerQuery += " values ((select max(PLAYER_ID) + 1 from LeagueOfLegendsStats.dbo.League_Player), @playerName, null, null)";
			SqlCommand command = new SqlCommand(playerQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@playerName", playerName);
			command.ExecuteNonQuery();
			this.con.Close();
		}

		public void fillPlayerStats(playerStatRow addRow)
		{
			int splitId = this.getCurrentSplitID();
			int seasonId = this.getSeasonID();
			string plyStQuery = "insert into LeagueOfLegendsStats.dbo.league_player_split_stats";
			plyStQuery += " values (@playerId, @teamId, @splitId, @positionId, @seasonId, @gamesPlayed, @winPercentage, " +
									"@kills, @deaths, @assists, @kda, @killPartPer, @goldDiffTen, @xpDiffTen, @csDiffTen, @csPerMin)";
			SqlCommand command = new SqlCommand(plyStQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@playerId", addRow.playerId);
			command.Parameters.AddWithValue("@teamId", addRow.teamId);
			command.Parameters.AddWithValue("@splitId", splitId);
			command.Parameters.AddWithValue("@positionId", addRow.posId);
			command.Parameters.AddWithValue("@seasonId", seasonId);
			command.Parameters.AddWithValue("@gamesPlayed", addRow.games);
			command.Parameters.AddWithValue("@winPercentage", Convert.ToDouble(addRow.winPer));
			command.Parameters.AddWithValue("@kills", addRow.kills);
			command.Parameters.AddWithValue("@deaths", addRow.deaths);
			command.Parameters.AddWithValue("@assists", addRow.assists);
			command.Parameters.AddWithValue("@kda", Convert.ToDouble(addRow.kda));
			command.Parameters.AddWithValue("@killPartPer", Convert.ToDouble(addRow.kpPer));
			command.Parameters.AddWithValue("@goldDiffTen", addRow.gdTen);
			command.Parameters.AddWithValue("@xpDiffTen", addRow.xpdTen);
			command.Parameters.AddWithValue("@csDiffTen", Convert.ToDouble(addRow.csdTen));
			command.Parameters.AddWithValue("@csPerMin", Convert.ToDouble(addRow.cspm));
			command.ExecuteNonQuery();
			this.con.Close();
		}
	}
}
