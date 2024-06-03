using UnityEngine;

public class PositionInFrontOfMainCamera : MonoBehaviour
{
    [SerializeField] private Vector3 Offset;

    private Camera Camera { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        Camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 camPosition = Camera.transform.TransformPoint(Offset);

        transform.position = camPosition;
    }
}
