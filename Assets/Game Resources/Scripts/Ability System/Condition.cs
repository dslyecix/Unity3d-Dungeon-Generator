﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : ScriptableObject {

    public string description;
    public int hash;

    public bool EvaluateCondition()
    {
        return true;
    }

}
