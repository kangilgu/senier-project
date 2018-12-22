using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ObjectBase
{
    int delay;
    int attackDelay;
    float red;
    bool isAttackEnter;
    float speed;
    Vector2 lookRandom;
    public int maxhp;
    public int hp;
    public bool isHit;
    int hitDelay;
    public BoxCollider hitBox;

    public void Awake()
    {
        base.OnCreate();
        hitBox = trans.Find("HitBox").GetComponent<BoxCollider>();
        delay = 0;
        hitDelay = 0;
        red = 0;
        lookRandom = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        trans.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);

        maxhp = 10;
        hp = 10;
        return;
    }

    public void OnHit()
    {
        if (isHit)
            return;

        isHit = true;
        hp -= 1;
        attackDelay = 0;
        hitDelay = 0;
        isAttackEnter = false;
        red = 0;

        mesh.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", Color.white);
        mesh.SetPropertyBlock(mpb);

        trans.localEulerAngles = Vector3.zero;
        trans.LookAt(PlayerScript.instance.trans);
        trans.Translate(Vector3.back * 50f * Time.smoothDeltaTime, Space.Self);

        if (hp<=0)
        {
            SceneChanger.instance.enemys.Remove(this);
            Destroy(gameObject);
        }
        return;
    }

    public void Start()
    {
        SceneChanger.instance.enemys.Add(this);
        return;
    }

    public void Update()
    {
        if(isHit)
        {
            if(++hitDelay>=5)
            {
                isHit = false;
                mesh.GetPropertyBlock(mpb);
                mpb.SetColor("_Color", new Color(0.3f, 0.3f, 0.7f, 1f));
                mesh.SetPropertyBlock(mpb);
            }
            return;
        }

        if (!isAttackEnter)
        {
            if (rigid.velocity == Vector3.zero)
            {
                if (++delay >= 120)
                {
                    delay = 0;
                    rigid.AddForce(Vector3.up * 3f, ForceMode.Impulse);
                }
            }
            if (++attackDelay >= 3)
            {
                attackDelay = 0;
                mesh.GetPropertyBlock(mpb);
                red += 0.02f;
                mpb.SetColor("_Color", new Color(red, 0.3f, 0.3f, 1));
                if (red >= 1f)
                {
                    mpb.SetColor("_Color", new Color(0.3f, 0.3f, 0.7f, 1));
                    isAttackEnter = true;
                    attackDelay = 0;
                    speed = 60;
                    red = 0;
                }
                mesh.SetPropertyBlock(mpb);
            }
        }
        else if(isAttackEnter)
        {
            trans.Rotate(Vector3.forward * 10f);
            trans.Translate(Vector3.forward * speed * Time.smoothDeltaTime, Space.Self);
            speed *= 0.95f;
            if (speed <= 3f)
            {
                trans.localEulerAngles = Vector3.zero;
                isAttackEnter = false;

                Vector3 lookPos = PlayerScript.instance.trans.position;
                lookPos.x += lookRandom.x;
                lookPos.z += lookRandom.y;
                trans.LookAt(lookPos);
            }
            else
            {
                if(col.bounds.Intersects(PlayerScript.instance.col.bounds))
                {
                    PlayerScript.instance.OnHit();
                }
            }
        }
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.layer==LayerMask.NameToLayer("Wall"))
        {
            red = 0;
            trans.localEulerAngles = Vector3.zero;
            isAttackEnter = false;

            Vector3 lookPos = PlayerScript.instance.trans.position;
            lookPos.x += lookRandom.x;
            lookPos.z += lookRandom.y;
            trans.LookAt(lookPos);

        }
        return;
    }
}
