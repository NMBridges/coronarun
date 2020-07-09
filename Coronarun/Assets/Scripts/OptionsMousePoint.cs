using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionsMousePoint : MonoBehaviour
{
	EventSystem mES;
	BaseRaycaster gR;
	PointerEventData mPED;
	float saveProgress;
    bool saveTouched;
    float optionsProgress;
    bool optionsTouched;
    float instructionsProgress;
    bool instructionsTouched;
	int click;
	int loadStage;
	float perc;
    int optPage;

    void Start()
    {
        mES = GetComponent<EventSystem>();
        gR = GetComponent<BaseRaycaster>();

        saveProgress = 0f;
        optionsProgress = 0f;
        instructionsProgress = 0f;
        click = 0;
        loadStage = 0;
        optPage = 0;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    void Update()
    {
    	if(Input.GetMouseButton(0) || Input.GetButtonDown("Jump"))
    	{
    		click = 1;
    	}
    	if(click == 1 && !Input.GetMouseButton(0) && !Input.GetButtonDown("Jump"))
    	{
    		click = 2;
    	}
    	mPED = new PointerEventData(mES);
    	mPED.position = Input.mousePosition;

        saveTouched = false;
        optionsTouched = false;
    	instructionsTouched = false;

    	List<RaycastResult> results = new List<RaycastResult>();
    	gR.Raycast(mPED, results);

    	foreach(RaycastResult result in results)
    	{
    		if(result.gameObject.tag == "savebutton")
            {
                saveProgress += (1f - saveProgress) * 5f * Time.deltaTime;
                saveTouched = true;
                if(click == 2 && loadStage == 0)
                {
                    loadStage = 1;
                    perc = 0f;
                }
            }
            if(result.gameObject.tag == "optionstitlebutton")
            {
                optionsProgress += (1f - optionsProgress) * 5f * Time.deltaTime;
                optionsTouched = true;
                if(click == 2 && loadStage == 0)
                {
                    optPage = 0;
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(3).gameObject.SetActive(false);
                }
            }
            if(result.gameObject.tag == "instructionstitlebutton")
            {
                instructionsProgress += (1f - instructionsProgress) * 5f * Time.deltaTime;
                instructionsTouched = true;
                if(click == 2 && loadStage == 0)
                {
                    optPage = 1;
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(3).gameObject.SetActive(true);
                }
            }
    	}
        if(optPage == 0 && !optionsTouched)
        {
            optionsProgress += (1f - optionsProgress) * 5f * Time.deltaTime;
        } else if(optPage == 1 && !instructionsTouched)
        {
            instructionsProgress += (1f - instructionsProgress) * 5f * Time.deltaTime;
        }
    	if(!saveTouched)
        {
            saveProgress += (-saveProgress) * 5f * Time.deltaTime;
        }
        if(!optionsTouched && optPage != 0)
        {
            optionsProgress += (-optionsProgress) * 5f * Time.deltaTime;
        }
        if(!instructionsTouched && optPage != 1)
        {
            instructionsProgress += (-instructionsProgress) * 5f * Time.deltaTime;
        }
        GameObject.Find("BackButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", saveProgress);
        GameObject.Find("OptionsTitleButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", optionsProgress);
    	GameObject.Find("InstructionsTitleButton").GetComponent<RawImage>().material.SetFloat("_WidthVar", instructionsProgress);

    	if(click == 2)
		{
			click = 0;
		}
		if(loadStage > 0)
		{
			load();
		}
    }

    void load()
    {
    	if(loadStage == 1)
    	{
    		perc += 2f * Time.deltaTime;
    		if(perc > 1f)
    		{
    			perc = 1f;
    			loadStage = 2;
    		}
    		GameObject.Find("blackscreen").GetComponent<CanvasGroup>().alpha = perc;
    	} else if(loadStage == 2)
    	{
    		SceneManager.LoadScene("StartScreen");
    	}
    }

    
}
