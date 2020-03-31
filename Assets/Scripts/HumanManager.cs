using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private void Awake()
    {
        Debug.Assert(m_pHuman != null, "Human prefab not assigned");

        int N = 24;

        int dia = 4;
        int extentsX = 44;
        int extentsY = 22;
        int dimsX = extentsX / dia;
        int dimsY = extentsY / dia;

        HashSet<int> set = new HashSet<int>();

        while(set.Count != N)
        {
            set.Add(UnityEngine.Random.Range(0, dimsX * dimsY));
        }

        m_humans = new Human[N];

        int i = 0;
        foreach(var index in set)
        {
            int row = index / dimsX; row -= (dimsY/2); row *= dia;
            int col = index % dimsX; col -= (dimsX/2); col *= dia;

            m_humans[i] = Instantiate<Human>(m_pHuman, gameObject.transform);
            m_humans[i].transform.position = new Vector3(col, row, 0);

            ++i;
        }
    }

    private void Start()
    {
        m_humans[0].Infect(Symptoms.cCough);
        m_humans[0].m_rigidBody.AddForce(UnityEngine.Random.insideUnitCircle * 350f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Human m_pHuman;

    private Human[] m_humans;
}
