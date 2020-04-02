using UnityEngine;

using System;
using System.Collections.Generic;

public interface ISubscriberEvents
{
    void RegisterEventHandlers();
    void UnRegisterEventHandlers();
}

public abstract class IEventType
{
    /// <summary>
    /// Reset the object so it can be used again
    /// </summary>
    public abstract void Reset();

    public bool FireImmediately { get; protected set; }

    protected IEventType() { FireImmediately = false; }

    public virtual Type TypeOf { get { return this.GetType(); } }
}

public interface IEventAggregator
{
    void Update();
    void Publish(IEventType e);
    void Register<T>(Action<IEventType> subscriber) where T : IEventType;
    void UnRegister<T>(Action<IEventType> subscriber) where T : IEventType;
}

/*
 * 
 * - http://joseoncode.com/2010/04/29/event-aggregator-with-reactive-extensions/
 * - https://github.com/ephe-meral/EventAggregator-CSharp/tree/master/EpheMeral.EventDrivenDesign
 * 
 * Todo: Make it a singleton and track memory
 */
public class EventAggregator : IEventAggregator
{
    protected Dictionary<Type, Action<IEventType>> collection;

    protected Queue<IEventType> eventQueue;

    public EventAggregator()
    {
        collection = new Dictionary<Type, Action<IEventType>>();

        eventQueue = new Queue<IEventType>();

        //		Debug.Log ("[EventAggregator]: Created event aggregator");
    }

    public void OnDestroy()
    {
        collection.Clear(); collection = null;
        eventQueue.Clear(); eventQueue = null;
        //		Debug.Log ("[EventAggregator]: Destroyed event aggregator");
    }

    /// <summary>
    /// Processes all the events in the eventQueue
    /// </summary>
    public void Update()
    {
        while (this.eventQueue.Count > 0)
        {
            TriggerEvent(eventQueue.Dequeue());
        }
    }

    private void TriggerEvent(IEventType evnt)
    {
        Debug.Assert(evnt != null, "[EventAggregator]: event cannot be null");

        Action<IEventType> subscribers;

        if (collection.TryGetValue(evnt.TypeOf, out subscribers))
        {
            //				Debug.Log ("[EventAggregator]: Processing event: " + evnt);
            subscribers.Invoke(evnt);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("[EventAggregator]: No subscriber for eventtype: " + evnt.GetType());
#endif
        }

        EventBuilder.Instance.Recycle(evnt);

        evnt = null; // marking it for GC
    }

    /// <summary>
    /// Queues the event to be published in the next update
    /// </summary>
    /// <param name="e">E.</param>
    public void Publish(IEventType e)
    {
        if (e.FireImmediately)
            TriggerEvent(e);
        else
            eventQueue.Enqueue(e);
    }

    /// <summary>
    /// Register the specified subscriber for the eventType
    /// </summary>
    /// <param name="subscriber">Subscriber.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public void Register<T>(Action<IEventType> subscriber) where T : IEventType
    {
        Type type = typeof(T);
        //		Debug.Log ("[EventAggregator]: Registering subscriber for eventType: " + type);

        if (!collection.ContainsKey(type))
            collection.Add(type, subscriber);
        else
            collection[type] += subscriber;
    }

    public void UnRegister<T>(Action<IEventType> subscriber) where T : IEventType
    {
        Type type = typeof(T);
        //		Debug.Log ("[EventAggregator]: UnRegistering subscriber for eventType: " + type);

        if (collection.ContainsKey(type))
        {
            collection[type] -= subscriber;
        }
        else
        {
            Debug.LogError("[EventAggregator]: " + subscriber + " not found in collection for event: " + type);
        }
    }
}

