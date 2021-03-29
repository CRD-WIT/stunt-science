using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


public class rotateback : MonoBehaviour
{
    [SerializeField]
    private Vector3 finalrot;
    [SerializeField] Ease ease;
    private Vector3 Originalrot, Currentrot;

    // Start is called before the first frame update
    void Start()
    {
        Originalrot = transform.rotation.eulerAngles;
        Currentrot = finalrot;
        Rotate(Currentrot);
    }
    void Rotate(Vector3 angle)
    {
        transform.DORotate(angle, 1f).SetEase(ease).OnComplete(() => Rotate(Currentrot));
        Currentrot = Currentrot == finalrot ? Originalrot : finalrot;
    }
}

