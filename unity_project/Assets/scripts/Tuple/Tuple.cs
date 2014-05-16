using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tuple{
	public List<string> data = new List<string>();
	public List<string> dataName = new List<string>();
	public List<float> dataNormalized = new List<float>();
	
	public string getDataFromString(string name){
		return data[dataName.FindIndex(delegate(string obj) {
			return obj == name;
		})];
	}
	public float getNormalizedDataFromString(string name){
		return dataNormalized[dataName.FindIndex(delegate(string obj) {
			return obj == name;
		})];
	}

	public void setData(string name, string data){
		this.data.Add(data);
		this.dataName.Add(name);
	}
}
