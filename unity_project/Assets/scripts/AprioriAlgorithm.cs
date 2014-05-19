using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

/**
 * performs the a-Priori algorithm on the transactions dataset with a given minimum supportThreshold.
 * all columns of transactions should have distinct values!!
 */
public class Apriori 
{
	public static List<ItemSet> lastRunSet;

	public static StreamWriter writer;

	public static IEnumerator apriori( Tuple[] transactions, int supportThreshold ) 
	{
		bf4stats.instance.aPrioriResults.Clear();

		if(File.Exists(Application.dataPath + "/Logs/log.txt"))
			File.Delete(Application.dataPath + "/Logs/log.txt");
		FileStream debugStream = File.Open(Application.dataPath + "/Logs/log.txt", FileMode.CreateNew);
		writer = new StreamWriter(debugStream);
		writer.WriteLine("starting aPriori at " + System.DateTime.Now);

		List<ItemSet> ret = new List<ItemSet>(); 
		int k;
		Dictionary<ItemSet, int> frequentItemSets = generateFrequentItemSetsLevel1( transactions, supportThreshold );
		Debug.Log("level 1 done");
		writer.WriteLine("level 1 done at " + System.DateTime.Now);
		SoundReference.instance.PlaySound(SoundReference.instance.BigProgress);
		yield return new WaitForEndOfFrame();
		
		// searches for frequent patterns of dim k, until no frequent patterns for current k found
		for (k = 1; frequentItemSets.Count > 0; k++) {
			Debug.Log("Finding frequent itemsets of length " + (k + 1) + "…");
			writer.WriteLine("Finding frequent itemsets of length " + (k + 1) + "… at " + System.DateTime.Now);

			frequentItemSets = generateFrequentItemSets( supportThreshold, transactions, frequentItemSets );
			foreach(ItemSet set in frequentItemSets.Keys)
			{
				ret.Add(set);
			}
			
			Debug.Log( " found " + frequentItemSets.Count );
			writer.WriteLine(" found " + frequentItemSets.Count);

			foreach(ItemSet i in frequentItemSets.Keys)
			{
				APrioriRes res = new APrioriRes();
				for(int j = 0; j<i.set.Length; j++)
				{
					int idx = (int)System.Math.Truncate(i.set[j]);
					i.set[j] = i.set[j]-idx;
					res.resColumn.Add(new APrioriRes.APrioriResItem(TupleFactory.instance.tupleList[0].dataName[idx], i.set[j]));
				}
				bf4stats.instance.aPrioriResults.Add(res);
				//string s = Normalize.instance.PrintUnfactorizedData(i.set);
				Debug.Log(res.ToString());
				writer.WriteLine(res.ToString());
			}

			SoundReference.instance.PlaySound(SoundReference.instance.BigProgress);
			yield return new WaitForEndOfFrame();
		}
		
		lastRunSet = ret;

		writer.Close();
		string toWrite = JsonConvert.SerializeObject(bf4stats.instance.aPrioriResults);
	    Directory.CreateDirectory(Application.dataPath+"/Data/LogAsJson/");
		File.WriteAllText(Application.dataPath+"/Data/LogAsJson/"+"aprioriResult"+".txt",toWrite);
	}
	
	/**
	 * looks for frequent patterns of dimension k (given by lowerLevelItemSets, min.2)
	 * @param lowerLevelItemSets the frequent itemsets from the last iteration (k-1)
	 * @return k-dimensional frequent itemsets
	 */
	private static Dictionary<ItemSet, int> generateFrequentItemSets( int supportThreshold, Tuple[] transactions,
	                                                                    Dictionary<ItemSet, int> lowerLevelItemSets ) 
	{
		
		Dictionary<ItemSet, int> ret = new Dictionary<ItemSet, int>();
		
		ItemSet joinedSet;
		int curSup;
		
		/*
         * try to join all sets with each other and check the support for all candidates, 
         * add only those that have enough support to the set
         */
		foreach(ItemSet setP in lowerLevelItemSets.Keys)
		{
			foreach(ItemSet setQ in lowerLevelItemSets.Keys)
			{
				if(setP.Equals(setQ))
					continue;
				
				if(setP.set.Length == 1)
					joinedSet = new ItemSet(new float[]{setP.set[0], setQ.set[0]});
				else
					joinedSet = joinSets(setP, setQ);
				
				if(joinedSet != null)
				{
					curSup = countSupport(joinedSet.set, transactions);
					if(curSup >= supportThreshold && !ret.ContainsKey(joinedSet))
						ret.Add(joinedSet, curSup);
				}
			}
		}
		
		return ret;
	}
	
	/**
     * try to join first and second. succeeds if first and second are equal until the 2nd-last position,
     * with the last being smaller at first. otherwise returns null.
     */
	private static ItemSet joinSets( ItemSet first, ItemSet second ) 
	{
		//writer.WriteLine("try to join sets " + first.ToString() + ", " + second.ToString());

		ItemSet ret = null;
		
		if(first.set.Length != second.set.Length)
			return null;
		
		// check if elements until index k-1 are the same
		for(int i=0; i<first.set.Length-1; i++)
		{
			if(first.set[i] != second.set[i])
				return null;
		}
		// check elements at index k
		if(first.set[first.set.Length-1] >= second.set[second.set.Length-1])
			return null;
		
		// join itemsets        
		ret = new ItemSet(new float[first.set.Length+1]);
		for(int i=0; i<first.set.Length; i++)
			ret.set[i] = first.set[i];
		ret.set[first.set.Length] = second.set[second.set.Length-1];        
		
		return ret;
	}
	
	/**
     * returns all frequent onedimensional itemsets of transactions.
     */
	private static Dictionary<ItemSet, int> generateFrequentItemSetsLevel1(Tuple[] transactions, int supportThreshold ) 
	{
		Dictionary<ItemSet, int> ret = new Dictionary<ItemSet, int>();
		
		List<float> allItems = new List<float>();
		
		// get all 1-dim items
		foreach(Tuple items in transactions)
		{
			foreach(float item in items.dataNormalized)
			{
				if(allItems.Contains(item) == false)
					allItems.Add(item);
			}
		}
		
		// generate itemsets
		foreach(float item in allItems)
		{
			float[] onedim = new float[]{ item };
			int curSup = countSupport(onedim, transactions);
			if(curSup >= supportThreshold)
				ret.Add(new ItemSet(onedim), curSup);
		}        
		
		return ret;
	}
	
	/**
     * counts the support of itemSet in all rows of transactions
     */
	private static int countSupport( float[] itemSet, Tuple[] transactions ) {
		// Assumes that items in ItemSets and transactions are both unique
		int supportCount = 0;
		
		/**
    	 * for every row (student), checking if it made the choices of the itemset, 
    	 * and then increasing the support count.
    	 */
		foreach(Tuple row in transactions)
		{
			if(CheckChoiceInRow(itemSet, row))
				supportCount++;
		}
		
		return supportCount;
	}
	
	/**
     * checks whether row contains itemSet
     */
	private static bool CheckChoiceInRow(float[] itemSet, Tuple row)
	{
		// check if whole choice found in row
		foreach(float item in itemSet)
		{
			// check if single item found in row
			bool foundItem = false;
			foreach(float choice in row.dataNormalized)
			{
				if(item == choice)
				{
					foundItem = true;
					break;
				}
			}
			if(!foundItem)
				return false;
		}
		return true;
	}
	
}
