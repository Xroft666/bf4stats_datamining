using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class WeaponSuggestionSystem 
{
	static private Dictionary<int, string> weaponsClusterMap = null;
	static private List<Tuple> cleanTuples = null;

	static public void LoadTrainingResult(string clusterWeaponMapFileName, string cleanTuplesName)
	{
		string[] lines = File.ReadAllLines( Application.dataPath + "/Data/ClusteringResult/" + clusterWeaponMapFileName + ".txt");

		if( lines == null || lines.Length == 0 )
		{
			Debug.Log("Data is corrupted. And bribed.");
			return;
		}

		weaponsClusterMap = new Dictionary<int, string>();

		for( int i = 1; i < lines.Length; i++ )
		{
			string[] keyValue = lines[i].Split('\t');
			weaponsClusterMap[ int.Parse(keyValue[0]) ] = keyValue[1];
		}

		string tuples = File.ReadAllText(Application.dataPath + "/Data/Tuples/"+cleanTuplesName+".txt");
		cleanTuples = JsonConvert.DeserializeObject<List<Tuple>>(tuples);	
	}

	static public string SuggestWeaponTo( Tuple newPlayer, int K )
	{
		if( weaponsClusterMap == null || cleanTuples == null)
		{
			Debug.Log("Data has not been loaded. Call LoadTrainingData first");
			return "Undefined";
		}

		// k-nn

		Tuple[] closestNeibhours = cleanTuples.OrderBy( t =>
		{
			float timePlayedSqrd = t.data[t.getIndexOfProperti("timePlayed")] - newPlayer.data[newPlayer.getIndexOfProperti("timePlayed")];
			float scoreSqrd = t.data[t.getIndexOfProperti("score")] - newPlayer.data[newPlayer.getIndexOfProperti("score")];
			float killsSqrd = t.data[t.getIndexOfProperti("kills")] - newPlayer.data[newPlayer.getIndexOfProperti("kills")];
			float deathsSqrd = t.data[t.getIndexOfProperti("deaths")] - newPlayer.data[newPlayer.getIndexOfProperti("deaths")];
			float shotsFiredSqrd = t.data[t.getIndexOfProperti("shotsFired")] - newPlayer.data[newPlayer.getIndexOfProperti("shotsFired")];
			float shotsHitSqrd = t.data[t.getIndexOfProperti("shotsHit")] - newPlayer.data[newPlayer.getIndexOfProperti("shotsHit")];
			float numLossesSqrd = t.data[t.getIndexOfProperti("numLosses")] - newPlayer.data[newPlayer.getIndexOfProperti("numLosses")];
			float numWinsSqrd = t.data[t.getIndexOfProperti("numWins")] - newPlayer.data[newPlayer.getIndexOfProperti("numWins")];
			float weaponKillsSqrd = t.data[t.getIndexOfProperti("weaponKills")] - newPlayer.data[newPlayer.getIndexOfProperti("weaponKills")];
			float vehicleKillsSqrd = t.data[t.getIndexOfProperti("vehicleKills")] - newPlayer.data[newPlayer.getIndexOfProperti("vehicleKills")];
			float medalsSqrd = t.data[t.getIndexOfProperti("medals")] - newPlayer.data[newPlayer.getIndexOfProperti("medals")];
			float headshotsSqrd = t.data[t.getIndexOfProperti("headshots")] - newPlayer.data[newPlayer.getIndexOfProperti("headshots")];
			float suppressionAssistsSqrd = t.data[t.getIndexOfProperti("suppressionAssists")] - newPlayer.data[newPlayer.getIndexOfProperti("suppressionAssists")];
			float avengerKillsSqrd = t.data[t.getIndexOfProperti("avengerKills")] - newPlayer.data[newPlayer.getIndexOfProperti("avengerKills")];
			float saviorKillsSqrd = t.data[t.getIndexOfProperti("saviorKills")] - newPlayer.data[newPlayer.getIndexOfProperti("saviorKills")];
			float nemesisKillsSqrd = t.data[t.getIndexOfProperti("nemesisKills")] - newPlayer.data[newPlayer.getIndexOfProperti("nemesisKills")];
			float resuppliesSqrd = t.data[t.getIndexOfProperti("resupplies")] - newPlayer.data[newPlayer.getIndexOfProperti("resupplies")];
			float repairsSqrd = t.data[t.getIndexOfProperti("repairs")] - newPlayer.data[newPlayer.getIndexOfProperti("repairs")];
			float healsSqrd = t.data[t.getIndexOfProperti("heals")] - newPlayer.data[newPlayer.getIndexOfProperti("heals")];
			float revivesSqrd = t.data[t.getIndexOfProperti("revives")] - newPlayer.data[newPlayer.getIndexOfProperti("revives")];
			float killAssistsSqrd = t.data[t.getIndexOfProperti("killAssists")] - newPlayer.data[newPlayer.getIndexOfProperti("killAssists")];
			float kdrSqrd = t.data[t.getIndexOfProperti("kdr")] - newPlayer.data[newPlayer.getIndexOfProperti("kdr")];
			float wlrSqrd = t.data[t.getIndexOfProperti("wlr")] - newPlayer.data[newPlayer.getIndexOfProperti("wlr")];
			float spmSqrd = t.data[t.getIndexOfProperti("spm")] - newPlayer.data[newPlayer.getIndexOfProperti("spm")];
			float gspmSqrd = t.data[t.getIndexOfProperti("gspm")] - newPlayer.data[newPlayer.getIndexOfProperti("gspm")];
			float kpmSqrd = t.data[t.getIndexOfProperti("kpm")] - newPlayer.data[newPlayer.getIndexOfProperti("kpm")];
			float sfpmSqrd = t.data[t.getIndexOfProperti("sfpm")] - newPlayer.data[newPlayer.getIndexOfProperti("sfpm")];
			float hkpSqrd = t.data[t.getIndexOfProperti("hkp")] - newPlayer.data[newPlayer.getIndexOfProperti("hkp")];
			float khpSqrd = t.data[t.getIndexOfProperti("khp")] - newPlayer.data[newPlayer.getIndexOfProperti("khp")];
			float accuracySqrd = t.data[t.getIndexOfProperti("accuracy")] - newPlayer.data[newPlayer.getIndexOfProperti("accuracy")];


			return Mathf.Sqrt(
				timePlayedSqrd * timePlayedSqrd +
				scoreSqrd * scoreSqrd +
				killsSqrd * killsSqrd +
				deathsSqrd * deathsSqrd +
				shotsFiredSqrd * shotsFiredSqrd +
				shotsHitSqrd * shotsHitSqrd +
				numLossesSqrd * numLossesSqrd +
				numWinsSqrd * numWinsSqrd +
				weaponKillsSqrd * weaponKillsSqrd +
				vehicleKillsSqrd * vehicleKillsSqrd +
				medalsSqrd * medalsSqrd +
				headshotsSqrd * headshotsSqrd +
				suppressionAssistsSqrd * suppressionAssistsSqrd +
				avengerKillsSqrd * avengerKillsSqrd +
				saviorKillsSqrd * saviorKillsSqrd +
				nemesisKillsSqrd * nemesisKillsSqrd +
				resuppliesSqrd * resuppliesSqrd +
				repairsSqrd * repairsSqrd +
				healsSqrd * healsSqrd +
				revivesSqrd * revivesSqrd +
				killAssistsSqrd * killAssistsSqrd +
				kdrSqrd * kdrSqrd +
				wlrSqrd * wlrSqrd +
				spmSqrd * spmSqrd +
				gspmSqrd * gspmSqrd +
				kpmSqrd * kpmSqrd +
				sfpmSqrd * sfpmSqrd +
				hkpSqrd * hkpSqrd +
				khpSqrd * khpSqrd +
				accuracySqrd * accuracySqrd
				);
		}).Take( K ).ToArray();


		Dictionary<string, int> weaponsCounter = new Dictionary<string, int>();
		for( int i = 0; i < closestNeibhours.Length; i++ )
		{
			if( !weaponsCounter.ContainsKey(closestNeibhours[i].favWeapon) )
				weaponsCounter[closestNeibhours[i].favWeapon] = 0;
			
			weaponsCounter[closestNeibhours[i].favWeapon]++;
		}

		if( weaponsCounter.Count > 0 )
		{
			return weaponsCounter.OrderBy( t => t.Value ).First().Key;
		}
		else
		{
			Debug.Log("Something went wrong");
			return "Undefined";
		}
	}
}
