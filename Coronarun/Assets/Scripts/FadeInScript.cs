using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInScript : MonoBehaviour
{
	int loadStage;
	float perc;

    void Awake()
    {
    	loadStage = 0;
    	perc = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(loadStage == 0)
        {
        	load();
        }
    }

    void load()
    {
    	perc -= 2f * Time.deltaTime;
		if(perc < 0f)
		{
			perc = 0f;
			loadStage = 1;
		}
		GetComponent<CanvasGroup>().alpha = perc;
    }
}
