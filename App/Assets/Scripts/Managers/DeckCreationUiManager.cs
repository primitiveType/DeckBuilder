using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckCreationUiManager : MonoBehaviour
{
    [SerializeField]
    private Button ReturnButton;

    [SerializeField]
    private Transform CardParent;

    [SerializeField]
    private float ScrollMod;

    // Start is called before the first frame update
    void Start()
    {
        ReturnButton.onClick.AddListener(ReturnToOverworld);
    }

 
    void Update()
    {
        if(Mouse.current.scroll.ReadValue() != Vector2.zero)
        {
            ScrollCards();
        }
    }

    private void ScrollCards()
    {
        CardParent.transform.position = new Vector3(CardParent.transform.position.x, CardParent.transform.position.y + Mouse.current.scroll.ReadValue().y * ScrollMod, CardParent.transform.position.z);
    }
 
    public void ReturnToOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }
}
