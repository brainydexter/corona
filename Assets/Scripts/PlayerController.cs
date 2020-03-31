using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Rigidbody2D m_rigidBody;
    private Collider2D m_collider;

    // Use this for initialization
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_collider.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        m_rigidBody.MovePosition(worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_rigidBody.velocity = Vector2.zero;
        m_collider.enabled = true;
    }
}
