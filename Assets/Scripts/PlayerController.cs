using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

    private int count;              //Integer to store the number of pickups collected so far.

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;

    private Human m_human;

    // Use this for initialization
    void Awake()
    {
        m_human = GetComponent<Human>();

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
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        m_human.m_rigidBody.AddForce(movement * m_human.m_speed);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }

            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                Vector2 force;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    force = Vector2.right;

                    if (x < 0)
                        force = Vector2.left;
                }
                else
                {
                    force = Vector2.up;

                    if (y < 0)
                        force = Vector2.down;
                }

                m_human.m_rigidBody.AddForce(force * m_human.m_speed);
            }
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
}
