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

		public playerStatRow parseLine(DataTable playerList, DataTable teamList)
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
			int rowGames = Int32.Parse(row[k + 2]);
			float rowWinPer = parsePercentageToDecimal(row[k + 3].Replace("-", "0.0%"));
			int rowKills = Int32.Parse(row[k + 4]);
			int rowDeaths = Int32.Parse(row[k + 5]);
			int rowAssists = Int32.Parse(row[k + 6]);
			float rowKda = float.Parse(row[k + 7]);
			float rowKpPer = parsePercentageToDecimal(row[k + 8].Replace("-", "0.0%"));
			int rowGdTen = Int32.Parse(row[k + 11].Replace(",", string.Empty));
			int rowXpdTen = Int32.Parse(row[k + 12].Replace(",", string.Empty));
			float rowCsdTen = float.Parse(row[k + 13]);
			float rowCspm = float.Parse(row[k + 14]);
			return new playerStatRow(rowPlayerName, rowTeamName, rowPlayerPos,
												rowGames, rowWinPer, rowKills, rowDeaths, rowAssists,
												rowKda, rowKpPer, rowGdTen, rowXpdTen, rowCsdTen, rowCspm);

		}

		public static float parsePercentageToDecimal(string row)
		{
			return float.Parse(row.Replace("%", string.Empty)) / 100;
		}
	}
}
