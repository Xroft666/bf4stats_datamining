using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
		
		if(TupleFactory.instance.tupleList.Count > 0){
			if(GUI.Button(new Rect(500,100,250,50),"Create learn data from tuples")){
				StartCoroutine(CreateLearningData.instance.LearnDataFromTuples(TupleFactory.instance.tupleList));
			}
		}
	}
}
