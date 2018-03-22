using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : MonoBehaviour {
    ParticleSystem explosionParticle;
    bool isPlay;
    float playTime;
    private void Awake()
    {
        explosionParticle = this.GetComponent<ParticleSystem>();
        explosionParticle.Pause();
        isPlay = false;
    }

    // Update is called once per frame
    void Update ()
    {
        //if (!explosionParticle.IsAlive())
        //    GameObject.Destroy(this.gameObject);

        if(isPlay)
        {
            playTime += Time.deltaTime;
        }
        if (playTime > 4.0f)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Boss")
        {
            if(explosionParticle != null && !isPlay)
            {
                explosionParticle.Play();
                isPlay = true; 
            }
        }
    }

}
