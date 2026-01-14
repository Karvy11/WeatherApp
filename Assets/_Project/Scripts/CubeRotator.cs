using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] Vector3 _axis = Vector3.up;
    [SerializeField] float _speed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_axis * _speed * Time.deltaTime);

    }
}
