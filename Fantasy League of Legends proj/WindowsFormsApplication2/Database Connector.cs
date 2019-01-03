using System;
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

		public Database_Connector(string connect_string)
		{
			this.connect_string = connect_string;
			this.con = new SqlConnection(this.connect_string);
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

		public DataTable getPlayerIdByName(string playerName)
		{
			DataTable playerNameTable = new DataTable();
			string playerNameQuery = "select PLAYER_ID from LeagueOfLegendsStats.dbo.League_Player where PLAYER_NAME like @playerName";
			SqlCommand command = new SqlCommand(playerNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@playerName", playerName);
			SqlDataAdapter data = new SqlDataAdapter(command);
			data.Fill(playerNameTable);
			this.con.Close();
			data.Dispose();
			return playerNameTable;
		}

		public DataTable getTeamIdByName(string teamName)
		{
			DataTable teamNameTable = new DataTable();
			string teamNameQuery = "select TEAM_ID from LeagueOfLegendsStats.dbo.league_team where TEAM_NAME like @teamName";
			SqlCommand command = new SqlCommand(teamNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@teamName", teamName);
			SqlDataAdapter data = new SqlDataAdapter(command);
			data.Fill(teamNameTable);
			this.con.Close();
			data.Dispose();
			return teamNameTable;
		}

		public DataTable getPosIdByName(string posName)
		{
			DataTable posNameTable = new DataTable();
			string posNameQuery = "select POSITION_ID from LeagueOfLegendsStats.dbo.league_position where POSITION_DISP_NAME like @posName";
			SqlCommand command = new SqlCommand(posNameQuery, this.con);
			this.con.Open();

			command.Parameters.AddWithValue("@posName", posName);
			SqlDataAdapter data = new SqlDataAdapter(command);
			data.Fill(posNameTable);
			this.con.Close();
			data.Dispose();
			return posNameTable;
		}

		//Need to create global season table with spring / summer split for this one
		public DataTable getCurrentSplitID()
		{
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
		}
	}
}
