using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIR
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    LEFT_UP,
    RIGHT_UP,
    LEFT_DOWN,
    RIGHT_DOWN
};

public class PlayerScript : ObjectBase
{
    public static PlayerScript instance;

    float angle = 0;
    Vector3 rotVector;
    public SphereCollider sphere;
    public List<Door> doors;
    public bool isInputLock;

    public bool isAttack;
    public int attackStep;
    public Transform armParent;
    Quaternion armRot;
    public TrailRenderer trail;
    public BoxCollider attackBox;

    public bool isDash;
    public float dashSpeed;
    public float maxhp;
    public float hp;
    public bool isHit;

    void Awake()
    {
        instance = this;
        isInputLock = false;
        trail.enabled = false;
        maxhp = 30;
        hp = maxhp;
        trail.Clear();
        base.OnCreate();
        return;
    }

    void Start()
    {
        SceneChanger.instance.hpText.text = hp + " / " + maxhp;
        return;
    }

    public void OnHit()
    {
        if (isHit)
            return;

        isHit = true;
        CameraScript.instance.SetShake();
        StartCoroutine(SceneChanger.instance.IEHitFade());

        hp--;
        SceneChanger.instance.hpText.text = hp + " / " + maxhp;
        if (hp<=0)
        {
            hp = 0;
            SceneChanger.instance.hpText.text = hp + " / " + maxhp;
            SceneChanger.instance.OnReStart();
        }
        return;
    }

    void Update()
    {
        if (isInputLock)
            return;
        if (SceneChanger.instance.isLock)
            return;

        if (doors != null && doors.Count > 0)
        {
            for (int i = 0; i < doors.Count; ++i)
            {
                if (sphere.bounds.Intersects(doors[i].sphere.bounds))
                    doors[i].OnFocusDoor(true);
                else
                    doors[i].OnFocusDoor(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            trail.enabled = true;
            trail.Clear();

            //-165 //60
            armRot = Quaternion.Euler(new Vector3(180, 0, 0));
            armParent.localEulerAngles = new Vector3(180, 0, 0);
            isAttack = true;
            attackStep = 0;
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            isDash = true;
            dashSpeed = 70;
        }

        if(isAttack)
        {
            if(attackStep==0)
            {
                armRot = Quaternion.RotateTowards(armRot, Quaternion.Euler(new Vector3(0, 0, 0)), 20f);
                armParent.localRotation = armRot;
                if(armRot==Quaternion.Euler(new Vector3(0, 0, 0)))
                {
                    trail.enabled = false;
                    trail.Clear();
                    isAttack = false;
                }
                else
                {
                    for(int i = 0; i < SceneChanger.instance.enemys.Count;++i)
                    {
                        if(attackBox.bounds.Intersects(SceneChanger.instance.enemys[i].hitBox.bounds))
                        {
                            CameraScript.instance.SetShake();
                            SceneChanger.instance.enemys[i].OnHit();
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            if (doors != null && doors.Count > 0)
            {
                for (int i = 0; i < doors.Count; ++i)
                {
                    if (doors[i].isFocus)
                    {
                        doors[i].OnRemoteDoorOpen();
                        return;
                    }
                }
            }
        }
        
        bool isMoveKeyDown = false;
        bool isTiltMove = false;

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            angle = 45f;

            isTiltMove = true;
            isMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            angle = -45f;

            isTiltMove = true;
            isMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            angle = 135f;

            isTiltMove = true;
            isMoveKeyDown = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            angle = -135f;

            isTiltMove = true;
            isMoveKeyDown = true;
        }
        if (!isTiltMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                angle = -90f;
                isMoveKeyDown = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                angle = 90f;
                isMoveKeyDown = true;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                angle = 0;
                isMoveKeyDown = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                angle = 180;
                isMoveKeyDown = true;
            }
        }
        
        rotVector.y = angle;
        
        obj.transform.localRotation = Quaternion.RotateTowards(obj.transform.localRotation, Quaternion.Euler(rotVector), 30f);
        
        if(isDash)
        {
            trans.Translate(Vector3.forward * dashSpeed * Time.smoothDeltaTime, Space.Self);
            dashSpeed *= 0.8f;
            if (dashSpeed <= 1f)
                isDash = false;
        }
        else if (isMoveKeyDown && obj.transform.localRotation == Quaternion.Euler(rotVector))
        {
            trans.Translate(Vector3.forward * 10f * Time.smoothDeltaTime, Space.Self);
            
            if(rigid.velocity==Vector3.zero)
                rigid.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }

        Vector3 pos = trans.position;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (pos.z > 8.45f)
                pos.z = 8.45f;
            else if (pos.z < 5.5f)
                pos.z = 5.5f;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
        }

        trans.position = pos;

        return;
    }
}