using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorBoxScript : MonoBehaviour {

	public void diplayError(string error)
    {
        gameObject.GetComponent<Text>().text = error;
        StartCoroutine("destroyText");
        
    }

    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<Text>().text = "";
    }
}
