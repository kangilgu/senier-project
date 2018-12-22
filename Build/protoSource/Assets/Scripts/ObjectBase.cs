using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour 
{
	public GameObject obj;
	public Transform trans;
	public MaterialPropertyBlock mpb;
	public MeshRenderer mesh;
	public Collider col;
	public Rigidbody rigid;
	public bool isMove;

	public virtual void OnCreate()
	{
		obj=gameObject;
		trans = transform;
		mpb=new MaterialPropertyBlock();
		mesh=GetComponent<MeshRenderer>();
		col=GetComponent<Collider>();
		rigid=GetComponent<Rigidbody>();
		return;
	}
}
