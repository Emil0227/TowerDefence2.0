﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class button : MonoBehaviour, IPointerClickHandler
{
    public string TextureName;
    public turretBase Tb;
    public bool IsDisable = false;
    GameObject turret = null;
    resLoad rl;
    AudioSource sfx;

    void Awake()
    {
        //set button texture
        Texture2D tex = Resources.Load<Texture2D>(TextureName);
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        gameObject.GetComponent<Button>().image.sprite = spr;
        sfx = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sfx.Play();
        resLoad rl = Camera.main.GetComponent<resLoad>();
        //do not respond to click when button is disabled
        if (IsDisable == true)
        {
            return;
        }
        //click button to create a turret
        if (gameObject.name == "turret1Button(Clone)")
        {
            turret = GameObject.Instantiate(rl.Turret[0]);
            turret.transform.position = Tb.transform.position;
            Tb.DestoryAllButton();
            Tb.IsHasTurret = turret;
            rl.ShowGold.GetComponent<showGold>().SetGold(rl.ShowGold.GetComponent<showGold>().GetGold() - turret.GetComponent<turret1>().GetPrice());
            Camera.main.GetComponent<createRole>().TurretList.Add(turret);
        }
        else if (gameObject.name == "turret2Button(Clone)")
        {
            turret = GameObject.Instantiate(rl.Turret[1]);
            turret.transform.position = Tb.transform.position;
            Tb.DestoryAllButton();
            Tb.IsHasTurret = turret;
            rl.ShowGold.GetComponent<showGold>().SetGold(rl.ShowGold.GetComponent<showGold>().GetGold() - turret.GetComponent<turret2>().GetPrice());
            Camera.main.GetComponent<createRole>().TurretList.Add(turret);
        }
        //click button to upgrade a turret
        else if (gameObject.name == "ButtonUpgrade(Clone)")
        {
            if (Tb.IsHasTurret.gameObject.name == "turret1(Clone)")
            {
                turret1 t = Tb.IsHasTurret.GetComponent<turret1>();
                if (t.CurrentRangeLevel < 4)
                {
                    t.Upgrade();
                    Tb.SetRange();
                    rl.ShowGold.GetComponent<showGold>().SetGold(rl.ShowGold.GetComponent<showGold>().GetGold() - 20);
                }
                if (t.CurrentRangeLevel == 4)
                {
                    Disable();
                }
                else if(rl.ShowGold.GetComponent<showGold>().GetGold() < 20)
                {
                    Disable();
                }
            }
            else if(Tb.IsHasTurret.gameObject.name == "turret2(Clone)")
            {
                turret2 t = Tb.IsHasTurret.GetComponent<turret2>();
                if (t.CurrentRangeLevel < 4)
                {
                    t.Upgrade();
                    Tb.SetRange();
                    rl.ShowGold.GetComponent<showGold>().SetGold(rl.ShowGold.GetComponent<showGold>().GetGold() - 20);
                }
                if (t.CurrentRangeLevel == 4)
                {
                    Disable();
                }
                else if (rl.ShowGold.GetComponent<showGold>().GetGold() < 20)
                {
                    Disable();
                }
            }
        }
        //click button to destory a turret
        else if (gameObject.name == "ButtonDestory(Clone)")
        {
            Destroy(Tb.IsHasTurret);
            Tb.IsHasTurret = null;
            Tb.DestoryAllButton();
        }
    }

    public void Disable()
    {
        IsDisable = true;
        TextureName += "Disable";
        Destroy(gameObject.GetComponent<Button>().GetComponent<Image>().sprite);
        Texture2D tex = Resources.Load<Texture2D>(TextureName);
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        gameObject.GetComponent<Button>().image.sprite = spr;
    }

}

