using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	Color color;
    void Awake()
    {
    	color = GetComponent<Image>().color;
    	if(GameObject.Find("LoadingScreenColorObject") != null)
    	{
    		GetComponent<Image>().color = GameObject.Find("LoadingScreenColorObject").GetComponent<LoadingScreenColor>().col;
		} else
		{
			GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
    }

    void Update()
    {
        if(GameObject.Find("LoadingScreenColorObject") != null)
    	{
    		GetComponent<Image>().color = GameObject.Find("LoadingScreenColorObject").GetComponent<LoadingScreenColor>().col;
		} else
		{
			GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
    }
}
