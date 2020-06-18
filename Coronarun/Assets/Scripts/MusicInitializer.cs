using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInitializer : MonoBehaviour
{
    
    void Start()
    {
    	DontDestroyOnLoad(gameObject);
    	if(FindObjectsOfType<MusicInitializer>().Length > 1)
    	{
    		Destroy(gameObject);
    	} else
    	{
    		FindObjectOfType<AudioManager>().Play("Song");
    	}
    }
}
