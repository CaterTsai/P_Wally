using UnityEngine;
using System.Collections;

public class cloud : MonoBehaviour {

    private Material _cloudTex = null;
    private float _offsetX = 0.0f;

	void Start ()
    {
        _cloudTex = this.GetComponent<Renderer>().material;
	}

	void Update ()
    {
        _offsetX += Time.deltaTime * constParameter.cCLOUD_SPEED;
        if(_offsetX >=1.0f)
        {
            _offsetX -= 1.0f;
        }

        _cloudTex.SetTextureOffset("_MainTex", new Vector2(_offsetX, 0));
	}
}
