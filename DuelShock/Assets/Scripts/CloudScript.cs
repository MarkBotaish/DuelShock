using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().color -= new Color(0.0f,0.25f,0.25f,0.0f);
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color += new Color(0.0f, 0.25f, 0.25f, 0.0f);
    }
}
