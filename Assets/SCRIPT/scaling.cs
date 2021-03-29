using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class scaling : MonoBehaviour
{
    [SerializeField]
    private ScaleData[] scaleDatas;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        ScaleObj();
    }
    void ScaleObj()
    {
        if(index >= scaleDatas.Length)
        {
            index = 0;
        }
        transform.DOScale(scaleDatas[index].size, scaleDatas[index].time).OnComplete(() => ScaleObj());
        index++;
    }
    [System.Serializable]
    public struct ScaleData
    {
        public float size;
        public float time;
    }
}
