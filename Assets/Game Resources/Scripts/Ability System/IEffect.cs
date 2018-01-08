using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEffect
{
	void Apply(GameObject source, List<GameObject> targets);
}
