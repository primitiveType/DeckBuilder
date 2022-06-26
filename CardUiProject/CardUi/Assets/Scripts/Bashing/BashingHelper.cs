using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashingHelper : MonoBehaviour
{
    [SerializeField] private GameObject m_CardPrefab;

    public void CreateCard()
    {
        Instantiate(m_CardPrefab);
    }
}
