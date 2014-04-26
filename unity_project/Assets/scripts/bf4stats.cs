﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

public class bf4stats : MonoBehaviour 
{
    public int numberOfPages = 1;

	// include the following data
	public bool imagePaths = true;
	public bool details = true;
	public bool names = true;
	public bool progress = true;
	public bool extra = true;
	public bool stats = true;
	public bool weapons = true;
	public bool weaponUnlocks = true;
	public bool weaponCategory = true;
	public bool vehicles = true;
	public bool vehicleCategory = true;
	public bool vehicleUnlocks = true;	
	public bool kititems = true;		
	public bool awards = true;	
	public bool dogtags = true;		
	public bool assignments = true;		
	public bool upcomingUnlocks = true;
	public bool urls = true;
				


	private List<string> playerNames = new List<string>(); 

    private string leaderboardURI = "http://bf4stats.com/leaderboards/pc_player_score?start=";
	private string playerInfoURI = "http://api.bf4stats.com/api/playerInfo?plat=pc&name=";

	private PlayerData currentPlayerData = null;

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
					playerNames.Add( www.text.Substring(startIndex + 2, endIndex - startIndex - 2) );
            	}
        }

		print ("" + playerNames.Count + " names crawled in: " + Time.time + " seconds.");
    }

	IEnumerator RetrivePlayerInfo(string name)
	{
		string outputFormat = "&output=js";

		string optParams = "&opt=";

		if(imagePaths) optParams+= "imagePaths,";
		if(details) optParams+= "details,";
		if(names) optParams+= "names,";
		if(progress) optParams+= "progress,";
		if(extra) optParams+= "extra,";
		if(stats) optParams+= "stats,";
		if(weapons) optParams+= "weapons,";
		if(weaponUnlocks) optParams+= "weaponUnlocks,";
		if(weaponCategory) optParams+= "weaponCategory,";
		if(vehicles) optParams+= "vehicles,";
		if(vehicleCategory) optParams+= "vehicleCategory,";
		if(vehicleUnlocks) optParams+= "vehicleUnlocks,";
		if(kititems) optParams+= "kititems,";
		if(awards) optParams+= "award,";
		if(dogtags) optParams+= "dogtags,";
		if(assignments) optParams+= "assignments,";
		if(upcomingUnlocks) optParams+= "upcomingUnlocks,";
		if(urls) optParams+= "urls,";


		print ("Retriving player info on " + name + "...");

		using (WWW www = new WWW(playerInfoURI + name + outputFormat + optParams.Substring(0, optParams.Length-1) ) )
		{
			yield return www;

			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;

			// substring to ignore "var pd=" line and ";" in the end
			currentPlayerData = JsonConvert.DeserializeObject<PlayerData>(www.text.Substring(7, www.text.Length - 8), settings);

		}
		print ("Done.");
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();

		namesScrollView = GUILayout.BeginScrollView(namesScrollView);
		foreach(string name in playerNames )
		{
			if( GUILayout.Button(name) )
			{
				StartCoroutine(RetrivePlayerInfo(name));
			}
		}
		GUILayout.EndScrollView();

		GUILayout.EndVertical();

		playerInfoScrollView = GUILayout.BeginScrollView(playerInfoScrollView);

		if( currentPlayerData != null )
		{
			GUILayout.Label("Name: " + currentPlayerData.player.name);
			GUILayout.Label("Country: " + currentPlayerData.player.countryName);
			GUILayout.Label("Score: " + currentPlayerData.player.score);
		}
		GUILayout.EndScrollView();

		GUILayout.EndHorizontal();
	}
}
