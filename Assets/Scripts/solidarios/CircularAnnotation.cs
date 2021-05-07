using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircularAnnotation : MonoBehaviour
{
    public enum Axis { X, Y, Z };
    [SerializeField]
    private int _segments = 60;
    [SerializeField]
    private float _horizRadius = 2;
    [SerializeField]
    private float _vertRadius = 2;
    public float _degrees = 360;
    [SerializeField]
    private Axis _axis = Axis.Z;
    [SerializeField]
    private bool _checkValuesChanged = true;
    private int _previousSegmentsValue;


    private float _previousHorizRadiusValue;
    private float _previousVertRadiusValue;
    private float _previousAngleValue;
    public Vector2 textOffset;

    private float _previousOffsetValue;
    private Axis _previousAxisValue;
    GameObject textDimension;
    public float initialAngle = 0f;
    private float _previousInitialAngleValue;
    public Vector2 _origin;
    private Vector2 _previousOriginValue;
    public float fontSize = 4;
    private float _offset = 0;
    private LineRenderer _line;
    public bool revealValue = true;


    public GameObject[] arrows = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        _line = transform.Find("Line").GetComponent<LineRenderer>();
        _line.positionCount = _segments + 1;
        _line.useWorldSpace = false;
        textDimension = transform.Find("Text").gameObject;

        UpdateValuesChanged();

        CreatePoints();
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void SetAngle(float angle)
    {
        this._degrees = angle;
        this.initialAngle = angle + 16.7f;
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_checkValuesChanged)
        {
            if (_previousSegmentsValue != _segments ||
                _previousHorizRadiusValue != _horizRadius ||
                _previousVertRadiusValue != _vertRadius ||
                _previousOffsetValue != _offset ||
                _previousAxisValue != _axis ||
                _previousAngleValue != _degrees ||
                _previousInitialAngleValue != initialAngle ||
                _previousOriginValue != _origin)
            {
                CreatePoints();
            }

            UpdateValuesChanged();
        }

        if (revealValue)
        {
            textDimension.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(_degrees, 2)}deg");
            textDimension.GetComponent<TextMeshPro>().fontSize = fontSize;
        }
        else
        {
            textDimension.GetComponent<TextMeshPro>().SetText("?");
            textDimension.GetComponent<TextMeshPro>().fontSize = fontSize;
        }

        textDimension.transform.position = new Vector3(_origin.x + textOffset.x, _origin.y + textOffset.y, 1);

        arrows[0].transform.position = new Vector3(_line.GetPosition(_segments).x, _line.GetPosition(_segments).y, 1);
        arrows[0].transform.rotation = Quaternion.Euler(0, 0, (_degrees * -1) - initialAngle);

        arrows[1].transform.position = new Vector3(_line.GetPosition(0).x, _line.GetPosition(0).y, 1);
        arrows[1].transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, arrows[0].transform.rotation.z + initialAngle));

        if (arrows[0].transform.position.y > 0)
        {
            initialAngle += 0.1f;
        }
        else
        {
            initialAngle -= 0.1f;
        }
    }

    void UpdateValuesChanged()
    {
        _previousSegmentsValue = _segments;
        _previousHorizRadiusValue = _horizRadius;
        _previousVertRadiusValue = _vertRadius;
        _previousOffsetValue = _offset;
        _previousAxisValue = _axis;
        _previousAngleValue = _degrees;
        _previousOriginValue = _origin;
        _previousInitialAngleValue = initialAngle;
    }

    void CreatePoints()
    {

        if (_previousSegmentsValue != _segments)
        {
            _line.positionCount = _segments + 1;
        }

        float x;
        float y;
        float z = _offset;
        float angle = 0f;

        for (int i = 0; i < (_segments + 1); i++)
        {
            float targetAngel = initialAngle + angle;
            x = Mathf.Sin(Mathf.Deg2Rad * targetAngel) * (_horizRadius);
            y = Mathf.Cos(Mathf.Deg2Rad * targetAngel) * (_vertRadius);

            switch (_axis)
            {
                case Axis.X:
                    _line.SetPosition(i, new Vector3(z, y + _origin.y, x + _origin.x));
                    break;
                case Axis.Y:
                    _line.SetPosition(i, new Vector3(y + _origin.y, z, x + _origin.x));
                    break;
                case Axis.Z:
                    _line.SetPosition(i, new Vector3(x + _origin.x, y + _origin.y, z));
                    break;
                default:
                    break;
            }

            angle += (_degrees / _segments);
        }
    }
}
