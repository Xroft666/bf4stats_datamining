﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using Newtonsoft.Json;

public class bf4stats : MonoBehaviour 
{
    //public int numberOfPages = 1;

	public int startPage = 0;
	public int endPage = 1;
	private int currentPage = 0;

	public bool OverwriteExistingFiles = false;

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

	private PlayerData[] playersData = null;

	private Vector2 namesScrollView;
	private Vector2 playerInfoScrollView;
	
	private string output = "";

	private int numPlayersDownloaded = 0;
	

	public string filepath = @"C:\MyApp\MySubDir\Data";

	void Awake()
	{


	}

	void LoadFilesFromHarddrive(){
		//FillPlayerData(File.ReadAllText("C:/Users/Zazà/Desktop/ITU/04_Data Mining/0bf4output.txt"));
		if(File.Exists(filepath+"/bf4output.txt")){
			FillPlayerData(File.ReadAllText(filepath+"/bf4output.txt"));
		}
		else
			Debug.Log("File wasn't found");
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,150,50),"Start Download")){
			StartCoroutine(StartDownload());
		}
		if(GUI.Button(new Rect(151,0,150,50),"Load Files")){
			LoadFilesFromHarddrive();
		}
		if( playersData != null )
		{
			if(GUI.Button(new Rect(302,0,150,50),"Extract training data"))
			{
				ConvertToLearningData();
			}
		}
	}

	IEnumerator StartDownload(){
		Directory.CreateDirectory(filepath);
		yield return StartCoroutine(RetriveNames());
	}

//	IEnumerator Start()
//	{
//		Directory.CreateDirectory(filepath);
//		yield return StartCoroutine(RetriveNames());
//
//	}

    IEnumerator RetriveNames()
    {
        string nameLineStart = "<a href=\"/pc/";
		string nameLineEnd = "</a>";

		print("Starting downloading "+(endPage-startPage)+" pages.");
		//print ("Retriving names...");

        for( int i = 0; i < endPage-startPage; i++ )
		{
			currentPage = startPage+i;
			print("starting downloading page "+currentPage);
			if(!File.Exists(filepath+"/bf4output_page"+currentPage+".txt") || OverwriteExistingFiles)
			{
				using (WWW www = new WWW(leaderboardURI + currentPage) )
				{
					// extracting the html page
	                yield return www;
					
					int startIndex = 0;
					playerNames.Clear();
					while( (startIndex = www.text.IndexOf(nameLineStart, startIndex)) != -1 )
					{
						// gettin to the start of name
						startIndex = www.text.IndexOf("\">", startIndex);
						// getting to the end of name
						int endIndex = www.text.IndexOf(nameLineEnd, startIndex);
						// extracting the name
						playerNames.Add( www.text.Substring(startIndex + 2, endIndex - startIndex - 2) );
	            	}

					foreach(string nome in playerNames)
					{
						//File.AppendAllText("C:/Users/Zazà/Desktop/ITU/04_Data Mining/0bf4output.txt", nome + "\n");
						yield return StartCoroutine(RetrivePlayerInfo(nome));
					//print (output);

					}
					print("------------------writing to file----------------------");
					File.AppendAllText(filepath+"/bf4output_page"+currentPage+".txt", output);
					output = "";
					print("completed downloading page "+currentPage);
				}
			}else{
				print("this page allready exists!");
			}
		}

	

		SoundReference.instance.PlaySound(SoundReference.instance.Done);
		print ("" + numPlayersDownloaded + " names crawled in: " + Time.time + " seconds.");
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
			//currentPlayerData = JsonConvert.DeserializeObject<PlayerData>(www.text.Substring(7, www.text.Length - 8), settings);
			//print (currentPlayerData.player.name);

			//Type objectType = currentPlayerData.GetType();

			//MethodInfo genericRetriveMethod = GetType().GetMethod("RetriveFieldsAndValues").MakeGenericMethod(objectType);
			//genericRetriveMethod.Invoke(this, new object[] { "PlayerInfo", currentPlayerData, 0});


			// Be aware of data capacity.
			// It might not get into because size
			if(www.error == null){
				output += www.text;
				numPlayersDownloaded++;
				print ("Done. "+numPlayersDownloaded+" Players downloaded.");
			}else{
				print("Connection timeout, retrying player "+name);
				Debug.LogWarning("WARNING: Unstable Connection");
				RetrivePlayerInfo(name);
			}




		}
	}



	public void FillPlayerData(string text)
	{
		Debug.Log("start reading file");

		string cleanText = text.Replace("var pd=", "");

		string[] stringSeparators = new string[] {"};"};
		string[] result;


		result = cleanText.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		//Debug.Log(result.Length);
		foreach (string s in result)
		{
			File.AppendAllText(filepath+"/clean bf4 output.txt", s);
			//File.AppendAllText("C:/Users/Zazà/Desktop/ITU/04_Data Mining/0bf4outputADADADAD.txt", s);

		}

		playersData = new PlayerData[result.Length];
		int i = 0;
		foreach (string s in result)
		{
			string s2 = s+"}";
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			
			// substring to ignore "var pd=" line and ";" in the end
			playersData[i] = JsonConvert.DeserializeObject<PlayerData>(s2, settings);
			i++;
			//---------FILLS MEMORY ON BIG DOWNLOAD-------
		
		}

		Debug.Log("finished reading file");
	}


	// For now I am extracting: score, kills, deaths, timeplayed and rank as class 
	public void ConvertToLearningData()
	{
		Debug.Log("Started extracting learning data");
		//FileStream stream = File.OpenWrite(filepath + "/bf4_learning_data.lrn");

		File.AppendAllText(filepath + "/bf4_learning_data.lrn", "%" + playersData.Length + "\n");
		File.AppendAllText(filepath + "/bf4_learning_data.lrn", "%6\n");	// number of columns
		File.AppendAllText(filepath + "/bf4_learning_data.lrn", "%9\t1\t1\t1\t1\t3\n");
		File.AppendAllText(filepath + "/bf4_learning_data.lrn", "%Key\t1\t2\t3\t4\tClass\n");
		for(int i = 0; i < playersData.Length; i++ )
		{
			string line = i.ToString() + "\t" + playersData[i].player.score 
										+ "\t" + playersData[i].stats.kills 
										+ "\t" + playersData[i].stats.deaths
										+ "\t" + playersData[i].stats.timePlayed
										+ "\t" + playersData[i].stats.rank + "\n";

			File.AppendAllText(filepath + "/bf4_learning_data.lrn", line);
		}

		Debug.Log("Finished extracting learning data");
	}

