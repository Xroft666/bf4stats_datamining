using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class CreateLearningData : MonoBehaviour {

	//public TextAsset dataFile;
	public string outputFileName;

	public static CreateLearningData instance;
	void Awake(){
		instance = this;
		Application.targetFrameRate = 0;
	}

	public IEnumerator LearnDataFromTuples(List<Tuple> tupleList){
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
		yield return new WaitForEndOfFrame();
		print("Done creating learning data");
		SoundReference.instance.PlaySound(SoundReference.instance.Done);

	}

	static public void ReadClusterFile(string tuplesFileName, string esomFileName)
	{
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
					tuples[j].playerCluster = playerCluster;
		}

		// sorting by claster
		tuples = tuples.OrderBy (element => element.playerCluster);

		// searching for the average weapon in the cluster
		Dictionary<int, string> weaponsClusterMap = new Dictionary<int, string> ();

		int clusterIndex = tuples[0].playerCluster;
		for( int i = 0; i < tuples.Count; i++ )
		{
			Dictionary<string, int> weaponsCounter = new Dictionary<string, int>();

			bool hasChanged = clusterIndex != tuples[i].playerCluster;
			clusterIndex = tuples[i].playerCluster;

			if( !weaponsCounter.ContainsKey(tuples[i].favWeapon) )
				weaponsCounter[tuples[i].favWeapon] = 0;

			weaponsCounter[tuples[i].favWeapon]++;

			if( hasChanged )
			{
				weaponsClusterMap[clusterIndex] = weaponsCounter.OrderByDescending( x => x.Value ).First().Key;
				weaponsCounter.Clear();
			}
		}

		// At this point weaponsClusterMap will have the most used guns of each clusters
	}
}
