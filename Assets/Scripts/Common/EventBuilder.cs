using UnityEngine;

using System;
using System.Collections.Generic;

/*
 * 
 * - http://joseoncode.com/2010/04/29/event-aggregator-with-reactive-extensions/
 * - https://github.com/ephe-meral/EventAggregator-CSharp/tree/master/EpheMeral.EventDrivenDesign
 * 
 * Todo: Make it a singleton and track memory
 */

/// <summary>
/// Event builder spawns an object of type T. 
/// It internally will automatically create a pool of type T
/// spawn will get an object from the pool
/// recycle will mark the object done so it can be reused again
/// </summary>
public class EventBuilder : Singleton<EventBuilder>
{
	/// <summary>
	/// Spawn an instance of event_type T. 
	/// It internally creates a pool so that object can be reused
	/// </summary>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T Spawn<T>() where T : IEventType
	{
		Type type = typeof(T);
		ObjectPool pool = null;

		if (!poolCollection.TryGetValue (type, out pool)) 
		{
			pool = CreatePool (type);
		}

		return pool.Spawn() as T;
	}

	/// <summary>
	/// Recycle the specified object
	/// </summary>
	/// <param name="obj">Object.</param>
	public void Recycle(IEventType obj)
	{
		Type type = obj.GetType ();

		if (!poolCollection.ContainsKey (type)) 
		{
#if UNITY_EDITOR
            Debug.LogWarning ("[EventBuilder]: " + type + " pool not found");
#endif
            return;
		}

		poolCollection [type].Recycle (obj);
	}

	private ObjectPool CreatePool(Type t)
	{
		Type elementType = Type.GetType (t.ToString ());
		Type[] types = new Type[] { elementType };
		Type listType = typeof(ObjectPool<>);
		Type genericType = listType.MakeGenericType (types);
		ObjectPool pool = (ObjectPool)Activator.CreateInstance (genericType);
		poolCollection.Add (t, pool);

//		Debug.Log ("[EventBuilder]: Creating pool for type: " + t);
		return pool;
	}

	private Dictionary<Type, ObjectPool> poolCollection = new Dictionary<Type, ObjectPool>();
}


public interface ObjectPool
{
	IEventType Spawn();
	void Recycle(IEventType obj);
}

public class ObjectPool<T> : ObjectPool where T : IEventType
{
	public ObjectPool ()
	{
		readyObjects = new Queue<IEventType> ();
	}

	public IEventType Spawn()
	{
		if (readyObjects.Count == 0) {
			readyObjects.Enqueue (Activator.CreateInstance<T> ());
//			Debug.Log ("[EventBuilder]: Enqueue object of type " + typeof(T));
		}

		IEventType obj = readyObjects.Dequeue ();
//		Debug.Log ("[EventBuilder]: Dequeue object of type " + typeof(T));

		return obj;
	}

	public void Recycle(IEventType obj)
	{
		readyObjects.Enqueue (obj);

//		Debug.Log ("[EventBuilder]: Recycle object of type " + obj.GetType());
	}

	private Queue<IEventType> readyObjects;
}