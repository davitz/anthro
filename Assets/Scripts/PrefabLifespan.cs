using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLifespan : MonoBehaviour {

    public float LifeSpan = 3.0f;

	void Start ()
    {
        if (LifeSpan > 0)
            Destroy(this.gameObject, LifeSpan);
	}
	
}
