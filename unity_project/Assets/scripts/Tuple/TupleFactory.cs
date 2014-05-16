﻿using UnityEngine;
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

	public TextAsset tupleFile;

	public string PlayerDataFilePath;
	public string fileName;

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
		if(GUI.Button(new Rect(250,250,250,50), "load tuples from file "+tupleFile.name)){
			//print("TEST");
			ReadTupleFile();
		}
	}

	IEnumerator CreateTuples(){
		for(int i=0;i<endPage-startPage;i++){
			currentPage = startPage+i;
			string fileName = PlayerDataFilePath+"/bf4output_page"+currentPage+".txt";
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
				SoundReference.instance.PlaySound(SoundReference.instance.BigProgress);
				yield return new WaitForEndOfFrame();
			}else{
				Debug.Log("File wasn't found");
			}
		}

		SaveTupleDataToFile();
	}

	void FillTuple(Tuple t,PlayerData playerData){
		t.setData("playerName",playerData.player.name);
		t.setData("kills",playerData.stats.kills.ToString());
		t.setData("headshots",playerData.stats.headshots.ToString());
		t.setData("deaths",playerData.stats.deaths.ToString());
		t.setData("dogtagsTaken",playerData.stats.dogtagsTaken.ToString());
		t.setData("flagCaptures",playerData.stats.flagCaptures.ToString());
		t.setData("flagDefend",playerData.stats.flagDefend.ToString());
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

	void SaveTupleDataToFile(){
		string toWrite = JsonConvert.SerializeObject(tupleList);
		Directory.CreateDirectory(Application.dataPath+"/Data/Tuples/");
		File.WriteAllText(Application.dataPath+"/Data/Tuples/"+fileName+".txt",toWrite);
		tupleList = new List<Tuple>();
		SoundReference.instance.PlaySound(SoundReference.instance.Done);
		print("Done creating tuples");
	}

	void ReadTupleFile(){
		List<Tuple> t;
		string s = tupleFile.text; 
		t = JsonConvert.DeserializeObject<List<Tuple>>(s);
		tupleList = t;
		print("Done loading file");
		SoundReference.instance.PlaySound(SoundReference.instance.Done);
	}


}
