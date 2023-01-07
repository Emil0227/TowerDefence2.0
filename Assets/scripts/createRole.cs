using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

class RoleInfo
{
    public GameObject Obj;
    public float StartTime;
    public int RoleCount;
    public float Interval;
    public float Speed;
    public int Life;
}

public class createRole : MonoBehaviour
{
    public GameObject ShowWave;
    public ArrayList RoleList;
    public ArrayList TurretList = null;
    public ArrayList TurretBaseList = null;
    ArrayList roleInfoList;
    int batches = 0;
    int num = 0;

    void Awake()
    {
        RoleList = new ArrayList();
        TurretList = new ArrayList();
        TurretBaseList = new ArrayList();
    }

    void Start()
    {
        roleInfoList = new ArrayList();

        //read file <enemyConfig.txt>
        FileInfo fi = new FileInfo(Application.dataPath + "/myLevel" + singleClass.currentLevel + "/enemyConfig.txt");
        StreamReader sr = fi.OpenText();
        batches = int.Parse(sr.ReadLine());     
        for (int i = 0; i < batches; i++)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.Obj = Resources.Load<GameObject>(sr.ReadLine());
            roleInfo.StartTime = float.Parse(sr.ReadLine());
            roleInfo.RoleCount = int.Parse(sr.ReadLine());
            roleInfo.Interval = float.Parse(sr.ReadLine());
            roleInfo.Speed = float.Parse(sr.ReadLine());
            roleInfo.Life = int.Parse(sr.ReadLine());
            roleInfoList.Add(roleInfo);
        }
        sr.Close();
        RoleInfo temp = (RoleInfo)roleInfoList[num];
        StartCoroutine(Wait(temp.StartTime, temp.Interval, temp.RoleCount));
    }

    //generate enemies
    IEnumerator Wait(float StartTime, float Interval, int RoleCount)
    {
        yield return new WaitForSeconds(StartTime);
        ShowWave.GetComponent<showWave>().SetText("Wave "+(num + 1) + "/" + batches);//show current wave
        for (int j = 0; j < RoleCount; j++)
        {
            Create();
            yield return new WaitForSeconds(Interval); 
        }
        num += 1;
        if (num < batches)
        {
            RoleInfo temp = (RoleInfo)roleInfoList[num];
            StartCoroutine(Wait(temp.StartTime, temp.Interval, temp.RoleCount));
        }
    }
    void Create()
    {
        if (singleClass.currentLevel == 1)
        {
            RoleInfo temp1 = (RoleInfo)roleInfoList[num];
            GameObject obj1 = GameObject.Instantiate(temp1.Obj);
            obj1.GetComponent<role>().InitRole("path1", temp1.Speed, temp1.Life);
            RoleList.Add(obj1);
        }
        if (singleClass.currentLevel == 2)
        {
            RoleInfo temp1 = (RoleInfo)roleInfoList[num];
            GameObject obj1 = GameObject.Instantiate(temp1.Obj);
            obj1.GetComponent<role>().InitRole("path1", temp1.Speed, temp1.Life);
            RoleInfo temp2 = (RoleInfo)roleInfoList[num];
            GameObject obj2 = GameObject.Instantiate(temp2.Obj);
            obj2.GetComponent<role>().InitRole("path2", temp2.Speed, temp1.Life);
            RoleList.Add(obj1);
            RoleList.Add(obj2);
        }
    }

    //check if level is passed
    void CheckWin()
    {
        bool isCreateComplete = true;
        if (num < batches)
        {
            isCreateComplete = false;
        }
        if (isCreateComplete == true)
        {
            //level passed
            if (RoleList.Count == 0)
            {
                Camera.main.GetComponent<gameState>().GameState = 1;
                //set UI
                GameObject showLevelInfo = Camera.main.GetComponent<resLoad>().ShowLevelInfo;
                GameObject canvas = Camera.main.GetComponent<resLoad>().Canvas;
                showLevelInfo.SetActive(true);
                canvas.GetComponent<Animator>().SetBool("showInfo", true); 
                showLevelInfo.GetComponent<showLevelInfo>().SetTitle("You Win!");
                showLevelInfo.GetComponent<showLevelInfo>().SetButtonText("Next Level");
            }
        }
    }

    //reset level
    public void ResetLevel()
    {
        StopAllCoroutines();
        num = 0;
        //clear enemy list
        foreach (GameObject obj in RoleList)
        {
            Destroy(obj);
        }
        RoleList.Clear();
        //clear turret list
        foreach (GameObject obj in TurretList)
        {
            Destroy(obj);
        }
        TurretList.Clear();
        foreach (GameObject obj in TurretBaseList)
        {
            obj.GetComponent<turretBase>().IsHasTurret = null;
        }
        TurretBaseList.Clear();
        Start();
    }
    void Update()
    {
        if (Camera.main.GetComponent<gameState>().GameState != 1)
        {
            CheckWin();
        }
    }
}


