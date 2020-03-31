using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

    private int count;              //Integer to store the number of pickups collected so far.

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;

    private Rigidbody2D m_rigidBody;
    private Collider2D m_collider;

    private Vector2 m_lastPosition;
    private Vector2 direction;
    private float moveSpeed = 100f;

    private bool m_isDragging = false;

    // Use this for initialization
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();

        //Initialize count to zero.
        count = 0;

        //Initialze winText to a blank string since we haven't won yet at beginning.
        //winText.text = "";

        //Call our SetCountText function which will update the text with the current value for count.
        //SetCountText();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    m_lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    direction = (m_lastPosition - transform.position).normalized;
        //    m_rigidBody.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        //}
        //else
        //{
        //    m_rigidBody.velocity = Vector2.zero;
        //}
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            //theTouch = Input.GetTouch(0);

            //if (theTouch.phase == TouchPhase.Began)
            //{
            //    touchStartPosition = theTouch.position;
            //}

            //else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            //{
            //    touchEndPosition = theTouch.position;

            //    float x = touchEndPosition.x - touchStartPosition.x;
            //    float y = touchEndPosition.y - touchStartPosition.y;

            //    Vector2 force;

               
            //}
        }
    }

    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString();

        //Check if we've collected all 12 pickups. If we have...
        if (count >= 12)
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDragging = true;
        m_collider.enabled = false;
        m_lastPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 displacement = eventData.position - m_lastPosition;
        //displacement.Normalize();

        var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        m_rigidBody.MovePosition(worldPosition);

        //Debug.Log(worldPosition + " :: " + transform.position);
        Debug.Log("drag-ing");

        //var positionDifference = worldPosition - transform.position;

        //if(positionDifference.magnitude < threshold)
        //{
        //    m_rigidBody.velocity = Vector2.zero;
        //}
        //else
        //{
        //    m_rigidBody.AddForce(displacement * 1f);
        //}

        m_lastPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("drag end");
        m_isDragging = false;
        m_rigidBody.velocity = Vector2.zero;

        m_collider.enabled = true;
    }

    public float threshold = 1f;
}
