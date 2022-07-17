using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPool : Pool
{
	Canvas c;
	private void Awake()
	{
		c = GetComponent<Canvas>();
	}

	public override Poolable CreateNewPooledObject()
	{
		Poolable newObject = Instantiate(sourceObject.gameObject, c.transform).GetComponent<Poolable>();
		newObject.sourcePool = this;
		newObject.gameObject.name = sourceObject.name + " (Pooled)";
		return newObject;
	}
}
