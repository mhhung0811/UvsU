using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapLaserController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _list_lasers = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(ActiveTrap());
    }
    IEnumerator ActiveTrap()
    {
        while(true)
        {
            foreach(GameObject laser in _list_lasers)
            {
                laser.GetComponent<Laser>()?.EnableLaser();
            }
            yield return new WaitForSeconds(5f);
        }

    }

}
