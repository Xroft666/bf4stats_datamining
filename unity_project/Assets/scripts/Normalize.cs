using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Normalize : MonoBehaviour {

	public float roundToNearest = 0;
	//user specified max and min value for normalization
	public float newMin,newMax;

	//int = column ID, float holds min and max
	//Dictionary<int,float[]> normalizeData = new Dictionary<int, float[]>();

	public static Normalize instance;
	void Awake(){
		instance = this;
	}

	public float[] min;
	public float[] max;


	public void NormalizeTuples(Tuple[] t){

		max = new float[t[0].data.Count]; 
		min = new float[t[0].data.Count];
		for(int i=0;i<max.Length;i++){
			max[i] = -Mathf.Infinity;
			min[i] = Mathf.Infinity;
		}

		for(int i=0;i<t.Length;i++){
			for(int j=0;j<max.Length;j++){
				if(t[i].data[j]<min[j]){
					min[j] = t[i].data[j];
				}else if(t[i].data[j]>max[j]){
					max[j] = t[i].data[j];
				}
			}
		}

		for(int i=0; i<t.Length; i++)
			t[i].dataNormalized.Clear();
		
		// z-score normalization
		/*List<float> dataColumn = new List<float>();
		for(int x=0; x<t[0].data.Count; x++)
		{
			// convert to columns
			dataColumn.Clear();
			for(int y=0; y<t.Length; y++)
			{
				dataColumn.Add(t[y].data[x]);
			}
			// normalize
			dataColumn = ZScoreNormalize(dataColumn);

			// reconvert to rows
			for(int y=0; y<t.Length; y++)
			{
				t[y].dataNormalized.Add(dataColumn[y]);
			}
		}*/

		// min-max normalization
		foreach(Tuple tu in t){
			MinMaxNormalize(tu);
		}

		print("Done normalizing");
	}

	private void MinMaxNormalize(Tuple t){
		//t.dataNormalized = new List<float>();
		List<float> newDataNormalized = new List<float>();
		for(int i=0;i<t.data.Count;i++){
			float value = (((t.data[i]-min[i])/(max[i]-min[i]))*(newMax-newMin)+newMin);
			value = Mathf.Clamp(value,newMin,newMax);
			value = Mathf.Floor(value * (1/t.roundValue[i])) / (1/t.roundValue[i]);
			newDataNormalized.Add(value);
		}
		t.dataNormalized = newDataNormalized;
	}

	private List<float> ZScoreNormalize(List<float> t)
	{
		List<float> ret = t;

		float mean = MathHelper.Mean(ret);
		float standardDeviation = MathHelper.StandardDeviation(ret);

		for(int i=0; i<ret.Count; i++)
		{
			ret[i] = (ret[i] - mean) / standardDeviation;
		}

		return ret;
	}

	public void FactorizeTuples(Tuple[] t)
	{
		for(int i=1; i<t.Length; i++)
		{
			FactorizeTuple(t[i]);
		}
	}

	private void FactorizeTuple(Tuple t)
	{
		for(int i=1; i<t.dataNormalized.Count; i++)
		{
			t.dataNormalized[i] += i;
		}
	}

	public void UnFactorizeTuples(Tuple[] t)
	{
		for(int i=1; i<t.Length; i++)
		{
			UnFactorizeTuple(t[i]);
		}
	}

	public void UnFactorizeTuple(Tuple t)
	{
		for(int i=1; i<t.dataNormalized.Count; i++)
		{
			t.dataNormalized[i] -= i;
		}
	}

	public string PrintUnfactorizedData(float[] data)
	{
		StringBuilder sb = new StringBuilder();

		foreach(float f in data)
		{
			int i = (int)System.Math.Truncate(f);
			sb.Append(TupleFactory.instance.tupleList[0].dataName[i]).Append(": ").Append(f-i).Append(" | ");
		}

		return sb.ToString();
	}

	/*
	public string[] denormalizeColumn(string[] values, int id){
		string[] toReturn = values;

		float[] value = normalizeData[id];

		//float result = ((outVal-min)/(max-min))*(newMax);

		//use normalizeData to denormalize

		return toReturn;
	}

	//normalizes the column
	public string[] normalizeColumn(string[] values, int id){

		string[] toReturn = values;

		float min = Mathf.Infinity;
		float max = -Mathf.Infinity;


		//finds the biggest and smallest value for the column
		foreach(string s in values){
			float outVal = 0;
			if(float.TryParse(s,out outVal)){
				if(outVal < min){
					min = outVal;
				}
				if(outVal > max){ 
					max = outVal;
				}
			}
		}

		normalizeData.Add(id,new float[]{min,max});

		//applies the normalization formula
		for(int i=0;i<toReturn.Length;i++){
			float outVal = 0;
			if(float.TryParse(toReturn[i],out outVal)){
				float result = (((outVal-min)/(max-min))*(newMax-newMin)+newMin);
				result = Mathf.Round(result * 100.0f) / 100.0f;
				toReturn[i] = result.ToString();
			}
		}

		return toReturn;
	}*/
}
