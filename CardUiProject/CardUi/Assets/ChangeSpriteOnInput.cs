using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnInput : MonoBehaviour
{
    [SerializeField] private Sprite UpSprite;
    [SerializeField] private Sprite DownSprite;
    [SerializeField] private Sprite LeftSprite;
    [SerializeField] private Sprite RightSprite;
    [SerializeField] private SpriteRenderer SpriteRenderer;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpriteRenderer.sprite = UpSprite;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SpriteRenderer.sprite = LeftSprite;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SpriteRenderer.sprite = DownSprite;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SpriteRenderer.sprite = RightSprite;
        }
    }
}
