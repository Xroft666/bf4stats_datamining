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

	public TextAsset tupleFile;

	public string PlayerDataFilePath;
	public string fileName;

	public int supportThreshold; // for apriori

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

	public IEnumerator CreateTuples(){
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
		CleanDuplicateTuples();
		SaveTupleDataToFile();
	}


	void CleanDuplicateTuples(){
		
	}

	void FillTuple(Tuple t,PlayerData pd){

		t.playerID = int.Parse(pd.player.id);

		t.setData("timePlayed",pd.stats.timePlayed,0.2f);
		t.setData("score",pd.player.score,0.2f);
		t.setData("kills",pd.stats.kills,0.2f);
		t.setData("deaths",pd.stats.deaths,0.2f);
		t.setData("shotsFired",pd.stats.shotsFired,0.2f);
		t.setData("shotsHit",pd.stats.shotsHit,0.2f);
		t.setData("numLosses",pd.stats.numLosses,0.2f);
		t.setData("numWins",pd.stats.numWins,0.2f);
		t.setData("weaponKills",pd.stats.extra.weaponKills,0.2f);
		t.setData("vehicleKills",pd.stats.extra.vehicleKills,0.2f);
		t.setData("medals",pd.stats.extra.medals,0.2f);
		t.setData("headshots",pd.stats.headshots,0.2f);
		t.setData("suppressionAssists",pd.stats.suppressionAssists,0.2f);
		t.setData("avengerKills",pd.stats.avengerKills,0.2f);
		t.setData("saviorKills",pd.stats.saviorKills,0.2f);
		t.setData("nemesisKills",pd.stats.nemesisKills,0.2f);
		t.setData("resupplies",pd.stats.resupplies,0.2f);
		t.setData("repairs",pd.stats.repairs,0.2f);
		t.setData("heals",pd.stats.heals,0.2f);
		t.setData("revives",pd.stats.revives,0.2f);
		t.setData("killAssists",pd.stats.killAssists,0.2f);

		t.setData("kdr",pd.stats.extra.kdr,0.2f);
		t.setData("wlr",pd.stats.extra.wlr,0.2f);
		t.setData("spm",pd.stats.extra.spm,0.2f);
		t.setData("gspm",pd.stats.extra.gspm,0.2f);
		t.setData("kpm",pd.stats.extra.kpm,0.2f);
		t.setData("sfpm",pd.stats.extra.sfpm,0.2f);
		t.setData("hkp",pd.stats.extra.hkp,0.2f);
		t.setData("khp",pd.stats.extra.khp,0.2f);
		t.setData("accuracy",pd.stats.extra.accuracy,0.2f);


		/*
		t.setData("playerName",pd.player.name);
		t.setData("kills",pd.stats.kills.ToString());
		t.setData("headshots",pd.stats.headshots.ToString());
		t.setData("deaths",pd.stats.deaths.ToString());
		t.setData("dogtagsTaken",pd.stats.dogtagsTaken.ToString());
		t.setData("flagCaptures",pd.stats.flagCaptures.ToString());
		t.setData("flagDefend",pd.stats.flagDefend.ToString());
		*/
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
		print("Done saving tuples");
	}

	public void ReadTupleFile(){
		List<Tuple> t;
		string s = tupleFile.text; 
		t = JsonConvert.DeserializeObject<List<Tuple>>(s);
		tupleList = t;
		print("Done loading file");

		//Normalize.instance.StartNormailzation(t.ToArray());

		SoundReference.instance.PlaySound(SoundReference.instance.Done);
	}

	public void NormalizeTupleFile(){
		ReadTupleFile();
		Normalize.instance.NormalizeTuples(tupleList.ToArray());
		SaveTupleDataToFile();
	}

	public void RunAPrioriFromFile()
	{
		ReadTupleFile();
		Normalize.instance.FactorizeTuples(tupleList.ToArray());
		StartCoroutine(Apriori.apriori(tupleList.ToArray(), supportThreshold));
		//Normalize.instance.UnFactorizeTuples(tupleList.ToArray());
		//SaveTupleDataToFile();
	}

}
