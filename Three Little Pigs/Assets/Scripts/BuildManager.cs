using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildManager : MonoBehaviour
{
    public static BuildManager S;

    [Serializable]
    public struct TurretInfo
    {
        public string name;
        public int cost;
        public GameObject turretObject;
    }
    public TurretInfo[] turretArray;
    public Dictionary<string, TurretInfo> turrets;

    private GameObject building = null;
    private int currentCost = 0;

    private void Awake()
    {
        // Singleton Definition
        if (BuildManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       turrets = new Dictionary<string, TurretInfo>();
        foreach (TurretInfo t in turretArray)
        {
            turrets.Add(t.name, t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            building.transform.position = new Vector3(pos.x, pos.y, 0);

            if (Input.GetMouseButtonDown(0))
            {
                if (building.GetComponent<Turret>().BuildTurret())
                {
                    GameManager.S.SubtractMoney(currentCost);
                    building = null;
                }
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(building);
                building = null;
            }
        }
    }

    public void btn_BuildTurret(string name)
    {
        if (building) return;

        TurretInfo turretType = turrets[name];
        if (GameManager.S.money >= turretType.cost)
        {
            currentCost = turretType.cost;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building = Instantiate(turretType.turretObject, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }
}
