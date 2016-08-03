using UnityEngine;
using System.Collections;

public class wallyMgr : MonoBehaviour {

    public GameObject[] _hlWally = new GameObject[constParameter.cHIGHLIGHT_WALLY_NUM];
    public GameObject[] _wally = new GameObject[constParameter.cWALLY_NUM];

    private wally[] _hlWallyCtrl = new wally[constParameter.cHIGHLIGHT_WALLY_NUM];
    private wally[] _wallyCtrl = new wally[constParameter.cWALLY_NUM];
    
    private int _hlWIdx = 0;
    private int _wIdx = 0;

    private AudioSource _newWallyEffect = null;

    #region Basic method
    //-------------------------------
    void Start()
    {
        //highlight wally
        for(int idx_ = 0; idx_ < constParameter.cHIGHLIGHT_WALLY_NUM; idx_++)
        {
            _hlWallyCtrl[idx_] = _hlWally[idx_].GetComponent<wally>();
            _hlWallyCtrl[idx_].enter();
        }

        //wally
        for(int idx_ = 0; idx_ < constParameter.cWALLY_NUM; idx_++)
        {
            _wallyCtrl[idx_] = _wally[idx_].GetComponent<wally>();
            _wallyCtrl[idx_].setHighLight(false);
            _wallyCtrl[idx_].enter();
        }

        if(_newWallyEffect == null)
        {
            _newWallyEffect = GetComponent<AudioSource>();
        }
        
    }
    #endregion

    public void newWally(ref Texture texMark, ref Texture texWally)
    {
        if(texMark == null || texWally == null)
        {
            Debug.Log("[ERROR]texMark or texWally is null");
            return;
        }

        var oldMarkTex_ = _hlWallyCtrl[_hlWIdx].getMarkTex();
        var oldWallyTex_ = _hlWallyCtrl[_hlWIdx].getWallyTex();

        _hlWallyCtrl[_hlWIdx].changeWally(ref texMark, ref texWally);
        _wallyCtrl[_wIdx].changeWally(ref oldMarkTex_, ref oldWallyTex_);

        _hlWIdx++;
        _wIdx++;
        _hlWIdx %= constParameter.cHIGHLIGHT_WALLY_NUM;
        _wIdx %= constParameter.cWALLY_NUM;

        //_newWallyEffect.PlayOneShot(_newWallyEffect.clip);
        StartCoroutine(playEffect());
    }

    private IEnumerator playEffect()
    {
        yield return new WaitForSeconds(3.0f);
        _newWallyEffect.PlayOneShot(_newWallyEffect.clip);
    }

}
