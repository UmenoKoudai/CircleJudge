using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{
    [Header("各地点の座標")]
    [SerializeField, Tooltip("A地点")] 
    private Transform _pointA;
    [SerializeField, Tooltip("B地点")] 
    private Transform _pointB;
    [SerializeField, Tooltip("C地点")] 
    private Transform _pointC;
    [Header("答えを記載するテキスト")]
    [SerializeField, Tooltip("答えのX座標")] 
    private Text _answerTextX;
    [SerializeField, Tooltip("答えのY座標")]
    private Text _answerTextY;
    [SerializeField, Tooltip("答えのZ座標")]
    private Text _answerTextZ;
    [Header("座標間に線を引く")]
    [SerializeField]
    private LineRenderer _render;
    [Header("Rayの設定")]
    [SerializeField]
    private LayerMask _layer = default;
    [SerializeField]
    private float _rayRange = 5.0f;
    [Header("Prefab")]
    [SerializeField]
    private GameObject _centerPrefab;

    private Vector3 _defaultA;
    private Vector3 _defaultB;
    private Vector3 _defaultC;

    public Vector3 PointA => _pointA.position;
    public Vector3 PointB => _pointB.position;
    public Vector3 PointC => _pointC.position;

    private bool _isObjMove = false;
    private Transform _moveObj;
    private Vector3 _answer;

    private void Start()
    {
        _defaultA = _pointA.position;
        _defaultB = _pointB.position;
        _defaultC = _pointC.position;
    }

    private void Update()
    {
        _render.SetPosition(0, _pointA.position);
        _render.SetPosition(1, _pointB.position);
        _render.SetPosition(2, _pointC.position);
        _render.SetPosition(3, _pointA.position);

        if(_isObjMove)
        {
            var mausePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mausePosition.z = 0;
            _moveObj.transform.position = mausePosition;
        }
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, _rayRange, _layer);
        if(Input.GetButtonDown("Fire1"))
        {
            if(hit && !_isObjMove)
            {
                _isObjMove = true;
                _moveObj = hit.transform;
            }
            else
            {
                _isObjMove = false;
            }
        }
    }

    public void Calculation()
    {


        var lineAB = PointB - PointA;
        var lineBC = PointC - PointB;
        var lineCA = PointA - PointC;

        var dotABBC = Vector3.Dot(lineAB, lineBC);
        var dotBCCA = Vector3.Dot(lineBC, lineCA);
        var dotCAAB = Vector3.Dot(lineCA, lineAB);

        if (dotABBC < 0 || dotBCCA < 0 || dotCAAB < 0)
        {
            var a = lineBC.x / 2;
            var b = lineBC.y / 2;
            if (dotABBC > 0) _answer = new Vector2(lineCA.x / 2, lineCA.y / 2);
            if (dotBCCA > 0) _answer = new Vector2(lineAB.x / 2, lineAB.y / 2);
            if (dotCAAB > 0) _answer = new Vector2(lineBC.x / 2, lineBC.y / 2);
        }
        else
        {
            var lengthAB = Mathf.Pow(lineAB.x, 2) + Mathf.Pow(lineAB.y, 2) + Mathf.Pow(lineAB.z, 2);
            var lengthBC = Mathf.Pow(lineBC.x, 2) - Mathf.Pow(lineBC.y, 2) - Mathf.Pow(lineBC.z, 2);

            var dot = Mathf.Pow(Vector3.Dot(lineAB, lineBC), 2);
            var Calculation1 = (1 / (lengthAB * lengthBC) - dot);
            var Calculation2 = (((lengthAB * lengthBC) - (lengthBC * dot)) * lineAB) + (((lengthAB * lengthBC) - (lengthAB * dot)) * lineBC);
            var Calculation3 = (Calculation2 * Calculation1) * 0.5f;

            _answer = PointB + Calculation3;
        }
        _answerTextX.text = _answer.x.ToString();
        _answerTextY.text = _answer.y.ToString();
        _answerTextZ.text = _answer.z.ToString();
        Instantiate(_centerPrefab, _answer, Quaternion.identity);
    }

    public void ResetPosition()
    {
        _pointA.position = _defaultA;
        _pointB.position = _defaultB;
        _pointC.position = _defaultC;
    }
}
