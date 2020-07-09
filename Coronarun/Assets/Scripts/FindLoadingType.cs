using UnityEngine;
using UnityEngine.UI;

public class FindLoadingType : MonoBehaviour
{
	public TMPro.TMP_Dropdown dd;
	public TMPro.TMP_Dropdown ddb;
	public Slider sl;
	public Slider sli;
	int valuu;
	float volum;
	float volum2;
	int bloodvalue;


	void Start()
	{
		if(FindObjectsOfType<LoadingScreenColor>() != null)
		{
			valuu = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().valu;
			if(dd != null)
			{
				dd.value = valuu;
			}
			volum = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().volumee;
			if(sl != null)
			{
				sl.value = volum;
			}
			volum2 = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().volumeeq;
			if(sli != null)
			{
				sli.value = volum2;
			}
			bloodvalue = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().bloodval;
			if(ddb != null)
			{
				ddb.value = bloodvalue;
			}
		}
	}

    public void findObject(int val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleInputData(val);
    	valuu = val;
    }

    public void changeBlood(int val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleBloodInputData(val);
    	bloodvalue = val;
    }

    public void changeVolume(float val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleVolumeData(val);
		AudioSource[] sources = GameObject.Find("EventSystem").GetComponents<AudioSource>();
		Sound[] audman = GameObject.Find("EventSystem").GetComponent<AudioManager>().sounds;
		for(int i = 0; i < sources.Length; i++)
		{
			if(GameObject.Find("EventSystem").GetComponent<AudioManager>().sounds[i].type == "song")
			{
				sources[i].volume = val * audman[i].volume;
			}
		}
		volum = val;
    }

    public void changeSFXVolume(float val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleSFXVolumeData(val);
		AudioSource[] sources = GameObject.Find("EventSystem").GetComponents<AudioSource>();
		Sound[] audman = GameObject.Find("EventSystem").GetComponent<AudioManager>().sounds;
		for(int i = 0; i < sources.Length; i++)
		{
			if(GameObject.Find("EventSystem").GetComponent<AudioManager>().sounds[i].type == "SFX")
			{
				sources[i].volume = val * audman[i].volume;
			}
		}
		volum2 = val;
    }
}
