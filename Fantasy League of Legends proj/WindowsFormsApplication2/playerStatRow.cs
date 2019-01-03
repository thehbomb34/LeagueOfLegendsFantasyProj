using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
	class playerStatRow
	{
		private string player;
		private string team;
		private string position;
		private int games;
		private float winPer;
		private int kills;
		private int deaths;
		private int assists;
		private float kda;
		private float kpPer;
		private int gdTen;
		private int xpdTen;
		private float csdTen;
		private float cspm;
		
		public playerStatRow(string player, string team, string position, int games, float winPer, int kills, int deaths, int assists, 
								float kda, float kpPer, int gdTen, int xpdTen, float csdTen, float cspm)
		{
			this.player = player;
			this.team = team;
			this.position = position;
			this.games = games;
			this.winPer = winPer;
			this.kills = kills;
			this.deaths = deaths;
			this.assists = assists;
			this.kda = kda;
			this.kpPer = kpPer;
			this.gdTen = gdTen;
			this.xpdTen = xpdTen;
			this.csdTen = csdTen;
			this.cspm = cspm;
		}

		public void printValues()
		{
			System.Diagnostics.Debug.WriteLine("player: " + this.player + "\n" +
												"team: " + this.team + "\n" +
												"position: " + this.position + "\n" +
												"games: " + this.games + "\n" +
												"win percentage: " + this.winPer + "\n" +
												"kills: " + this.kills + "\n" +
												"deaths: " + this.deaths + "\n" +
												"assists: " + this.assists + "\n" +
												"K/D/A: " + this.kda + "\n" +
												"kpPer: " + this.kpPer + "\n" +
												"gold diff Ten: " + this.gdTen + "\n" +
												"xp diff Ten: " + this.xpdTen + "\n" +
												"cs diff Ten: " + this.csdTen + "\n" +
												"cs per min: " + this.cspm + "\n");
		}
	}
}
