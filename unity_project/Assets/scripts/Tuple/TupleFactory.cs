using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using System.Linq;
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
				playersData = null;
				GC.Collect();
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

		float time = pd.stats.timePlayed;
		float kills = pd.stats.kills;
		float rounding = 0.1f;
		// EVERYTHING

		//-------------General perfomance stats in a ratio based on time
		t.setData("timePlayed",pd.stats.timePlayed,rounding);
		t.setData("score",pd.player.score/time,rounding);
		t.setData("kills",pd.stats.kills/time,rounding);
		t.setData("deaths",pd.stats.deaths/time,rounding);
		t.setData("shotsFired",pd.stats.shotsFired/time,rounding);
		t.setData("shotsHit",pd.stats.shotsHit/time,rounding);
		t.setData("numLosses",pd.stats.numLosses/time,rounding);
		t.setData("numWins",pd.stats.numWins/time,rounding);
		t.setData("weaponKills",pd.stats.extra.weaponKills/kills,rounding);
		t.setData("vehicleKills",pd.stats.extra.vehicleKills/kills,rounding);
		t.setData("medals",pd.stats.extra.medals/time,rounding);
		t.setData("headshots",pd.stats.headshots/kills,rounding);
		t.setData("suppressionAssists",pd.stats.suppressionAssists/time,rounding);
		t.setData("avengerKills",pd.stats.avengerKills/kills,rounding);
		t.setData("saviorKills",pd.stats.saviorKills/kills,rounding);
		t.setData("nemesisKills",pd.stats.nemesisKills/kills,rounding);
		t.setData("resupplies",pd.stats.resupplies/time,rounding);
		t.setData("repairs",pd.stats.repairs/time,rounding);
		t.setData("heals",pd.stats.heals/time,rounding);
		t.setData("revives",pd.stats.revives/time,rounding);
		t.setData("killAssists",pd.stats.killAssists/time,rounding);

		t.setData("kdr",pd.stats.extra.kdr,rounding);
		t.setData("wlr",pd.stats.extra.wlr,rounding);
		t.setData("spm",pd.stats.extra.spm,rounding);
		t.setData("gspm",pd.stats.extra.gspm,rounding);
		t.setData("kpm",pd.stats.extra.kpm,rounding);
		t.setData("sfpm",pd.stats.extra.sfpm,rounding);
		t.setData("hkp",pd.stats.extra.hkp,rounding);
		t.setData("khp",pd.stats.extra.khp,rounding);
		t.setData("accuracy",pd.stats.extra.accuracy,rounding);


		/*//----------------- attempt to extract best guns
		List<PlayerData.Weapon.Stat> topWep = new List<PlayerData.Weapon.Stat>();
		for(int i=0;i< pd.weapons.Length;i++){
			topWep.Add(pd.weapons[i].stat);
		}
		topWep.OrderByDescending(x=>x.kills);
		t.setData("gun_"+0,float.Parse(topWep[0].id),0.2f);
		t.setData("gun_"+1,float.Parse(topWep[1].id),0.2f);
		t.setData("gun_"+2,float.Parse(topWep[2].id),0.2f);
		t.setData("gun_"+3,float.Parse(topWep[3].id),0.2f);
		t.setData("gun_"+4,float.Parse(topWep[4].id),0.2f);
		*/


		// BEHAVIOUR CLASSIFICATION

		t.setData ("Pilot", pd.stats.extra.vehKillsP, 0.2f);
		t.setData ("Trooper", pd.stats.extra.weaKillsP, 0.2f);
		t.setData ("Officer", pd.stats.extra.ribbons +
		           				pd.stats.extra.medals +
		           				pd.stats.extra.assignments, 0.2f );
		t.setData ("AntiArmorTech", pd.stats.vehicleDamage, 0.2f);
		t.setData ("Assistance", pd.stats.resupplies +
						pd.stats.repairs +
						pd.stats.heals +
		           		pd.stats.revives, 0.2f);
		t.setData("TeamPlayer", (float)(pd.stats.avengerKills +
		          				pd.stats.saviorKills +
		                         pd.stats.nemesisKills) / (float) pd.stats.kills, 0.2f);
		t.setData ("Leader", pd.stats.flagCaptures + pd.stats.flagDefend, 0.2f);


		// WEAPON SUGGESTION

		/*
		foreach( PlayerData.Kititem kitItem in pd.kititems )
			foreach( PlayerData.Weapon weapon in pd.weapons )
			{
				t.setData ("Kit class", System.Convert.ToInt32(kitItem.detail.id), 0.2f);
				t.setData ("Weapon", System.Convert.ToInt32(weapon.detail.id), 0.2f);
				t.setData ("Kills per Minute", weapon.extra.kpm, 0.2f);
			}
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
		//tupleList = new List<Tuple>();
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
