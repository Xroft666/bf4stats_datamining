using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class CreateLearningData : MonoBehaviour {

	//public TextAsset dataFile;
	public string outputFileName;


	public static CreateLearningData instance;
	void Awake(){
		instance = this;
		Application.targetFrameRate = 0;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
			startOfFile +="\t"+(i+1).ToString();
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
	
}
