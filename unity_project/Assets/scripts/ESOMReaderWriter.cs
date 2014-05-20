using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class ESOMReaderWriter {

	static public void LearnDataFromTuples(List<Tuple> tupleList, string outputFileName){
		if(File.Exists(Application.dataPath + "/Data/LearningData/"+outputFileName+".lrn")){
			File.Delete(Application.dataPath + "/Data/LearningData/"+outputFileName+".lrn");
		}

		string startOfFile = "";
		startOfFile +="%" + tupleList.Count + "\n";
		startOfFile +="%"+(tupleList[0].data.Count+1)+"\n";
		startOfFile +="%9";
		for(int i=0;i<tupleList[0].data.Count;i++){
			startOfFile +="\t1";
		}
		startOfFile += "\n";

		startOfFile +="%Key";
		for(int i=0;i<tupleList[0].data.Count;i++){
			startOfFile +="\t"+(tupleList[0].dataName[i]).ToString();
		}
		startOfFile += "\n";
		//startOfFile +="%Key\t1\t2\t3\t4\n";
		File.AppendAllText(Application.dataPath + "/Data/LearningData/"+outputFileName+".lrn", startOfFile);

		for(int i=0;i<tupleList.Count;i++){
			string line = "";
			line += tupleList[i].playerID.ToString();
			for(int j=0;j<tupleList[i].data.Count;j++){
				line += "\t" + tupleList[i].dataNormalized[j].ToString(); 
			}
			line += "\n";
			File.AppendAllText(Application.dataPath + "/Data/LearningData/"+outputFileName+".lrn", line);
			//print("progress: "+i+"/"+tupleList.Count);
		}

		Debug.Log("Done creating learning data");
		SoundReference.instance.PlaySound(SoundReference.instance.Done);

	}

	static public void ReadClusterFile(string tuplesFileName, string esomFileName)
	{
		Debug.Log("started reading cluster file " + esomFileName);

		string filePath = Application.dataPath + "/Data/ESOMResult/" + esomFileName + ".cls";
		string tuplesPath = Application.dataPath + "/Data/Tuples/" + tuplesFileName + ".txt";

		string[] fileLines = File.ReadAllLines (filePath);
		string jsonTuples = File.ReadAllText (tuplesPath);

		List<Tuple> tuples = JsonConvert.DeserializeObject<List<Tuple>>(jsonTuples);

		// going to the first player id
		for (int i = 0; i < fileLines.Length; i++) 
		{
			if (fileLines [i] [0] == '%')
				continue;

			string[] lineElements = fileLines [i].Split('\t');
			int playerId = int.Parse(lineElements[0]);
			int playerCluster = int.Parse(lineElements[1]);

			for( int j = 0; j < tuples.Count; j++ )
				if( tuples[j].playerID == playerId )
					tuples[j].clusterID = playerCluster;
		}

		Debug.Log("Cluster numbers are now inserted into tuples " + tuplesFileName);

		// ignoring 0 cluster
		tuples = (from x in tuples where x.clusterID != 0 select x).ToList();

		// ignoring "Undefined" weapon
		tuples = (from x in tuples where x.favWeapon != "Undefined" select x).ToList();

		// sorting by claster
		tuples = tuples.OrderBy (element => element.clusterID).ToList();

		Debug.Log("Cluster file " + tuplesFileName + " is being cleaned and sorted");

		// serialization of new cleaned tuples into another file
		string cleanedTuplesJson = JsonConvert.SerializeObject(tuples);
		File.AppendAllText(Application.dataPath + "/Data/Tuples/" + tuplesFileName + "_cleaned.txt", cleanedTuplesJson);

		Debug.Log("New tuples file " + tuplesFileName + "_cleaned.txt has been created");

		// searching for the average weapon in the cluster
		Dictionary<int, string> weaponsClusterMap = new Dictionary<int, string> ();
		Dictionary<string, int> weaponsCounter = new Dictionary<string, int>();

		Debug.Log("Searching for the best weapon...");

		int clusterIndex = 0;
		for( int i = 0; i < tuples.Count; i++ )
		{
			if( clusterIndex != tuples[i].clusterID )
			{
				if( weaponsCounter.Count > 0 )
				{
					weaponsClusterMap[clusterIndex] = weaponsCounter.OrderByDescending( x => x.Value ).First().Key;		
					weaponsCounter.Clear();
				}
				clusterIndex = tuples[i].clusterID;
			}

			if( !weaponsCounter.ContainsKey(tuples[i].favWeapon) )
				weaponsCounter[tuples[i].favWeapon] = 0;

			weaponsCounter[tuples[i].favWeapon]++;
		}

		// taking into consideration the last cluster
		if( weaponsCounter.Count > 0 )
		{
			weaponsClusterMap[clusterIndex] = weaponsCounter.OrderByDescending( x => x.Value ).First().Key;
			weaponsCounter.Clear();
		}

		Debug.Log("Searching is completed. Writing...");

		// At this point weaponsClusterMap will have the most used guns of each clusters
		// storing the result into a file

		File.AppendAllText(Application.dataPath + "/Data/ClusteringResult/"+esomFileName+"_weaponSuggestion.txt", "Cluster\tWeapon\n");
		foreach( KeyValuePair<int, string> clusterWeapon in weaponsClusterMap )
		{
			string line = clusterWeapon.Key + "\t" + clusterWeapon.Value + "\n";
			File.AppendAllText(Application.dataPath + "/Data/ClusteringResult/"+esomFileName+"_weaponSuggestion.txt", line);
		}

		Debug.Log("Clustering result has been written into " + esomFileName+"_weaponSuggestion.txt");
	}
}
