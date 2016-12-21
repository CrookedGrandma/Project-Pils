using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nameplate : MonoBehaviour {

    public GameObject nameplatePrefab;
    public string nameplateName;

	// Use this for initialization
	void Start () {
        GameObject nameplate = Instantiate(nameplatePrefab) as GameObject;
        nameplate.transform.parent = gameObject.transform;
        nameplate.transform.localPosition = new Vector3(0, 1.5f, -1);

        nameplate.GetComponentInChildren<TextMesh>().text = nameplateName;
	}
}
