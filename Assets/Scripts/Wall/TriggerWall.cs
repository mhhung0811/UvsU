using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TriggerWall : MonoBehaviour
{
    // Danh sách các wall sẽ được nhận trigger
    [SerializeField] private List<GameObject> _walls;
    
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Has trigger checkpoint");
        if(collision.gameObject.layer == 7 || collision.GetComponent<PlayerHP>())
        {
            gameObject.SetActive(false);
            foreach(GameObject wall in _walls)
            {
                wall.SetActive(!wall.activeSelf);
            }
        }
    }

}
