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
