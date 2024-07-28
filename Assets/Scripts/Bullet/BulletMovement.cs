using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _direct;
    [SerializeField] private float _velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Moving();
    }
    private void Moving()
    {
        transform.Translate(new Vector3(_direct * 15f, 0f, 0f) * Time.deltaTime);
    }
    public void SetDirection(float value)
    {
        _direct = value;
        Vector3 new_scale = transform.localScale;
        new_scale.x *= _direct;
        transform.localScale = new_scale;
    }
    
}
