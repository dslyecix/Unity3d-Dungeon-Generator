using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionCollection : ScriptableObject {

	 public string description;
     public Condition[] requiredConditions = new Condition[0];

     public bool CheckConditions()
     {
        for (int i = 0; i < requiredConditions.Length; i++)
        {
            if (!requiredConditions[i].EvaluateCondition())
            {
                return false;
            }
        }

        return true;
     }

}