//	void OnGUI()
//	{
//		GUILayout.BeginHorizontal();
//
//		GUILayout.BeginVertical();
//
//		namesScrollView = GUILayout.BeginScrollView(namesScrollView);
//		foreach(string name in playerNames )
//		{
//			if( GUILayout.Button(name) )
//			{
//				StartCoroutine(RetrivePlayerInfo(name));
//			}
//		}
//		GUILayout.EndScrollView();
//
//		GUILayout.EndVertical();
//
//		playerInfoScrollView = GUILayout.BeginScrollView(playerInfoScrollView);
//
//		if( currentPlayerData != null )
//		{
//			GUILayout.Label(output);
//		}
//		GUILayout.EndScrollView();
//
//		GUILayout.EndHorizontal();
//	}

	// Reflection method
	// I made this awesome method just for nothing. It was too late when i realized that.
	// Moreover its not just useless because original JSON is the same,
	// but also if u try to retrive the full data, the recursion will freeze unity for a while
	public void RetriveFieldsAndValues<ReflectionType>( string fieldName, ReflectionType reflectionObject, int depth )
	{
		int localDepth = depth;
		
		if( typeof(ReflectionType).Namespace == "System" )
		{
			//output.Add( new string(' ', localDepth * 5) + fieldName + ": " + reflectionObject );
			output += new string(' ', localDepth * 5) + fieldName + ": " + reflectionObject + "\n";
			return;
		}
		
		if( typeof(ReflectionType).IsArray )
		{
			//output.Add( new string(' ', localDepth * 5) + fieldName + ": [");
			output += new string(' ', localDepth * 5) + fieldName + ": [\n";

			Type itemType = typeof(ReflectionType).GetElementType();
			foreach(object item in (reflectionObject as object[]))
			{
				MethodInfo genericRetriveMethod = GetType().GetMethod("RetriveFieldsAndValues").MakeGenericMethod(itemType);
				genericRetriveMethod.Invoke(this, new object[] { "", item, localDepth + 1});
			}
			
			//output.Add( new string(' ', localDepth * 5) + "]");
			output += new string(' ', localDepth * 5) + "]\n";
			return;
		}
		
		//output.Add( new string(' ', localDepth * 5) + fieldName + ": ");
		output += new string(' ', localDepth * 5) + fieldName + ":\n";
		foreach (FieldInfo fieldInfo in typeof(ReflectionType).GetFields())
		{
			object fieldValue = fieldInfo.GetValue(reflectionObject);
			if( fieldValue == null )
				continue;
			
			MethodInfo genericRetriveMethod = GetType().GetMethod("RetriveFieldsAndValues").MakeGenericMethod(fieldInfo.FieldType);
			genericRetriveMethod.Invoke(this, new object[] { fieldInfo.Name, fieldValue, localDepth + 1});
		}
	}
}
