using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret2 : MonoBehaviour
{
    public GameObject Aim = null;
    public int CurrentRangeLevel = 1;
    public int RotateSpeed = 0;
    public int Price = 0;
    public int BulletDamage = 0;
    public float FireRate = 0;
    public float AttackRange = 0;
    public float AttackRangeUpgrade = 0;
    public float BulletSpeed = 0;
    int countBullet = 0;
    float distance = 0;
    bool isShooting = false;
    Transform transRotate = null;
    Transform transBarrel = null;
    GameObject resBullet2 = null;
    GameObject existedAim = null;
    ArrayList roleList;
    AudioSource shoot;

    void Start()
    {
        transRotate = transform.Find("base").Find("rotation");
        transBarrel = transform.Find("base").Find("rotation").Find("arm").Find("gunBody").Find("barrel");
        resBullet2 = Camera.main.GetComponent<resLoad>().Bullet2;
        roleList = Camera.main.GetComponent<createRole>().RoleList;
        shoot = gameObject.GetComponent<AudioSource>();
    }

    //set turret price
    public void SetPrice(int x)
    {
        Price = x;
    }
    public int GetPrice()
    {
        return Price;
    }

    //set turret fire rate
    public void SetFireRate(float f)
    {
        FireRate = f;
    }
    public float GetFireRate()
    {
        return FireRate;
    }

    //set turret attack range
    public void SetAttackRange(float r)
    {
       AttackRange = r;
    }
    public float GetAttackRange()
    {
        return AttackRange;
    }

    //set factor for attack range upgrade
    public void SetAttackRangeUpgrade(float f)
    {
        AttackRangeUpgrade = f;
    }
    public float GetAttackRangeUpgrade()
    {
        return AttackRangeUpgrade;
    }

    //update attack range
    public void Upgrade()
    {
        if (CurrentRangeLevel == 4)
        {
            return;
        }
        AttackRange *= AttackRangeUpgrade;
        CurrentRangeLevel += 1;
    }
    public int GetCurrentRangeLevel()
    {
        return CurrentRangeLevel;
    }

    //set bullet speed
    public void SetBulletSpeed(float f)
    {
        BulletSpeed = f;
    }
    public float GetBulletSpeed()
    {
        return BulletSpeed;
    }

    //set bullet damage 
    public void SetBulletDamage(int d)
    {
        BulletDamage = d;
    }
    public int GetBulletDamage()
    {
        return BulletDamage;
    }

    void StartShoot()
    {
        if (isShooting == false)
        {
            InvokeRepeating("CreateBullet", 1.0f, FireRate);
            isShooting = true;
        }
    }
    void StopShoot()
    {
        if (isShooting == true)
        {
            CancelInvoke("CreateBullet");
            isShooting = false;
        }
    }

    //Randomly release 3 bullets tracking target enemy
    void CreateBullet()
    {
        InvokeRepeating("CreateScattershot", 0.0f, 0.1f);
    }
    void CreateScattershot()
    {
        shoot.Play();
        GameObject bullet2 = GameObject.Instantiate(resBullet2);
        if (Aim != null)
        {
            bullet2.GetComponent<bullet>().Aim = this.Aim;
            Aim.GetComponent<role>().AddBullet(bullet2.GetComponent<bullet>());
        }
        bullet2.GetComponent<bullet>().Speed = BulletSpeed;
        bullet2.GetComponent<bullet>().Damage = BulletDamage;
        bullet2.transform.position = transBarrel.position;
        bullet2.transform.eulerAngles = new Vector3(transBarrel.transform.eulerAngles.x, transBarrel.transform.eulerAngles.y + Random.Range(-15, 15), transBarrel.transform.eulerAngles.z);
        countBullet++;
        if (countBullet == 3)
        {
            countBullet = 0;
            CancelInvoke("CreateScattershot");
        }
    }

    void Update()
    {
        //find closest target
        Aim = null;
        distance = 0.0f;
        foreach (GameObject r in roleList)
        {
            float d = Vector3.Distance(r.transform.position, transform.position);
            if (Aim == null)
            {
                Aim = r;
                distance = d;
            }
            else
            {
                if (distance > d)
                {
                    Aim = r;
                    distance = d;
                }
            }
        }
        //aim at target and attack
        if (Aim != null && distance > 0 && distance < AttackRange)
        {
            //put turret2 in target's turret list
            if (Aim != existedAim)
            {
                existedAim = Aim;
                existedAim.GetComponent<role>().AddTurret2(this);
            }
            float currentAngleY = transRotate.eulerAngles.y;
            float currentAngleX = transRotate.eulerAngles.x;
            transRotate.LookAt(Aim.transform);
            float destAngleY = transRotate.eulerAngles.y;
            float destAngleX = transRotate.eulerAngles.x;
            float angleY = Mathf.MoveTowardsAngle(currentAngleY, destAngleY, Time.deltaTime * RotateSpeed);
            float angleX = Mathf.MoveTowardsAngle(currentAngleX, destAngleX, Time.deltaTime * RotateSpeed);
            transRotate.eulerAngles = new Vector3(angleX, angleY, 0);
            StartShoot();
        }
        else
        {
            StopShoot();
        }
    }
}