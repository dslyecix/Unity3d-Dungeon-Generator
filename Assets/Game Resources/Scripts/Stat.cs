using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
	private int baseValue;

    private List<int> addModifiers = new List<int>();
    private List<float> multiplyModifiers = new List<float>();

    public int GetValue()
    {
        int finalValue = baseValue;

        addModifiers.ForEach(x => finalValue += x);
        multiplyModifiers.ForEach(x => finalValue *= (int)x);
        return finalValue;
    }

    public void AddMultiplyModifier(float modifier)
    {
        if (modifier != 0) multiplyModifiers.Add(modifier);
    }

    public void AddModifier(int modifier)
    {
         if (modifier != 0) addModifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0) addModifiers.Remove(modifier);
    }

    public void RemoveMultiplyModifier(int modifier)
    {
        if (modifier != 0) multiplyModifiers.Remove(modifier);
    }
}
