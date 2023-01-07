using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject Aim = null;
    public float Speed = 0;
    public int Damage = 0;
    ArrayList roleList;
    resLoad rl;

    void Start()
    {
        roleList = Camera.main.GetComponent<createRole>().RoleList;
    }

    private void OnTriggerEnter(Collider other)
    {
        resLoad rl = Camera.main.GetComponent<resLoad>();

        //when bullet hits an enemy
        if (other.gameObject.tag == "enemy")
        {
            //set life bar change
            role r = other.gameObject.GetComponent<role>();
            r.SetLife(r.GetLife() - Damage);
            //when enemy dies
            if (r.GetLife() <= 0)
            {
                r.DisconnectTurret1();
                r.DisconnectTurret2();
                r.DisconnectBullet();  
                other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                roleList.Remove(other.gameObject);
                Destroy(this.gameObject);
                r.SetLife(0);
                //get gold reward
                rl.ShowGold.GetComponent<showGold>().SetGold(rl.ShowGold.GetComponent<showGold>().GetGold() + 15);
                rl.ShowGold.GetComponent<showGold>().BonusSFX();
                //effects and animations
                GameObject resParticleEliminate = Camera.main.GetComponent<resLoad>().ParticleEliminate;
                GameObject particleEliminate = GameObject.Instantiate(resParticleEliminate);
                particleEliminate.transform.position = transform.position;
                Animator anim = other.gameObject.GetComponent<Animator>();
                anim.SetBool("die", true);
                Destroy(other.gameObject, 1.0f);
                Destroy(particleEliminate, 1.7f);

            }
            //enemy only gets hurt
            else
            {
                Destroy(this.gameObject);
                //effects and animations
                GameObject resParticleHit = Camera.main.GetComponent<resLoad>().ParticleHit;
                GameObject particleHit = GameObject.Instantiate(resParticleHit);
                particleHit.transform.position = transform.position;
                Destroy(particleHit, 1.0f);
            }
        }
        //when bullet hits terrain
        else if (other.gameObject.tag == "terrain")
        {
            if (Aim != null)
            {
                Aim.GetComponent<role>().DestoryBullet(this);
            }
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        //destory bullet if beyond map edge
        if ((transform.position.y < 0) || (transform.position.y > 12) || (transform.position.x < 0) || (transform.position.x > 100) || (transform.position.z < 10) || (transform.position.z > 70))
        {
            if (Aim != null)
            {
                Aim.GetComponent<role>().DestoryBullet(this);
            }
            Destroy(this.gameObject);
        }
        //bullet movement
        if (gameObject.name == "bullet1(Clone)" || Aim == null)
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
        }
        else if (gameObject.name == "bullet2(Clone)")
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
            Quaternion q1 = transform.rotation;
            Vector3 destPosition = Aim.transform.position;
            destPosition.y += 1.5f;
            transform.LookAt(destPosition);
            Quaternion q2 = transform.rotation;
            transform.rotation = Quaternion.RotateTowards(q1, q2, Time.deltaTime * 180);
        }
    }
}

