using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHitCheck : MonoBehaviour {

    public GameObject cellTower;

	// Use this for initialization

    public void OnHit(Vector3 pos)
    {
        GameObject tower =  Instantiate(cellTower, this.transform.position, Quaternion.identity);
        
        if(tower.GetComponent<TowerRigid>() != null)
        {
            tower.GetComponent<TowerRigid>().CellFire(pos, 1000, 50);
        }

        this.gameObject.SetActive(false);
    }

	
}
