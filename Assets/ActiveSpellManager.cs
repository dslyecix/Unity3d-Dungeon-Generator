using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpellManager : MonoBehaviour {

    public float time;

#region SINGLETON PATTERN
    public static ActiveSpellManager _instance;
    public static ActiveSpellManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ActiveSpellManager>();
                
                if (_instance == null)
                {
                    GameObject container = new GameObject("ActiveSpellManager");
                    _instance = container.AddComponent<ActiveSpellManager>();
                }
            }
        
            return _instance;
        }
    }
#endregion

    void Update()
    {
        time = Time.time;
    }
}
