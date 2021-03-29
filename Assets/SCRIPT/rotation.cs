using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class rotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 finalrot;
    private Vector3 Originalrot, Currentrot;


    // Start is called before the first frame update
    void Start()
    {
      
    }
    void Rotate(Vector3 angle)
    {
        transform.DORotate(angle, 1f);
    }




}
