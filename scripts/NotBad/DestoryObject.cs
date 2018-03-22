using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour {

    public float playTime = 5.0f;
    

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playTime <= 0.0f)
            Destroy(this.gameObject);

        playTime -= Time.deltaTime;
	}
}
