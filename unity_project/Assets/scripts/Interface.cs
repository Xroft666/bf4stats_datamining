using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class Interface : MonoBehaviour {

	string userName = "";

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,250,50),"Start Download")){
			StartCoroutine(bf4stats.instance.StartDownload());
		}

		if(GUI.Button(new Rect(0,50,250,50), "Create Tuples From Database")){
			StartCoroutine(TupleFactory.instance.CreateTuples());
		}
		if(GUI.Button(new Rect(250,100,250,50), "Load Tuples From File "+TupleFactory.instance.tupleFile.name)){
			TupleFactory.instance.ReadTupleFile();
		}
		
		if(GUI.Button(new Rect(0,100,250,50), "Normalize Tuple from File "+TupleFactory.instance.tupleFile.name)){
			TupleFactory.instance.NormalizeTupleFile();
		}

		if(GUI.Button(new Rect(250, 0, 250, 50), "Run aPriori from File "+TupleFactory.instance.tupleFile.name))
		{
			TupleFactory.instance.RunAPrioriFromFile();
		}
		
		if(TupleFactory.instance.tupleList.Count > 0){
			if(GUI.Button(new Rect(500,100,250,50),"Create learn data from tuples")){
				ESOMReaderWriter.LearnDataFromTuples(TupleFactory.instance.tupleList, "BestPlayers");
			}
		}

		if(GUI.Button(new Rect(750, 0, 250, 50), "Read cluster file and craft results"))
		{
			ESOMReaderWriter.ReadClusterFile("BestPlayerTuples", "6999 class");
        }

		userName = GUI.TextField(new Rect(750, 75, 250, 25), userName);

		if( !string.IsNullOrEmpty(userName) )
		if(GUI.Button(new Rect(750, 100, 250, 50), "Suggest weapon to this player"))
		{
			StartCoroutine(AnalyzePlayer(userName));
        }
    }

	IEnumerator AnalyzePlayer(string name)
	{
		Debug.Log ("Suggestion is being processed...");

		WeaponSuggestionSystem.LoadTrainingResult("6999 class_weaponSuggestion","BestPlayerTuples_cleaned");
		bf4stats downloader = FindObjectOfType(typeof(bf4stats)) as bf4stats;
		downloader.output = "";

		yield return downloader.StartCoroutine(downloader.RetrivePlayerInfo(userName));
		Debug.Log("Downloading " + name + " completed");

		string result = downloader.output;
		downloader.output = "";


		JsonSerializerSettings settings = new JsonSerializerSettings();
		settings.NullValueHandling = NullValueHandling.Ignore;
		PlayerData pd = JsonConvert.DeserializeObject<PlayerData>(result.Substring(7, result.Length - 8), settings);
		
		if( pd != null )
		{
			Tuple tuple = new Tuple();
			
			TupleFactory tupFactory = FindObjectOfType(typeof(TupleFactory)) as TupleFactory;
			tupFactory.FillTuple( tuple, pd );
			
			string suggestedWeapon = WeaponSuggestionSystem.SuggestClusterTo(tuple, 5);
			
			print ("Suggested weapon for " + userName + " is " + suggestedWeapon);
		}
		else
		{
			Debug.Log("Could not retrive info on the player.");
		}
	}
}
