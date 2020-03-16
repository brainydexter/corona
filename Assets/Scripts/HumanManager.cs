using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private void Awake()
    {
        Debug.Assert(m_pHuman != null, "Human prefab not assigned");

        int N = Random.Range(2, 5);

        m_humans = new Human[N];

        for(int i = 0; i < N; ++i)
        {
            m_humans[i] = Instantiate<Human>(m_pHuman, gameObject.transform);

            if(i % 2 == 0)
            {
                m_humans[i].transform.position = new Vector3(0, Random.Range(1, 10.5f), 0);
            }
            else
            {
                m_humans[i].transform.position = new Vector3(Random.Range(1, 10.5f), 0, 0);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Human m_pHuman;

    private Human[] m_humans;
}
