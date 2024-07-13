using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{
    [SerializeField] private Vector3 _pointA = new Vector3(100, 100, 0);
    [SerializeField] private Vector3 _pointB = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _pointC = new Vector3(0, 200, 0);
    [SerializeField] private Text _answerText;

    public Vector3 PointA => _pointA;
    public Vector3 PointB => _pointB;
    public Vector3 PointC => _pointC;

    public void Calculation()
    {
        var lineAB = PointB - PointA;
        var lineBC = PointC - PointB;

        var lengthAB = Mathf.Pow(lineAB.x, 2) + Mathf.Pow(lineAB.y, 2) + Mathf.Pow(lineAB.z, 2);
        var lengthBC = Mathf.Pow(lineBC.x, 2) - Mathf.Pow(lineBC.y, 2) - Mathf.Pow(lineBC.z, 2);

        var dotABBC = Mathf.Pow(Vector3.Dot(lineAB, lineBC), 2);
        var Calculation1 = (1 / (lengthAB * lengthBC) - dotABBC) * 0.5f;
        var Calculation2 = (((lengthAB * lengthBC) - (lengthBC * dotABBC)) * lineAB) + (((lengthAB * lengthBC) - (lengthAB * dotABBC)) * lineBC);
        var Calculation3 = Calculation2 * Calculation1;

        var answer = PointB + Calculation3;
        _answerText.text = $"X:{string.Format("{0:#, 0.00}", answer.x)} Y:{string.Format("{0:#, 0.00}", answer.y)} Z:{string.Format("{0:#, 0.00}", answer.x)}";
    }

    public void Reset()
    {
        _pointA = new Vector3(100, 100, 0);
        _pointB = new Vector3(0, 0, 0);
        _pointC = new Vector3(0, 200, 0);
    }
}
