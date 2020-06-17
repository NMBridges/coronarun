using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
	float perc;
	int transition;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
		reset();
	}

	void reset()
	{
		perc = 0f;
		transition = 0;
	}

	void Update()
	{
		if(transition == 1)
		{
			perc += 2f * Time.deltaTime;
			if(perc > 1f)
			{
				perc = 1f;
				transition = 2;
			}
			GameObject.Find("blackscreen").GetComponent<CanvasGroup>().alpha = perc;
		} else if(transition == 2)
		{
			SceneManager.LoadScene("StartScreen");
			reset();
		} else if(transition == 3)
		{
			perc += 2f * Time.deltaTime;
			if(perc > 1f)
			{
				perc = 1f;
				transition = 4;
			}
			GameObject.Find("blackscreen").GetComponent<CanvasGroup>().alpha = perc;
		} else if(transition == 4)
		{
			SceneManager.LoadScene("OptionsScene");
			reset();
		}
	}
    public void optionsToMain()
    {
    	transition = 1;
    }
    public void mainToOptions()
    {
    	transition = 3;
    }
    public void gameToMain()
    {
    	transition = 1;
    }
}
