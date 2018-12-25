using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace WindowsFormsApplication2
{
	class rowParser
	{
		string[] row;

		public rowParser(string[] lineToParse)
		{
			row = lineToParse;
		}

		public void parseLine(DataTable playerList, DataTable teamList)
		{
			int k = 0;
			//iterate through until you have player
			string rowPlayerName = row[k];
			while (!playerList.AsEnumerable().Any(row => rowPlayerName == row.Field<String>("PLAYER_NAME")))
			{
				k++;
				rowPlayerName += " " + row[k];
			}
			System.Diagnostics.Debug.WriteLine("fullName: " + rowPlayerName);
			k++;
			string rowTeamName = row[k];
			while (!teamList.AsEnumerable().Any(row => rowTeamName == row.Field<String>("SHORT_NAME")))
			{
				k++;
				rowTeamName += " " + row[k];
			}
			System.Diagnostics.Debug.WriteLine("teamName: " + rowTeamName);
			string rowPlayerPos = row[k + 1];
			string rowGames = row[k + 2];
			string winPer = row[k + 3];
			string rowWinPer = row[k + 4];
			string rowKills = row[k + 5];
			string rowDeaths = row[k + 6];
			string rowAssists = row[k + 7];
			string rowKda = row[k + 8];
			string rowKpPer = row[k + 9];
			string rowGdTen = row[k + 10];
			string rowXpdTen = row[k + 11];
			string rowCsdTen = row[k + 12];
			string rowCspm = row[k + 13];
		}
	}
}
