using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletImpart : MonoBehaviour
{
    private int _damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Gate>() != null)
        {
            return;
        }
        collision.gameObject.GetComponent<IDamageable>()?.ReceiveDamage(_damage);
        BulletSpawner.Instance.Despawn(transform);
    }
}
