using UnityEngine;
using System.Collections;

public class markFloat : MonoBehaviour
{
    public float _radius = 0.05f;

    private bool _isFloat = false;
    private double _rVec = Mathf.PI * 0.5f;
    private float _startY = 0.0f;
    private double _radian = 0.0f;
    
	void Update ()
    {   
	    if(_isFloat)
        {
            _radian += Time.deltaTime * _rVec;
            if(_radian > Mathf.PI * 2)
            {
                _radian -= Mathf.PI * 2;
            }

            var pos_ = transform.position;
            transform.position = new Vector3(pos_.x, _startY + Mathf.Sin((float)_radian) * _radius, pos_.z);
        }
	}

    public void setFloat(bool val, float radius = 0.0f)
    {
        _isFloat = val;
        if(val)
        {
            _radian = 0.0f;
            _radius = radius;
            _startY = transform.position.y;
        }
    }
}
