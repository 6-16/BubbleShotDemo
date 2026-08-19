using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    public Transform Transform => transform;
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
}
