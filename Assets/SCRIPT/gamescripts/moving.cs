using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class moving : MonoBehaviour
{
    [SerializeField]
    private Vector3 finalpos;
    [SerializeField]
    private Ease ease;
    private Vector3 originalpos, currentpos;

    // Start is called before the first frame update
    void Start()
    {
        originalpos = transform.position;
        currentpos = finalpos;
        Move(currentpos);


    }
    private void Move(Vector3 pos)
    {
        transform.DOMove(pos, 6.67f);
    }

   
}
