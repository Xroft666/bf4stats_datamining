using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/***
 * The ItemSet class is used to store information concerning a single transaction.
 * Should not need any changes.
 *
 */
public class ItemSet {
	
	/***
	 * The PRIMES array is internally in the ItemSet-class' hashCode method
	 */
	//private static final int[] PRIMES = { 2, 3, 5, 7, 11, 13, 17, 23, 27, 31, 37 };
	public float[] set;
	
	/***
     * Creates a new instance of the ItemSet class.
     * @param set Transaction content
     */
	public ItemSet( float[] set ) {
		this.set = set;
	}
	
	/**
     * hashCode functioned used internally in Hashtable
     */
	public int HashCode() 
	{
		int code = 0;
		for (int i = 0; i < set.Length; i++) {
			code += (int)set[i];// * PRIMES[i];
		}
		return code;
	}
	
	
	/**
     * Used to determine whether two ItemSet objects are equal
     */
	public bool Equals( ItemSet o ) 
	{
		if(this == o)
			return true;
		
		ItemSet other = o;
		if (other.set.Length != this.set.Length) {
			return false;
		}
		for (int i = 0; i < set.Length; i++) {
			if (!contains(other.set, set[i])) {
				return false;
			}
		}
		return true;
	}
	
	public bool contains(float[] otherSet, float d)
	{
		for(int i=0; i<otherSet.Length; i++)
			if(d == otherSet[i])
				return true;
		return false;
	}
	
	public override string ToString()
	{
		string ret = "";
		foreach(float d in set)
			ret += d + " ";
		return ret;
	}
}

