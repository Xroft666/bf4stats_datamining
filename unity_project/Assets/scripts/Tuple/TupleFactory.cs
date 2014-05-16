using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using Newtonsoft.Json;

public class TupleFactory : MonoBehaviour {

	public int startPage = 0;
	public int endPage = 1;
	private int currentPage = 0;

	public string filepath;

	public List<Tuple> tupleList = new List<Tuple>();

	/*
	public IncludeData includeData;

	[System.Serializable]
	public class IncludeData{

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
	}*/

	public static TupleFactory instance;
	void Awake(){
		instance = this;

	}

	void Start(){
		//filepath = bf4stats.instance.filepath;
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,250,250,50), "create tuples "+startPage+" "+endPage)){
			//print("TEST");
			StartCoroutine(CreateTuples());
		}
	}

	IEnumerator CreateTuples(){
		for(int i=0;i<endPage-startPage;i++){
			currentPage = startPage+i;
			string fileName = filepath+"/bf4output_page"+currentPage+".txt";
			if(File.Exists(fileName)){
				PlayerData[] playersData = null;
				string dataString = File.ReadAllText(fileName);
				playersData = FillPlayerData(dataString);

				for(int j=0;j<playersData.Length;j++){
					Tuple tuple = new Tuple();
					FillTuple(tuple,playersData[j]);
					tupleList.Add(tuple);
				}

				print("created tuples from file "+currentPage);
				yield return new WaitForEndOfFrame();
			}else{
				Debug.Log("File wasn't found");
			}
		}
	}

	void FillTuple(Tuple t,PlayerData playerData){
		t.setData("playerName",playerData.player.name);
		t.setData("kills",playerData.stats.kills.ToString());
	}

	public PlayerData[] FillPlayerData(string text)
	{
		PlayerData[] playersData;
				
		string cleanText = text.Replace("var pd=", "");
		
		string[] stringSeparators = new string[] {"};"};
		string[] result;

		result = cleanText.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		
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
		}
		return playersData;
	}
}
