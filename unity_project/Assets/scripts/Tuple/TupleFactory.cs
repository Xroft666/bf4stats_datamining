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
		float rounds = pd.stats.numRounds;
		float score = pd.player.score;
		float rounding = 0.2f;
		// EVERYTHING

		t.favWeapon = pd.weapons.OrderByDescending(element => element.stat.kills).First().name;
		//t.setData(pd.weapons[0].name.ToString(),pd.weapons[0].stat.kills,rounding);

		//-------------General perfomance stats in a ratio based on time
		//------------Used for apriori
		t.setData("timePlayed",pd.stats.timePlayed,rounding);
		t.setData("score",pd.player.score/time,rounding);
		t.setData("kills",pd.stats.kills/time,rounding);
		t.setData("deaths",pd.stats.deaths/time,rounding);
		t.setData("shotsFired",pd.stats.shotsFired/time,rounding);
		t.setData("shotsHit",pd.stats.shotsHit/time,rounding);
		t.setData("numLosses",pd.stats.numLosses/rounds,rounding);
		t.setData("numWins",pd.stats.numWins/rounds,rounding);
		t.setData("weaponKills",pd.stats.extra.weaponKills/kills,rounding);
		t.setData("vehicleKills",pd.stats.extra.vehicleKills/kills,rounding);  
		t.setData("medals",pd.stats.extra.medals/time,rounding);
		t.setData("headshots",pd.stats.headshots/kills,rounding);
		t.setData("suppressionAssists",pd.stats.suppressionAssists/time,rounding);
		t.setData("avengerKills",pd.stats.avengerKills/kills,rounding);
		t.setData("saviorKills",pd.stats.saviorKills/kills,rounding);
		t.setData("nemesisKills",Mathf.Clamp(pd.stats.nemesisKills/kills,0,0.1f),rounding);	//0.1
		t.setData("resupplies",Mathf.Clamp(pd.stats.resupplies/score,0,0.002f),rounding);		//0.002
		t.setData("repairs",Mathf.Clamp(pd.stats.repairs/score,0,0.0005f),rounding);			//0.0005
		t.setData("heals",Mathf.Clamp(pd.stats.heals/score,0,0.005f),rounding);				//0.005
		t.setData("revives",Mathf.Clamp(pd.stats.revives/score,0,0.0005f),rounding);			//0.0005
		t.setData("killAssists",pd.stats.killAssists/score,rounding);

		t.setData("kdr",Mathf.Clamp(pd.stats.extra.kdr,0f,5f),rounding); //clamp 5
		t.setData("wlr",Mathf.Clamp(pd.stats.extra.wlr,0f,5f),rounding); //clamp 5
		t.setData("spm",Mathf.Clamp(pd.stats.extra.spm,0f,2000f),rounding); //clamp 2000
		t.setData("gspm",pd.stats.extra.gspm,rounding);
		t.setData("kpm",pd.stats.extra.kpm,rounding);
		t.setData("sfpm",Mathf.Clamp(pd.stats.extra.sfpm,0f,100f),rounding); // clapm 100
		t.setData("hkp",Mathf.Clamp(pd.stats.extra.hkp,0f,30f),rounding);  //claimp 30
		t.setData("khp",Mathf.Clamp(pd.stats.extra.khp,0f,30),rounding);  //clamp 30
		t.setData("accuracy",Mathf.Clamp(pd.stats.extra.accuracy,0f,30f),rounding); //clamp 30


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
			/*
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
*/

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
		//DoPrint();
		//Normalize.instance.StartNormailzation(t.ToArray());

		SoundReference.instance.PlaySound(SoundReference.instance.Done);
	}

	void DoPrint(){
		int index = tupleList[0].getIndexOfProperti("revives");
		float max = -Mathf.Infinity;

		float[] numZero = new float[tupleList[0].data.Count];

		foreach(Tuple t in tupleList){
			for(int i=0;i<numZero.Length;i++){
				if(t.dataNormalized[i] == 0){
					numZero[i]++;
				}
			}
			//print(t.dataName[index]+"    "+t.data[index]+"    "+t.dataNormalized[index]);
			if(t.data[index] > max){
				max = t.data[index];
			}
		}

		for(int i=0;i<numZero.Length;i++){
			print(tupleList[0].dataName[i]+"   "+numZero[i]);
		}

		//print("MAX for this value is "+max);
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
