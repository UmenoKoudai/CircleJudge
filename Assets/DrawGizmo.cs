using UnityEngine;
using UnityEngine.UI;

public class DrawGizmo : MonoBehaviour
{
    [SerializeField]
    private Transform _pointA;
    [SerializeField]
    private Text _A;
    [SerializeField]
    private Transform _pointB;
    [SerializeField]
    private Text _B;
    [SerializeField]
    private Transform _pointC;
    [SerializeField]
    private Text _C;

    public void OnDrawGizmos()
    {
        //LineAB
        Gizmos.DrawLine(_pointA.position, _pointB.position);
        //LineBC
        Gizmos.DrawLine(_pointB.position, _pointC.position);
        //LineCA
        Gizmos.DrawLine(_pointC.position, _pointA.position);
        _A.text = $"({_pointA.position.x},{_pointA.position.y})";
        _B.text = $"({_pointB.position.x},{_pointB.position.y})";
        _C.text = $"({_pointC.position.x},{_pointC.position.y})";
    }
}
