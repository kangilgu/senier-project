using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : ObjectBase
{
    public SphereCollider sphere;
    public bool isFocus;
    public TextMesh text;
    public MeshRenderer handMesh;
    public bool isOpenDoor;
    public int timer;

    public void Awake()
    {
        base.OnCreate();
        isFocus = false;
        text = trans.Find("Text").GetComponent<TextMesh>();
        handMesh = trans.Find("Hand").GetComponent<MeshRenderer>();
        sphere = handMesh.GetComponent<SphereCollider>();
        isOpenDoor = false;
        return;
    }

    public void OnFocusDoor(bool isFocus)
    {
        this.isFocus = isFocus;
        return;
    }

    public void OnRemoteDoorOpen()
    {
        timer = 0;
        isOpenDoor = true;
        PlayerScript.instance.isInputLock = true;
        return;
    }
    
    public void Update()
    {
        if (SceneChanger.instance.isLock)
            return;

        if(isFocus)
        {
            text.color = new Color(0.3f, 0.7f, 0.3f, 1f);
            handMesh.material.SetColor("_Color", new Color(0.3f, 0.7f, 0.3f, 1f));
        }
        else if(!isFocus)
        {
            text.color = Color.black;
            handMesh.material.SetColor("_Color", Color.white);
        }

        if(isOpenDoor)
        {
            Vector3 rot = new Vector3(0, 80, 0);

            trans.localRotation = Quaternion.Lerp(trans.localRotation, Quaternion.Euler(rot), 1f * Time.smoothDeltaTime);

            if(++timer>=100)
            {
                SceneChanger.instance.isLock = true;
                SceneChanger.instance.OnFade();
            }
        }
        return;
    }
}
