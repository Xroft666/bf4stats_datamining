using System;
using System.Collections;
using System.Collections.Generic;

public class MathHelper 
{

	public static float Mean(List<float> values)
	{
		return values.Count == 0 ? 0 : Mean(values, 0, values.Count);
	}
	
	public static float Mean(List<float> values, int start, int end)
	{
		float s = 0;
		
		for (int i = start; i < end; i++)
		{
			s += values[i];
		}
		
		return s / (end - start);
	}

	public static float Variance(List<float> values)
	{
		return Variance(values, Mean(values), 0, values.Count);
	}
	
	public static float Variance(List<float> values, float mean)
	{
		return Variance(values, mean, 0, values.Count);
	}
	
	public static float Variance(List<float> values, float mean, int start, int end)
	{
		float variance = 0;
		
		for (int i = start; i < end; i++)
		{
			variance += (float)Math.Pow((values[i] - mean), 2);
		}
		
		int n = end - start;
		if (start > 0) n -= 1;
		
		return variance / (n);
	}
	
	public static float StandardDeviation(List<float> values)
	{
		return values.Count == 0 ? 0 : StandardDeviation(values, 0, values.Count);
	}
	
	public static float StandardDeviation(List<float> values, int start, int end)
	{
		float mean = Mean(values, start, end);
		float variance = Variance(values, mean, start, end);
		
		return (float)Math.Sqrt(variance);
	}
}
