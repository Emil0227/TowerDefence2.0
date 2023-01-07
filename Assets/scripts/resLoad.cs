using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class resLoad : MonoBehaviour
{
    public GameObject ParticleHit = null;
    public GameObject ParticleEliminate = null;
    public GameObject Bullet1 = null;
    public GameObject Bullet2 = null;
    public GameObject[] Turret = null;
    public GameObject[] Button = null;
    public GameObject ButtonUpgrade = null;
    public GameObject ButtonDestory = null;
    public int TurretCount=0;
    public GameObject ShowGold;
    public GameObject ShowLevelInfo;
    public GameObject Canvas;

    void Awake()
    {
        //read file <turretConfig.txt>
        FileInfo fi = new FileInfo(Application.dataPath + "/myLevel" + singleClass.currentLevel + "/turretConfig.txt");
        StreamReader sr = fi.OpenText();
        TurretCount = int.Parse(sr.ReadLine());
        Turret = new GameObject[TurretCount];
        Button = new GameObject[TurretCount];  
        for (int j = 0; j < TurretCount; j++)
        {
            string turretName = sr.ReadLine();
            Turret[j] = Resources.Load<GameObject>(turretName);
            Button[j] = Resources.Load<GameObject>("UI/"+ turretName+"Button");
            int price = int.Parse(sr.ReadLine());
            int rotateSpeed = int.Parse(sr.ReadLine());
            float fireRate = float.Parse(sr.ReadLine());
            float attackRange = float.Parse(sr.ReadLine());
            float attackRangeUpgrade = float.Parse(sr.ReadLine());
            int bulletSpeed = int.Parse(sr.ReadLine());
            int bulletDamage = int.Parse(sr.ReadLine());
            if (Turret[j].gameObject.name == "turret1")
            {
                turret1 t = Turret[j].GetComponent<turret1>();
                t.SetPrice(price);
                t.RotateSpeed = rotateSpeed;
                t.SetFireRate(fireRate);
                t.SetAttackRange(attackRange);
                t.SetAttackRangeUpgrade(attackRangeUpgrade);
                t.SetBulletSpeed(bulletSpeed);
                t.SetBulletDamage(bulletDamage);
            }
            else if (Turret[j].gameObject.name == "turret2")
            {
                turret2 t = Turret[j].GetComponent<turret2>();
                t.SetPrice(price);
                t.RotateSpeed = rotateSpeed;
                t.SetFireRate(fireRate);
                t.SetAttackRange(attackRange);
                t.SetAttackRangeUpgrade(attackRangeUpgrade);
                t.SetBulletSpeed(bulletSpeed);
                t.SetBulletDamage(bulletDamage);
            }
        }
        sr.Close();

        ParticleHit = Resources.Load<GameObject>("hit");
        ParticleEliminate = Resources.Load<GameObject>("eliminate");
        Bullet1 = Resources.Load<GameObject>("bullet1");
        Bullet2 = Resources.Load<GameObject>("bullet2");
        ButtonUpgrade = Resources.Load<GameObject>("UI/ButtonUpgrade");
        ButtonDestory = Resources.Load<GameObject>("UI/ButtonDestory");
    }
}
