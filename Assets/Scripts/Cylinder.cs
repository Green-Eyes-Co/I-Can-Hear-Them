using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
