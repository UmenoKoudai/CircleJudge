using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{
    [Header("�e�n�_�̍��W")]
    [SerializeField, Tooltip("A�n�_")] 
    private Vector3 _pointA;
    [SerializeField, Tooltip("B�n�_")] 
    private Vector3 _pointB;
    [SerializeField, Tooltip("C�n�_")] 
    private Vector3 _pointC;
    [Header("�e�n�_�̃I�u�W�F�N�g")]
    [SerializeField, Tooltip("A�̃I�u�W�F�N�g")]
    private Transform _pointAObj;
    [SerializeField, Tooltip("B�̃I�u�W�F�N�g")]
    private Transform _pointBObj;
    [SerializeField, Tooltip("C�̃I�u�W�F�N�g")]
    private Transform _pointCObj;
    [Header("�������L�ڂ���e�L�X�g")]
    [SerializeField, Tooltip("������X���W")] 
    private Text _answerTextX;
    [SerializeField, Tooltip("������Y���W")]
    private Text _answerTextY;
    [SerializeField, Tooltip("������Z���W")]
    private Text _answerTextZ;
    [Header("���W�Ԃɐ�������")]
    [SerializeField]
    private LineRenderer _render;
    [Header("Ray�̐ݒ�")]
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

    public Vector3 PointA => _pointA;
    public Vector3 PointB => _pointB;
    public Vector3 PointC => _pointC;

    private bool _isObjMove = false;
    private Transform _moveObj;
    private Vector3 _answer;
    private GameObject _agoCircle;

    private void Start()
    {
        _defaultA = _pointA;
        _defaultB = _pointB;
        _defaultC = _pointC;
    }

    private void Update()
    {
        _render.SetPosition(0, _pointA);
        _render.SetPosition(1, _pointB);
        _render.SetPosition(2, _pointC);
        _render.SetPosition(3, _pointA);

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
        //�e�n�_���q���x�N�g��
        var lineAB = PointB - PointA;
        var lineBC = PointC - PointB;
        var lineCA = PointA - PointC;

        //�݊p���s�p���𒲂ׂ邽�߂ɓ��ς��v�Z
        var dotABBC = Vector3.Dot(lineAB, lineBC);
        var dotBCCA = Vector3.Dot(lineBC, lineCA);
        var dotCAAB = Vector3.Dot(lineCA, lineAB);


        if (dotABBC < 0 && dotBCCA < 0 && dotCAAB < 0)
        {

            Debug.Log("�s�p������");
            //��Βl�̌v�Z
            var lengthAB = Mathf.Pow(lineAB.x, 2) + Mathf.Pow(lineAB.y, 2) + Mathf.Pow(lineAB.z, 2);
            var lengthBC = Mathf.Pow(lineBC.x, 2) + Mathf.Pow(lineBC.y, 2) + Mathf.Pow(lineBC.z, 2);

            var dot = Vector3.Dot(lineAB, lineBC);
            var Calculation1 = (1 / ((lengthAB * lengthBC) - Mathf.Pow(dot, 2)));
            var Calculation2 = (((lengthAB * lengthBC) - (lengthBC * dot)) * lineAB) + (((lengthAB * lengthBC) - (lengthAB * dot)) * lineBC);
            var Calculation3 = (Calculation2 * Calculation1) * 0.5f;

            _answer = PointB + Calculation3;
        }
        else
        {
            Debug.Log("�݊p������");
            if (dotABBC > 0) _answer = (PointA + PointC) / 2;
            if (dotBCCA > 0) _answer = (PointA + PointB) / 2;
            if (dotCAAB > 0) _answer = (PointB + PointC) / 2;
        }
        //�����̐��l��\��
        _answerTextX.text = _answer.x.ToString();
        _answerTextY.text = _answer.y.ToString();
        _answerTextZ.text = _answer.z.ToString();

        //�Z���^�[�ɐԂ��ۂ̃I�u�W�F�N�g��\��
        var obj = Instantiate(_centerPrefab, _answer, Quaternion.identity);
        if (_agoCircle)
        {
            Destroy(_agoCircle);
        }
        _agoCircle = obj;
    }

    public void ResetPosition()
    {
        _pointA = _defaultA;
        _pointB = _defaultB;
        _pointC = _defaultC;
    }
}