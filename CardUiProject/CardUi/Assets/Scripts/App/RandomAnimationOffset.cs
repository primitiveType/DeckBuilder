using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationOffset : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int Offset = Animator.StringToHash("Offset");

    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat(Offset, Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}