using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GameObjectPool
{
	#region Pool implementation

	/// <summary>
	/// Spawn a gameobject with RecycleHelper attached to it
	/// </summary>
	/// <param name="setActive">If set to <c>true</c> set active.</param>
	public GameObject Spawn (bool setActive = true)
	{
		if (items.Count == 0) {
			//#if UNITY_EDITOR
			//Debug.Log ("[GameObjectPool]: Spawning for: " + prefab.name);
			//#endif

			var clone = Object.Instantiate<GameObject> (prefab);
			clone.gameObject.transform.parent = this.parentTransform;

			clone.SetActive (false);

			var recycleHelper = clone.AddComponent<RecycleHelper> ();
			recycleHelper.Pool = this;

			items.Enqueue (clone);

			if(itemMap != null)
				itemMap.Add (clone.GetInstanceID (), clone);
		}

		var item = items.Dequeue ();
		Debug.Assert (!item.activeInHierarchy, "[GameObjectPool]: item is already active: " + item.gameObject.name);

		item.SetActive (setActive);

		return item;
	}

	/// <summary>
	/// Recycle the specified gameObject.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Recycle (GameObject item)
	{
		//#if UNITY_EDITOR
		//Debug.Log ("[GameObjectPool]: Recycling: " + item.name + " in " + prefab.name + " pool");
		//#endif

		item.SetActive (false);
		items.Enqueue (item);
	}

	/// <summary>
	/// Recycles all gameobjects within this pool
	/// </summary>
	public void RecycleAll()
	{
		if (itemMap == null)
			return;
		
		foreach(var item in this.itemMap.Values) {
			if (item.activeInHierarchy)
				this.Recycle (item);
		}
	}

	public void DestroyAll()
	{
		items.Clear ();

		if (itemMap == null)
			return;
		
		foreach(var item in this.itemMap.Values) {
			Object.Destroy (item);
		}

		itemMap.Clear ();
	}

	/// <summary>
	/// Clear the gameobject pool
	/// </summary>
	public void Clear()
	{
		DestroyAll ();

		prefab = null;
		parentTransform = null;
	}
	#endregion

	public GameObjectPool (GameObject prefab, Transform parent, bool trackItems = false)
	{
		this.prefab = prefab;
		this.parentTransform = parent;

		if (trackItems)
			itemMap = new Dictionary<int, GameObject> ();
	}

	#region members
	private GameObject prefab = null;
	private Transform parentTransform = null;

	private Queue<GameObject> items = new Queue<GameObject>();
	private Dictionary<int, GameObject> itemMap = null;
	#endregion
}

public class RecycleHelper : MonoBehaviour{

	void OnDisable()
	{
		//#if UNITY_EDITOR
		//Debug.Log ("[Recycle Helper]: onDisable called: " + gameObject.name);
		//#endif

		Pool.Recycle (this.gameObject);
	}

	public GameObjectPool Pool { get; set; }
}
