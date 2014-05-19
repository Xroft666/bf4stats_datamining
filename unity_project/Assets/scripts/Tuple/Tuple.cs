using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tuple{
	public int playerID;
	public List<float> data = new List<float>();
	public List<string> dataName = new List<string>();
	public List<float> dataNormalized = new List<float>();
	public List<float> roundValue = new List<float>();
	
	public float getDataFromString(string name){
		return data[dataName.FindIndex(delegate(string obj) {
			return obj == name;
		})];
	}
	public float getNormalizedDataFromString(string name){
		return dataNormalized[dataName.FindIndex(delegate(string obj) {
			return obj == name;
		})];
	}

	public int getIndexOfProperti(string name){
		return dataName.FindIndex(delegate(string obj) {
			return obj == name;
		});
	}

	public void setData(string name, float data, float roundValue){
		this.data.Add(data);
		this.dataName.Add(name);
		this.roundValue.Add(roundValue);
	}
}
