using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float _speed = -90f;
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, _speed * Time.deltaTime);
    }
}
