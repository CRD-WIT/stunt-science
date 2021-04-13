using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DimensionManager : MonoBehaviour
{
    public TMP_Text dimensionText;
    public LineRenderer dimensionLine;
    public GameObject endAnnotationArrow;
    public static float dimensionLength;
    Vector3[] position;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float XDistanceFromGround = -2f;
        if (SimulationManager.stage != 3)
        {
            switch (this.gameObject.name)
            {
                case "AnnotationLength":
                    position = new Vector3[2];
                    position[0] = new Vector3(0, XDistanceFromGround, 0);
                    position[1] = new Vector3(dimensionLength, XDistanceFromGround, 0);

                    dimensionLine.positionCount = position.Length;
                    dimensionLine.SetPositions(position);
                    break;
                case "EndAnnotationLine":
                    position = new Vector3[2];
                    position[0] = new Vector3(dimensionLength, -1.25f, 0);
                    position[1] = new Vector3(dimensionLength, -3f, 0);

                    dimensionLine.positionCount = position.Length;
                    dimensionLine.SetPositions(position);
                    break;
            }
        }
        else
        {

        }
        dimensionText.text = dimensionLength.ToString() + "m";
        dimensionText.transform.position = new Vector3(dimensionLength / 2, XDistanceFromGround, 0);
        endAnnotationArrow.transform.position = new Vector3(dimensionLength, XDistanceFromGround, 0);
    }
}
