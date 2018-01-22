using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHelper : MonoBehaviour {

    public float time;

#region SINGLETON PATTERN
    public static AbilityHelper _instance;
    public static AbilityHelper Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AbilityHelper>();
                
                if (_instance == null)
                {
                    GameObject container = new GameObject("AbilityHelper");
                    _instance = container.AddComponent<AbilityHelper>();
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

    public IEnumerator WaitForTime (float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator WaitForFinish ()
    {
        yield return null;
    }
}
