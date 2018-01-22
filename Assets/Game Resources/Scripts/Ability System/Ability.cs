﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Abilities/New Ability")]
[System.Serializable]
public class Ability : Effect
{
    public Sprite sprite;

    public int manaCost;
	[TextArea(5, 3)] public string description;

    public override void Execute(GameObject source, ref List<GameObject> targets)
    {
        Debug.Log("Ability has been cast!");
        ExecuteEffects(source, ref targets);
    
    }
}
