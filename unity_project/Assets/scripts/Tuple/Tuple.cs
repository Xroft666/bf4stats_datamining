using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tuple{
	public List<string> data = new List<string>();
	public List<string> dataName = new List<string>();
	 List<float> dataNormalized = new List<float>();

	string findName;

	public string getDataFromString(string name){
		findName = name;
		return data[dataName.FindIndex(isName)];
	}
	public float getNormalizedDataFromString(string name){
		findName = name;
		return dataNormalized[dataName.FindIndex(isName)];
	}

	private bool isName(string name)	
	{
		return (name==findName);
	}

	public void setData(string name, string data){
		this.data.Add(data);
		this.dataName.Add(name);
	}
}
