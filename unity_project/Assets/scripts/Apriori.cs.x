using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Apriori : MonoBehaviour {

	public int supportThreshold;

	List<string[]> data;

	//Dictionary<string[],int> frequentPatterns = new Dictionary<string[], int>();

	List<float[]> rows =  new List<float[]>();
	
	List<Tuple> tuples;

	public static Apriori instance;
	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Init(List<Tuple> tuples){
		this.tuples = tuples;
		//rows.Clear();
		//converts data from string to float
		/*for(int i=0;i<tuples.Count;i++){
			Tuple textData = tuples[i];
			float[] floatData = new float[textData.Length];
			for(int j = 0;j<textData.Length;j++){
				//add values to new array and makes values uniqe based on columns
				floatData[j] = float.Parse(textData[j])*((i+1)*10);
			}

			rows.Add(floatData);
			//Visualizer.instance.VisualizeF(floatData);
		}
		print("converted data");
		*/
		main();
	}

	float[] decodeSet(float[] itemSet){
		float[] toReturn = new float[itemSet.Length];

		for(int i=0;i<itemSet.Length;i++){
			toReturn[i] = (itemSet[i]%10)/10;
			toReturn[i] = Mathf.Round(toReturn[i] * 100.0f) / 100.0f;
		}

		return toReturn;
	}

	void main(){
		List<float[]> frequentSets = new List<float[]>();
		int k;

		Dictionary<float[],int> freqPat = generateFrequentItemSetsLevel1();

		//keeps creating item sets with more dimentions untill there are no more frequent patterns
		for(k=1; freqPat.Count > 0; k++){
			print("dimention "+(k-1)+" have "+freqPat.Count+" frequent patterns");
			print("finding patterns in dimention "+k);
			freqPat = generateFrequentItemSets(freqPat);
			foreach(KeyValuePair<float[],int> itemSet in freqPat){
				frequentSets.Add(itemSet.Key);
			}
		}

		//call for decoding of every item set
		List<float[]> frequentSetsDecoded = new List<float[]>();
		foreach(float[] f in frequentSets){
			frequentSetsDecoded.Add(decodeSet(f));
		}
	}

	Dictionary<float[], int> generateFrequentItemSets(Dictionary<float[], int> lowerDimentionPatterns){
		//print ("trying to joind sets");

		Dictionary<float[], int> toReturn = new Dictionary<float[], int>();

		foreach(KeyValuePair<float[],int> s1 in lowerDimentionPatterns){
			foreach(KeyValuePair<float[],int> s2 in lowerDimentionPatterns){
				float[] joinedSet = joinSets(s1.Key,s2.Key);
				if(joinedSet != null){
					int sup = countSupport(joinedSet);
					if(sup >= supportThreshold){
						toReturn.Add(joinedSet, sup);
					}
				}
			}
		}

		return toReturn;
	}

	Dictionary<float[], int> generateFrequentItemSetsLevel1(){
		Dictionary<float[], int> toReturn = new Dictionary<float[], int>();

		List<float> allItems = new List<float>();

		//get all items for use as first dimention
		foreach(float[] row in rows){
			foreach( float item in row){
				if(!allItems.Contains(item)){
					allItems.Add(item);
				}

			}
		}

		//created dataset of every cell and calls for counting of the support of them
		foreach(float item in allItems){
			float[] theItem = new float[1];
			theItem[0] = item;

			int sup = countSupport(theItem);
			if(sup > supportThreshold){
				toReturn.Add(theItem,sup);
			}
		}

		return toReturn;
	}


	//counts the support of the provided itemset
	int countSupport(float[] itemSet){
		int supportCounter = 0;

		//checks if every float in the item set is found in the current row
		foreach(float[] row in rows){
			bool foundSet = true;
			foreach(float item in itemSet){
				bool foundItem = false;
				foreach(float rowItem in row){
					if(item == rowItem){
						foundItem = true;
						break;
					}
				}
				if(!foundItem){
					foundSet = false;
				}
			}
			if(foundSet){
				supportCounter++;
			}
		}

		return supportCounter;
	}

	float[] joinSets(float[] s1, float[] s2){
		float[] toReturn;

		//checks if the sets allready are similar
		if(s1 == s2 || s1.Length != s2.Length){
			return null;
		}

		//checks if the two sets are similar untill the last element
		for(int i=0;i<s1.Length-1;i++){
			if(s1[i] != s2[i]){
				return null;
			}
		}

		//checks that the last element of the first set is bigger than the last element of the second set
		if(s1[s1.Length-1] > s2[s2.Length-1]){
			return null;
		}

		//sets all but the last element of to return to be like the first set, and the last value to be like the second set
		toReturn = new float[s1.Length+1];
		for(int i=0;i<s1.Length;i++){
			toReturn[i] = s1[i];
		}
		toReturn[s1.Length] = s2[s2.Length-1];

		return toReturn;
	}



	void OnGUI(){

			//value = GUI.TextField(new Rect(10,110,140,50), value);
			if(GUI.Button(new Rect(150,110,150,50), "Apriori")){
				Init();
			}

	}

//		/***
//	 * The TRANSACTIONS 2-dimensional array holds the full data set for the lab
//	 */
//
//	
//	void main( string[] args ) {
//		// TODO: Select a reasonable support threshold via trial-and-error. Can either be percentage or absolute value.
//		int supportThreshold = 42;
//		apriori( TRANSACTIONS, supportThreshold );
//	}
//	
//	List<ItemSet> apriori( int[][] transactions, int supportThreshold ) {
//		int k;
//		Dictionary<ItemSet, Integer> frequentItemSets = generateFrequentItemSetsLevel1( transactions, supportThreshold );
//		for (k = 1; frequentItemSets.size() > 0; k++) {
//			print( "Finding frequent itemsets of length " + (k + 1) + "…" );
//			frequentItemSets = generateFrequentItemSets( supportThreshold, transactions, frequentItemSets );
//			// TODO: add to list
//			
//			println( " found " + frequentItemSets.size() );
//		}
//		// TODO: create association rules from the frequent itemsets
//		
//		// TODO: return something useful
//		return null;
//	}
//	
//	Dictionary<ItemSet, Integer> generateFrequentItemSets( int supportThreshold, int[][] transactions,
//	                                                      Dictionary<ItemSet, Integer> lowerLevelItemSets ) {
//		// TODO: first generate candidate itemsets from the lower level itemsets
//		
//		/*
// * TODO: now check the support for all candidates and add only those
// * that have enough support to the set
// */
//		
//		// TODO: return something useful
//		return null;
//	}
//	
//	ItemSet joinSets( ItemSet first, ItemSet second ) {
//		// TODO: return something useful
//		return null;
//	}
//	
//	Dictionary<ItemSet, Integer> generateFrequentItemSetsLevel1( int[][] transactions, int supportThreshold ) {
//		// TODO: return something useful
//		return null;
//	}
//	
//	int countSupport( int[] itemSet, int[][] transactions ) {
//		// Assumes that items in ItemSets and transactions are both unique
//		
//		// TODO: return something useful
//		return 0;
//	}
		


}
