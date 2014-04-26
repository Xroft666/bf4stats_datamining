using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bf4stats : MonoBehaviour 
{
    public int numberOfPages = 1;
	private List<string> names = new List<string>(); 

    private string leaderboardURI = "http://bf4stats.com/leaderboards/pc_player_score?start=";
	private string playerInfoURI = "http://api.bf4stats.com/api/playerInfo?plat=pc&name=";

	private string currentPlayerInfo = "";

	private Vector2 namesScrollView;
	private Vector2 playerInfoScrollView;

	IEnumerator Start()
	{
		yield return StartCoroutine(RetriveNames());
	}

    IEnumerator RetriveNames()
    {
        string nameLineStart = "<a href=\"/pc/";
		string nameLineEnd = "</a>";

		print ("Retriving names...");

        for( int i = 0; i < numberOfPages; i++ )
            using (WWW www = new WWW(leaderboardURI + i) )
            {
				// extracting the html page
                yield return www;
				
				int startIndex = 0;
				while( (startIndex = www.text.IndexOf(nameLineStart, startIndex)) != -1 )
				{
					// gettin to the start of name
					startIndex = www.text.IndexOf("\">", startIndex);
					// getting to the end of name
					int endIndex = www.text.IndexOf(nameLineEnd, startIndex);
					// extracting the name
					names.Add( www.text.Substring(startIndex + 2, endIndex - startIndex - 2) );
            	}
        }

		print ("" + names.Count + " names crawled in: " + Time.time + " seconds.");
    }

	IEnumerator RetrivePlayerInfo(string name)
	{
		string outputFormat = "&output=js";

		print ("Retriving player info on " + name + "...");

		using (WWW www = new WWW(playerInfoURI + name + outputFormat) )
		{
			yield return www;
			currentPlayerInfo = www.text;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();

		namesScrollView = GUILayout.BeginScrollView(namesScrollView);
		foreach(string name in names )
		{
			if( GUILayout.Button(name) )
			{
				StartCoroutine(RetrivePlayerInfo(name));
			}
		}
		GUILayout.EndScrollView();

		GUILayout.EndVertical();

		playerInfoScrollView = GUILayout.BeginScrollView(playerInfoScrollView);
		GUILayout.Label(currentPlayerInfo);
		GUILayout.EndScrollView();

		GUILayout.EndHorizontal();
	}
}
