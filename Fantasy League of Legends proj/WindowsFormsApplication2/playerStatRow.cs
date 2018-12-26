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

		}
	}
}
