using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public bool isDont;
    public static SceneChanger instance;
    public List<GameObject> ddoList;

    public bool isLock;
    public Image fadeImage;
    public Image hitFade;
    public List<Enemy> enemys;
    public Text hpText;
    public Transform garbage;

    public void Awake()
    {
        if (!isDont)
        {
            if(ddoList!=null && ddoList.Count>0)
            {
                for(int i=0;i<ddoList.Count;++i)
                    DontDestroyOnLoad(ddoList[i].gameObject);
            }
            isDont = true;
            instance = this;
            isLock = false;
        }

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
        enemys = new List<Enemy>();
        enemys.Clear();
        return;
    }

    public void Start()
    {
        return;
    }

    public void OnReStart()
    {
        garbage = GameObject.Find("Garbage").transform;

        for (int i = 0; i < ddoList.Count; ++i)
            ddoList[i].transform.parent = garbage.transform;
        SceneManager.LoadScene(0);
        return;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            OnReStart();
        }
        return;
    }

    public void OnFade(int code=0)
    {
        if(code==1)
        {
            isLock = false;
            PlayerScript.instance.isInputLock = false;
            PlayerScript.instance.doors = null;
            PlayerScript.instance.trans.position = new Vector3(0, -3.5f, 7.018f);
        }

        fadeImage.enabled = true;
        StartCoroutine(IEFade(code));
        return;
    }

    public IEnumerator IEHitFade()
    {
        hitFade.enabled = true;
        hitFade.color = new Color(1, 0, 0, 0.5f);

        while(true)
        {
            Color c = hitFade.color;
            c.a -= 0.05f;
            hitFade.color = c;

            if (c.a <= 0f)
                break;

            yield return new WaitForEndOfFrame();
        }

        hitFade.enabled = false;
    }

    IEnumerator IEFade(int code=0)
    {
        while(true)
        {
            Color c = fadeImage.color;

            if(code==0)
            {
                c.a += 0.02f;
            }
            else if (code == 1)
            {
                c.a -= 0.02f;
            }

            fadeImage.color = c;

            if(code==0 && c.a>=1f)
            {
                c.a = 1f;
                fadeImage.color = c;
                SceneManager.LoadScene(1);
                OnFade(1);
                break;
            }
            else if (code == 1 && c.a <= 0f)
            {
                c.a = 0f;
                fadeImage.color = c;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}