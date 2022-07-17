using System.Collections.Generic;
using UnityEngine;

public abstract class Pool : MonoBehaviour
{
	[SerializeField] protected Poolable sourceObject;
	protected readonly Queue<Poolable> itemPool = new Queue<Poolable>();

	protected List<Poolable> activeItems = new List<Poolable>();

	public virtual Poolable GetPooledObject()
	{
		Poolable newObject = itemPool.Count == 0 ? CreateNewPooledObject() : itemPool.Dequeue();
		newObject.gameObject.SetActive(true);
		activeItems.Add(newObject);
		return newObject;
	}

	public virtual void ReturnPooledObject(Poolable returningObject)
	{
		itemPool.Enqueue(returningObject);
		activeItems.Remove(returningObject);
		returningObject.gameObject.SetActive(false);
	}

	public virtual Poolable CreateNewPooledObject()
	{
		//Debug.LogWarning($"Ran out of items in the {gameObject.name} pool, instantiating a new instance");
		Poolable newObject = Instantiate(sourceObject.gameObject).GetComponent<Poolable>();
		newObject.sourcePool = this;
		newObject.gameObject.name = sourceObject.name + " (Pooled)";
		return newObject;
	}
}