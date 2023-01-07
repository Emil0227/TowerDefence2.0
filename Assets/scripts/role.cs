using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class role : MonoBehaviour
{
    ArrayList turretList1;
    ArrayList turretList2;
    ArrayList bulletList;
    GameObject path = null;
    Transform nextTrans = null;
    Vector3 vFullLife;
    float speed = 0;
    int initialLife = 0;
    int currentLife = 0;

    void Start()
    {
        turretList1 = new ArrayList();
        turretList2 = new ArrayList();
        bulletList = new ArrayList();
        vFullLife = gameObject.transform.Find("lifeBar").localScale;
    }

    //add turret1 targeting at the enemy
    public void AddTurret1(turret1 t)
    {
        turretList1.Add(t);
    }

    //add turret2 targeting at the enemy
    public void AddTurret2(turret2 t)
    {
        turretList2.Add(t);
    }

    //add bullet targeting at the enemy
    public void AddBullet(bullet b)
    {
        bulletList.Add(b);
    }

    //delete turret1 from the list
    public void DestoryTurret1(turret1 t)
    {
        turretList1.Remove(t);
    }

    //delete turret2 from the list
    public void DestoryTurret2(turret2 t)
    {
        turretList2.Remove(t);
    }

    //delete bullet from the list
    public void DestoryBullet(bullet b)
    {
        bulletList.Remove(b);
    }

    //disconnect all turret1 targeting at the enemy
    public void DisconnectTurret1()
    {
        foreach (turret1 t in turretList1)
        {
            t.Aim = null;
        }
    }

    //disconnect all turret2 targeting at the enemy
    public void DisconnectTurret2()
    {
        foreach (turret2 t in turretList2)
        {
            t.Aim = null;
        }
    }

    //disconnect all bullets targeting at the enemy
    public void DisconnectBullet()
    {
        foreach (bullet b in bulletList)
        {
            b.Aim = null;
        }
    }

    //set life bar length
    public void SetLife(int life)
    {
        currentLife = life;
        float percentage = currentLife * 1.0f / initialLife;
        gameObject.transform.Find("lifeBar").localScale = new Vector3(vFullLife.x * percentage, vFullLife.y, vFullLife.z);
    }
    public int GetLife()
    {
        return currentLife;
    }

    //set enemy path, speed, life
    public void InitRole(string path, float speed, int blood)
    {
        initialLife = blood;
        currentLife = blood;
        this.path = GameObject.Find(path);
        this.speed = speed;
        transform.position = this.path.transform.position;
        nextTrans = this.path.transform.Find("path");
        transform.LookAt(nextTrans);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Update()
    {
        //enemy pathfollow
        if (nextTrans != null)
        {
            float distance = Vector3.Distance(transform.position, nextTrans.position);
            if (distance > Time.deltaTime * this.speed)
            {
                transform.position += transform.forward * Time.deltaTime * speed;
                float currentAngle = transform.eulerAngles.y;
                transform.LookAt(nextTrans);
                float destAngle = transform.eulerAngles.y;
                float angle = Mathf.MoveTowardsAngle(currentAngle, destAngle, Time.deltaTime * 400);
                transform.eulerAngles = new Vector3(0, angle, 0);
            }
            else
            {
                transform.position = nextTrans.position;
                nextTrans = nextTrans.Find("path");
                if (nextTrans == null)
                {
                    //set UI for game over
                    if (Camera.main.GetComponent<gameState>().GameState != 1)
                    {
                        Camera.main.GetComponent<gameState>().GameState = 1;
                        GameObject showLevelInfo = Camera.main.GetComponent<resLoad>().ShowLevelInfo;
                        GameObject canvas = Camera.main.GetComponent<resLoad>().Canvas;
                        showLevelInfo.SetActive(true);
                        canvas.GetComponent<Animator>().SetBool("showInfo", true); 
                        showLevelInfo.GetComponent<showLevelInfo>().SetTitle("Game Over");
                        showLevelInfo.GetComponent<showLevelInfo>().SetButtonText("Try Again");
                    }     
                }
            }
        }
    }
}


