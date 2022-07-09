using App.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[ExecuteInEditMode]
public class CardUiContainer : MonoBehaviour
{
    [SerializeField] private GameObject _Card;

    private RectTransform MyRectTransform { get; set; }

    private void Awake()
    {
        MyRectTransform = GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateSizeOfCard();
    }

    public void AddCard(GameObject card)
    {
        _Card = card;
        card.transform.SetParent(transform);
        card.transform.localPosition = new Vector3(0, 0, -1);
        UpdateSizeOfCard();
    }

    private void UpdateSizeOfCard()
    {
        _Card.transform.SetLayerRecursively(LayerMask.NameToLayer("card"));
        _Card.transform.localScale = Vector3.one;
        BoxCollider box = _Card.GetComponentInChildren<BoxCollider>(); //treat a box as this thing's bounds.
        Camera cam = Camera.main;
        float heightOfFrustum = cam.orthographicSize * 2;
        Vector2 sizeOfFrustum = new Vector2(heightOfFrustum, cam.aspect * heightOfFrustum);

        Vector2 mySize = MyRectTransform.rect.size;
        Vector2 cardSize = box.size;
        float cardAspect = _Card.transform.localScale.x / _Card.transform.localScale.y; //2:3, .66
        float rtAspect = mySize.x / mySize.y; //1:1, 1

        

        Vector2 newDimensions = new Vector2(mySize.x / cardSize.x, mySize.y / cardSize.y); //1:1, 1
        float newAspect = newDimensions.x / newDimensions.y;
        float diffRatio = cardAspect / newAspect;

        if (newAspect > cardAspect)
        { //its stretching it horizontally, so we need to unstretch it
            //reduce the x value until the ratios match.
            newDimensions.x = newDimensions.y * cardAspect;
        }
        else if (newAspect < cardAspect)
        { //its stretching it vertically, so we need to unstretch it
            newDimensions.y = newDimensions.x / cardAspect;
        }

        _Card.transform.localScale = new Vector3(newDimensions.x, newDimensions.y, 1);
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateSizeOfCard();
        }
    }
}
