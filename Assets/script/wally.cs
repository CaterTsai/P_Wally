using UnityEngine;
using System.Collections;
using DG.Tweening;

public class wally : MonoBehaviour {
    
    //enum
    private enum eWallyState
    {
        eState_disable    =   0,
        eState_enter,
        eState_idle,
        eState_exit,
        eState_finish,
        eState_num
    }

    public ParticleSystem _ps = null;
    public GameObject _wally = null;
    public GameObject _mark = null;
    
    public bool _isUseMark = true;

    private Material _wallyMaterial = null;
    private Material _markMaterial = null;
    private eWallyState _eState = eWallyState.eState_disable;
    private Vector3 _markStartPos;
    private Animator _wallyAnim = null;

    private Texture _texNextWally = null;
    private Texture _texNextMark = null;

    #region Basic Method
    //---------------------------------
    void Awake()
    {
        if (_wally != null)
        {
            GameObject child_ = null;
            child_ = _wally.transform.Find("wally_3").gameObject;
            _wallyMaterial = child_.GetComponent<Renderer>().material;

            _wallyAnim = _wally.GetComponent<Animator>();
            
            //Wave hand
            _wallyAnim.Play("wallyWaveHand", 0, Random.Range(0.0f, 1.0f));
            if (!_isUseMark)
            {
                _wallyAnim.speed = 0.0f;
            }
        }


        if (_mark != null)
        {
            _markStartPos = _mark.transform.position;
            _markStartPos.y = 2.0f + _mark.transform.localScale.x * 0.5f;
            _markMaterial = _mark.GetComponent<Renderer>().material;
        }

        reset();
    }
    #endregion

    #region Method
    //---------------------------------
    private void reset()
    {
        _eState = eWallyState.eState_disable;
        _wallyMaterial.SetFloat("_Alpha", 0.0f);
        _wallyMaterial.SetFloat("_MKGlowPower", 0.0f);

        _markMaterial.SetFloat("_Alpha", 1.0f);
        _mark.transform.position = _markStartPos;
    }

    //---------------------------------
    private void setUsedMark(bool val)
    {
        _isUseMark = val;
    }
    #endregion

    #region Highlight
    public void setHighLight(bool val)
    {
        if(val)
        {
            _wallyMaterial.SetFloat("_MKGlowTexStrength", 1.0f);
        }
        else
        {
            _wallyMaterial.SetFloat("_MKGlowTexStrength", 0.0f);
        }
    }
    #endregion

    #region Animation Method

    //---------------------------------
    public void enter()
    {
        if (_eState != eWallyState.eState_disable)
        {
            return;
        }

        if(_ps.isStopped)
        {   
            if(_isUseMark)
            {
                Sequence _enterSeq = DOTween.Sequence();
                _enterSeq.Append(_mark.transform.DOLocalMoveY(-0.5f + _mark.transform.localScale.x/2.0f, 1.0f).SetEase(Ease.OutCirc));
                _enterSeq.AppendCallback(() => _ps.Play());
                _enterSeq.Append(_wallyMaterial.DOFloat(1.0f, "_Alpha", 1.5f).SetEase(Ease.InCubic));
                _enterSeq.Join(_wallyMaterial.DOFloat(constParameter.cWALLY_GLOW_LEVEL, "_MKGlowPower", 1.5f).SetEase(Ease.InCubic));
                _enterSeq.Join(_mark.transform.DOLocalMoveY(0.5f + _mark.transform.localScale.x/2.0f + _mark.transform.localScale.x / 4.0f, 1.5f));
                _enterSeq.AppendCallback(() => {
                    setMarkFloat(true);
                    _eState = eWallyState.eState_idle;
                    });
            }
            else
            {
                _ps.Play();
                _wallyMaterial.DOFloat(1.0f, "_Alpha", 2.0f).SetEase(Ease.InCubic).OnComplete(() => {
                    _eState = eWallyState.eState_idle;
                    });
            }

            _eState = eWallyState.eState_enter;
        }
        
    }

    //---------------------------------
    public void exit(bool continueEnter = false)
    {
        if (_eState != eWallyState.eState_idle)
        {
            return;
        }

        if(_ps.isStopped)
        {
            _ps.Play();
            if (_isUseMark)
            {
                setMarkFloat(false);
                _markMaterial.DOFloat(0.0f, "_Alpha", 1.5f).SetEase(Ease.OutCubic);
            }

           _wallyMaterial.DOFloat(0.0f, "_Alpha", 1.5f).SetEase(Ease.OutCubic).OnComplete(() => {
                reset();
                _eState = eWallyState.eState_disable;

                if(continueEnter)
                {
                    changeTexture();
                    StartCoroutine(WaitPSFinish());
                }
            });

            _eState = eWallyState.eState_exit;
        }
         
    }

    //---------------------------------
    private void setMarkFloat(bool val)
    {
        _mark.GetComponent<markFloat>().setFloat(val, 0.05f);
    }

    //---------------------------------
    public void changeWally(ref Texture mark, ref Texture wally)
    {
        _texNextMark = mark;
        _texNextWally = wally;
        
        exit(true);
    }

    //---------------------------------
    private IEnumerator WaitPSFinish()
    {
        yield return new WaitUntil(()=> _ps.isStopped);
        enter();
    }
    #endregion

    #region Texture
    //---------------------------------
    private void changeTexture()
    {
        _markMaterial.SetTexture("_MainTex", _texNextMark);
        _wallyMaterial.SetTexture("_MainTex", _texNextWally);
    }

    //---------------------------------
    public Texture getMarkTex()
    {
        return _markMaterial.GetTexture("_MainTex");
    }

    //---------------------------------
    public Texture getWallyTex()
    {
        return _wallyMaterial.GetTexture("_MainTex");
    }
    #endregion

}
