using System.Collections;
using System.Collections.Generic;
using SummerJam1;
using UnityEngine;
using UnityEngine.UI;

public class LeaveShopButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        GameContext.Instance.Game.EndShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
