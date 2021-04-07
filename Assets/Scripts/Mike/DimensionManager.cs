using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DimensionManager : MonoBehaviour
{
    public TMP_Text dimensionText;
    public LineRenderer dimensionLine;
    public static float dimensionLength;
    Vector3[] position;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // float YConstant = 0.75f;
        float XDistanceFromGround = 0.75f;
        dimensionText.text = dimensionLength.ToString();
        dimensionText.transform.position = new Vector3(dimensionLength / 2, XDistanceFromGround, 0);
        // position = new Vector3[6];
        // position[0] = new Vector3(0, YConstant, 0);
        // position[1] = new Vector3(0, -YConstant, 0);
        // position[2] = new Vector3(0, XDistanceFromGround, 0);
        // position[3] = new Vector3(dimensionLength, XDistanceFromGround, 0);
        // position[4] = new Vector3(dimensionLength, YConstant, 0);
        // position[5] = new Vector3(dimensionLength, -YConstant, 0);
        position = new Vector3[2];
        position[0] = new Vector3(0, XDistanceFromGround, 0);
        position[1] = new Vector3(dimensionLength, XDistanceFromGround, 0);

        dimensionLine.positionCount = position.Length;
        dimensionLine.SetPositions(position);
    }
}
