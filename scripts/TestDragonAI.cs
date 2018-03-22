using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDragonAI : MonoBehaviour {

    public MoveManager dragonFlame;
    public MoveManager dragonAttack;
    public GameObject flameParticle;
    public GameObject player;

    public float upDownSpeed;
    public float GlidSpeed;
    public float Distance;


    Animator anim;
    Transform dragon;

    public GameObject bigExplosionParticle;

    //코루틴 실행체크 함수
    bool corFly;
    bool corFlame;
    bool corAttack;
    bool corLanding;
    bool corUp;

	// Use this for initialization
	void Awake ()
    {
        anim = this.GetComponent<Animator>();
        dragon = this.transform;
        //flameParticle.SetActive(false);
        flameParticle.GetComponent<ParticleSystem>().Stop();
        //StartCoroutine("CorFly");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("CorDragonFlame");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine("CorDragonAttack");
        }
    }

    IEnumerator CorFly()
    {
        corFly = true;
        float moveDistance = 0.0f;
        anim.SetTrigger("Hover");
        while(moveDistance < Distance)
        {
            moveDistance += upDownSpeed * Time.deltaTime;
            dragon.position += Vector3.up * upDownSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        corFly = false;
        yield return null;
    }

    IEnumerator CorLanding()
    {
        corLanding = true;
        if (dragon.position.y < 0)
            yield return null;

        while(dragon.position.y > 0)
        {
            dragon.position += Vector3.down * Time.deltaTime * upDownSpeed;
            yield return new WaitForEndOfFrame();
        }
        anim.SetTrigger("Idle");

        corLanding = false;
        yield return null;
    }

    IEnumerator CorUp()
    {
        corUp = true;

        if (dragon.position.y > 0)
            yield return null;

        while (dragon.position.y < 0)
        {
            dragon.position += Vector3.up * Time.deltaTime * upDownSpeed;
            yield return new WaitForEndOfFrame();
        }
        anim.SetTrigger("Idle");

        corUp = false;
        yield return null;
    }

    IEnumerator CorDragonFlame()
    {
        corFlame = true;
        //날기 시작
        if (!corFly)
        {
            StartCoroutine("CorFly");
            yield return new WaitForEndOfFrame();
        }

        while (corFly)
        {
            yield return new WaitForEndOfFrame();
        }
        //날기 끝
        //Debug.Log("endFly");


        //flame움직임 시작
        dragonFlame.Reset();
        dragonFlame.MovePlay(dragon.position,dragon.rotation);
         Quaternion rot = dragon.rotation;

        while (!dragonFlame.GetisEnd())
        {
            dragon.position = dragonFlame.GetCurrentPos();
            dragon.rotation = rot * dragonFlame.GetCurrentRotation();
            yield return new WaitForEndOfFrame();
        }

        Vector3 flameDir = (player.transform.position - flameParticle.transform.position).normalized;
        flameParticle.transform.rotation =  Quaternion.LookRotation(flameDir, Vector3.up);
        flameParticle.GetComponent<ParticleSystem>().Play();


        float Flametime = 0.0f;
        while(Flametime < 3)
        {
            Flametime += Time.deltaTime;
            flameDir = (player.transform.position - flameParticle.transform.position).normalized;
            flameParticle.transform.rotation = Quaternion.LookRotation(flameDir, Vector3.up);
            yield return new WaitForEndOfFrame();
        }

        flameParticle.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(0.2f);

        //착륙 시작
        if (!corLanding)
        {
            StartCoroutine("CorLanding");
            yield return new WaitForEndOfFrame();
        }
        while (corLanding)
        {
            yield return new WaitForEndOfFrame();
        }
        //착륙 끝


        corFlame = false;
        yield return null;

    }

    IEnumerator CorDragonAttack()
    {
        corAttack = true;
        //날기 시작
        if (!corFly)
        {
            StartCoroutine("CorFly");
            yield return new WaitForEndOfFrame();
        }

        while (corFly)
        {
            yield return new WaitForEndOfFrame();
        }
        //날기 끝
        //dragonAttack 시작
        anim.SetTrigger("Glide");
        dragonAttack.Reset();
        dragonAttack.MovePlay(dragon.position,dragon.rotation);
        Quaternion rot = dragon.rotation;
        
        while (!dragonAttack.GetisEnd())
        {
            dragon.position = dragonAttack.GetCurrentPos();
            dragon.rotation = rot * dragonAttack.GetCurrentRotation();
            yield return new WaitForEndOfFrame();
        }

        anim.SetTrigger("Hover");
        //dragonAttack 끝
        float distance = Vector3.Distance(dragon.position,player.transform.position);
        Vector3 dragonDir = ((player.transform.position - new Vector3(0, 15, 0)) - dragon.position).normalized;

        //플레이어한테 돌격
        while (distance > 100)
        {
            dragonDir = ((player.transform.position - new Vector3(0,15,0)) - dragon.position).normalized;
            dragon.rotation = Quaternion.Lerp(dragon.rotation, Quaternion.LookRotation(dragonDir, dragon.up), 0.05f);
            dragon.position += dragonDir * Time.deltaTime * GlidSpeed;
            distance = Vector3.Distance(dragon.position, player.transform.position);
            yield return new WaitForEndOfFrame();

        }
        Time.timeScale = 0.1f;

        //Rigidbody
        //방향변환 없이 돌격
        bool ExplosionParticle = true;
        while (dragon.position.y > -50)
        {
            dragon.position += dragonDir * Time.deltaTime * GlidSpeed;

            if(dragon.position.y <0 && ExplosionParticle)
            {
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.deltaTime;
                Instantiate(bigExplosionParticle, dragon.position + dragon.forward * 20,Quaternion.identity);
                ExplosionParticle = false;
            }
            yield return new WaitForEndOfFrame();

        }

        Vector3 lookDir = player.transform.position - dragon.position;
        lookDir.y = 0;

        dragon.rotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        //anim.SetTrigger("Hover");
        //착륙 시작
        if (!corUp)
        {
            StartCoroutine("CorUp");
            yield return new WaitForEndOfFrame();
        }
        while (corUp)
        {
            yield return new WaitForEndOfFrame();
        }
        //착륙 끝

        corAttack = false;
        yield return null;
    }

}
