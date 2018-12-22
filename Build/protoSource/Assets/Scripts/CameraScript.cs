using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{
	public static CameraScript instance;
	public Transform trans;
	public ObjectBase targetObject;
    public bool isShake;
    public float shake;

    public void Awake()
    {
        instance =this;
        trans = transform;
        return;
    }

    public void SetShake()
    {
        isShake = true;
        shake = 0.3f;
        return;
    }

	public void LateUpdate()
	{
		if(targetObject==null)
			return;
        if (SceneChanger.instance.isLock)
            return;
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            trans.localEulerAngles = Vector3.zero;
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            trans.localEulerAngles = new Vector3(45, 0, 0);

        Vector3 pos = transform.position;

		Vector3 targetPos=targetObject.trans.position;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            targetPos.z -= 15f;

        pos = Vector3.Lerp(pos, targetPos, 10f * Time.smoothDeltaTime);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            pos.y = -1.5f;
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            pos.y = 15f;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            pos.z = -2.5f;
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (pos.z <= -25f)
                pos.z = -25f;
        }

        if (isShake)
        {
            Vector2 shakeRatio = Random.insideUnitCircle * shake;
            pos.x += shakeRatio.x;
            pos.z += shakeRatio.y;
            shake = Mathf.MoveTowards(shake, 0, 1f);
            if (shake == 0f)
                isShake = false;
        }

        transform.position = pos;
		return;
	}
}
