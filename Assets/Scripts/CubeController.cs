using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {
    Vector3 initialPosition;
    Quaternion initialRotation;
    public bool Moving { set; get; } = false;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Moving)
        {
            transform.Rotate(Vector3.up, 1f);
        }
	}

    public void ResetCube()
    {
        ChangeColor(0);
        Moving = false;
        transform.SetPositionAndRotation(initialPosition, initialRotation);
    }

    public void ChangeColor(int colorId)
    {
        Color[] colors = { Color.white, Color.blue, Color.red };
        var color = colors[Mathf.Clamp(colorId, 0, colors.Length - 1)];
        GetComponent<Renderer>().material.color = color;
    }
}
