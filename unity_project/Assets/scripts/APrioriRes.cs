using System.Text;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class APrioriRes 
{

	public List<APrioriResItem> resColumn;

	[System.Serializable]
	public class APrioriResItem
	{
		public string descriptor;
		public float value;

		public APrioriResItem(string desc, float v)
		{
			descriptor = desc;
			value = v;
		}
	}

	public APrioriRes()
	{
		resColumn = new List<APrioriResItem>();
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		foreach(APrioriResItem item in resColumn)
			sb.Append(" | ").Append(item.descriptor).Append(": ").Append(item.value);
		sb.Append(" |");
		return sb.ToString();
	}
}
