using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Data;

namespace WindowsFormsApplication2
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			parsePage("https://oracleselixir.com/statistics/na/na-lcs-2018-spring-regular-season-player-statistics/", 1);
			parsePage("https://oracleselixir.com/statistics/eu/eu-lcs-2018-summer-regular-season-player-statistics/", 2);
			parsePage("https://oracleselixir.com/statistics/lck/lck-2018-summer-regular-season-player-statistics/", 3);
			parsePage("https://oracleselixir.com/statistics/lms/lms-2018-summer-regular-season-player-statistics/", 5);
			parsePage("https://oracleselixir.com/statistics/lpl/lpl-2018-summer-regular-season-player-statistics/", 4);
		}

		private static void parsePage(string url, int regionId)
		{
			IWebDriver driver = new ChromeDriver();
			driver.Url = url;
			var finder = driver.FindElements(By.ClassName("search-field"));
			System.Diagnostics.Debug.WriteLine(finder.ElementAt(0).TagName);
			System.Diagnostics.Debug.WriteLine(finder.ElementAt(0).GetAttribute("value"));
			IWebElement team = driver.FindElement(By.XPath("//li[input/@value='TEAM']"));
			team.Click();
			IList<IWebElement> teams = driver.FindElements(By.CssSelector(".active-result"));
			Database_Connector dbInfo = new Database_Connector("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;", regionId);
			iterateTeams(teams, driver, dbInfo);
			System.Threading.Thread.Sleep(3000);
			driver.Close();
		}

		private static void iterateTeams(IList<IWebElement> teams, IWebDriver driver, Database_Connector dbInfo)
		{
			DataTable currTeams = dbInfo.getTeams();
			for (int i = 0; i < teams.Count; i++)
			{
				IWebElement currTeam = driver.FindElements(By.CssSelector(".active-result")).ElementAt(i);
				currTeam.Click();
				driver.FindElement(By.XPath("//li[input/@value='TEAM']")).Click();
				IList<IWebElement> players = driver.FindElements(By.XPath("//table/tbody/tr"));
				iteratePlayers(players, driver, dbInfo);
				driver.FindElement(By.ClassName("search-choice-close")).Click();
			}
		}

		private static void iteratePlayers(IList<IWebElement> players, IWebDriver driver, Database_Connector dbInfo)
		{
			DataTable currPlayers = dbInfo.getPlayers();
			DataTable currTeams = dbInfo.getTeams();
			for (int j = 0; j < players.Count; j++)
			{
				IWebElement currPlayer = players[j];
				//Worst code I've ever written, but this trims out the second table results with just names

				//Really need to separate these into two separate callable methods
				//and make it so that it doesn't insert duplicates
				/*if (currPlayer.Text.Length < 20 && !currPlayers.AsEnumerable().Any(row => currPlayer.Text == row.Field<String>("PLAYER_NAME")))
				{
					dbInfo.fillPlayers(currPlayer.Text);
				}*/
				//code is dogshit as well, I'll fix it up sometime
				if (currPlayer.Text.Length > 20)
				{
					String[] stats = currPlayer.Text.Split();
					rowParser playerStats = new rowParser(stats);
					playerStatRow rowToAdd = playerStats.parseLine(currPlayers, currTeams);
					rowToAdd.printValues();
					int playerId = dbInfo.getPlayerIdByName(rowToAdd.player);
					int teamId = dbInfo.getTeamIdByName(rowToAdd.team);
					int posId = dbInfo.getPosIdByName(rowToAdd.position);
					rowToAdd.setPlayerId(playerId);
					rowToAdd.setTeamId(teamId);
					rowToAdd.setPosId(posId);
					dbInfo.fillPlayerStats(rowToAdd);
				}
			}
		}
	}
}
