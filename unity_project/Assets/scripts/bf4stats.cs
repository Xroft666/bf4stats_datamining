using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bf4stats : MonoBehaviour 
{
    public int numberOfPages = 1;
    private TextMesh textMesh;
    private string leaderboardURI = "http://bf4stats.com/leaderboards/pc_player_score?start=";

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    IEnumerator Start()
    {
        string nameLineStart = "<a href=\"/pc/";
		string nameLineEnd = "</a>";

        List<string> names = new List<string>(); 

		print ("Loading...");

        for( int i = 0; i < numberOfPages; i++ )
            using (WWW www = new WWW(leaderboardURI + i) )
            {
				// extracting the html page
                yield return www;
				
				int startIndex = 0;
				while( (startIndex = www.text.IndexOf(nameLineStart, startIndex)) != -1 )
				{
					// gettin to the start of name
					startIndex = www.text.IndexOf("\">", startIndex);
					// getting to the end of name
					int endIndex = www.text.IndexOf(nameLineEnd, startIndex);
					// extracting the name
					names.Add( www.text.Substring(startIndex + 2, endIndex - startIndex - 2) );
            	}
        }
	
		foreach( string name in names )
			print (name);

		print ("" + names.Count + " names crawled in: " + Time.time + " seconds.");
    }
}
