using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public GameObject player;

    [SerializeField]
    private TextMeshProUGUI distanceTextGUI;

    public GameObject ReDotStart;
    public GameObject ReDotEnd;

    float distance;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition = new Vector3(player.transform.position.x, 1, 0f);
        Vector3 endPosition = new Vector3(0, 1, 0f);
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        ReDotStart.transform.position = startPosition;
        ReDotEnd.transform.position = endPosition;

        distanceTextGUI.transform.position = new Vector3(startPosition.x + 1.5f, 1, 0f);
    }
}
