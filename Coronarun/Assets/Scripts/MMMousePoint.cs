using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MMMousePoint : MonoBehaviour
{
	EventSystem mES;
	BaseRaycaster gR;
	PointerEventData mPED;
	float playProgress;
	bool playTouched;
	float optionsProgress;
	bool optionsTouched;
	float quitProgress;
	bool quitTouched;
	int click;

    void Start()
    {
        mES = GetComponent<EventSystem>();
        gR = GetComponent<BaseRaycaster>();

        playProgress = 0f;
        optionsProgress = 0f;
        quitProgress = 0f;
        click = 0;
    }

    void Update()
    {
    	if(Input.GetMouseButton(0))
    	{
    		click = 1;
    	}
    	if(click == 1 && !Input.GetMouseButton(0))
    	{
    		click = 2;
    	}
    	mPED = new PointerEventData(mES);
    	mPED.position = Input.mousePosition;

    	playTouched = false;
    	optionsTouched = false;
    	quitTouched = false;

    	List<RaycastResult> results = new List<RaycastResult>();
    	gR.Raycast(mPED, results);

    	foreach(RaycastResult result in results)
    	{
    		if(result.gameObject.tag == "playbutton")
    		{
    			playProgress += (1f - playProgress) * 5f * Time.deltaTime;
    			playTouched = true;
    			if(click == 2)
    			{
    				SceneManager.LoadSceneAsync("GameScene");
    			}
    		}
    		if(result.gameObject.tag == "optionsbutton")
    		{
    			optionsProgress += (1f - optionsProgress) * 5f * Time.deltaTime;
    			optionsTouched = true;
    			if(click == 2)
    			{
    				
    			}
    		}
    		if(result.gameObject.tag == "quitbutton")
    		{
    			quitProgress += (1f - quitProgress) * 5f * Time.deltaTime;
    			quitTouched = true;
    			if(click == 2)
    			{
    				Application.Quit();
    			}
    		}
    	}
    	if(!playTouched)
    	{
    		playProgress += (-playProgress) * 5f * Time.deltaTime;
    	}
    	if(!optionsTouched)
    	{
    		optionsProgress += (-optionsProgress) * 5f * Time.deltaTime;
    	}
    	if(!quitTouched)
    	{
    		quitProgress += (-quitProgress) * 5f * Time.deltaTime;
    	}
    	GameObject.Find("PlayButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", playProgress);
    	GameObject.Find("OptionsButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", optionsProgress);
    	GameObject.Find("QuitButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", quitProgress);

    	if(click == 2)
		{
			click = 0;
		}
    }
}
