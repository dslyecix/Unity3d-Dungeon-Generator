using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour 
{
    private static Singleton instance = null; 
    public static Singleton Instance 
    { 
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<Singleton>();
				if( instance == null) 
				{
					GameObject go = new GameObject();
					go.name = "Singleton";
					instance = go.AddComponent<Singleton>(); 
					DontDestroyOnLoad(go); 
				}
			}
			return instance;
		}
	}
	  
	void Awake() 
	{
		if(instance == null ) 
		{
			instance = this; 
			DontDestroyOnLoad(this.gameObject); 
		}
		else
		{
			Destroy(gameObject); 
		}
	}
}
