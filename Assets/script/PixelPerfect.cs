using UnityEngine;
using System.Collections;

public class PixelPerfect : MonoBehaviour {

    public Camera targetCamera;

    // Use this for initialization
    void Start () {
        PixelPerfectUpdate();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void PixelPerfectUpdate()
    {
        if (!targetCamera) targetCamera = Camera.main;
        float cameraScale = (Screen.height / 2.0f) / targetCamera.orthographicSize;
        transform.localScale = new Vector3(Screen.width/ cameraScale , Screen.height / cameraScale , 1);
    }
}
