using UnityEngine;
using System.Collections;

public class boatFloat : MonoBehaviour {
    
    [Range(0.1f, 1.0f)]
    public float boatSize = 0.1f;

    private float rRotateVal = 5.0f;
    private float rFloatVal = 0.01f;
    private double _rVec = Mathf.PI * 0.5f;
    private double _radian = 0.0f;
    private float _startY = 0.0f;
    private float _startRZ = 0.0f;

    // Update is called once per frame
    void Start()
    {
        _radian = Random.Range(0, Mathf.PI);
        _startY = transform.position.y;
        _rVec = Mathf.PI * (1.0f - boatSize);
        rFloatVal = (1.0f - boatSize) * 0.02f;
        rRotateVal = (1.0f - boatSize) * -8.0f;
    }
    void Update ()
    {
        _radian += Time.deltaTime * _rVec;
        if (_radian >= Mathf.PI * 2)
        {
            _radian -= Mathf.PI * 2;
        }

        var pos_ = transform.position;
        
        transform.position = new Vector3(pos_.x, _startY + Mathf.Sin((float)_radian) * rFloatVal, pos_.z);
        transform.localRotation = Quaternion.Euler(0, 0, _startRZ + Mathf.Cos((float)_radian) * rRotateVal);
    }
}
