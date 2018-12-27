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
			IWebDriver driver = new ChromeDriver();
			driver.Url = "https://oracleselixir.com/statistics/na/na-lcs-2018-spring-regular-season-player-statistics/";
			var finder = driver.FindElements(By.ClassName("search-field"));
			System.Diagnostics.Debug.WriteLine(finder.ElementAt(0).TagName);
			System.Diagnostics.Debug.WriteLine(finder.ElementAt(0).GetAttribute("value"));
			IWebElement team = driver.FindElement(By.XPath("//li[input/@value='TEAM']"));
			team.Click();
			///driver.FindElement(By.ClassName("chosen-results")).FindElement(By.LinkText("Aatrox")).Click();
			IList<IWebElement> teams = driver.FindElements(By.CssSelector(".active-result"));
			Database_Connector dbInfo = new Database_Connector("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;");
			iterateTeams(teams, driver, dbInfo);
			/*foreach (DataRow teamRow in teams.Rows)
			{
				foreach (var item in teamRow.ItemArray)
				{
					System.Diagnostics.Debug.WriteLine(item);
				}
			}*/
			///System.Diagnostics.Debug.WriteLine(champ.ToString());
			System.Threading.Thread.Sleep(3000);
			driver.Close();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
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
				if (currPlayer.Text.Length < 20 && !currPlayers.AsEnumerable().Any(row => currPlayer.Text == row.Field<String>("PLAYER_NAME")))
				{
					dbInfo.fillPlayers(currPlayer.Text);
				}
				//code is dogshit as well, I'll fix it up sometime
				else if (currPlayer.Text.Length > 20)
				{
					//System.Diagnostics.Debug.WriteLine(currPlayer.Text);
					String[] stats = currPlayer.Text.Split();
					rowParser playerStats = new rowParser(stats);
					playerStatRow rowToAdd = playerStats.parseLine(currPlayers, currTeams);
					rowToAdd.printValues();
				}
			}
		}
	}
}
