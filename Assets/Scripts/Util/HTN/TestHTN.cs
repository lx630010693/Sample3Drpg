using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHTN : MonoBehaviour
{
    public int gold;
    public int life;
    public int medicine;
    public int tired;
    public bool isArrive;
    
    public float speed;
    public Transform[] pos;

    private HTNPlanBuilder hTN;

    private void Awake()
    {
        HTNWorld.AddState("gold", () => { return gold; }, (value) => { gold = (int)value; });
        HTNWorld.AddState("life", () => { return life; }, (value) => { life = (int)value; });
        HTNWorld.AddState("medicine", () => { return medicine; }, (value) => { medicine = (int)value; });
        HTNWorld.AddState("tired", () => { return tired; }, (value) => { tired = (int)value; });
        HTNWorld.AddState("isArrive", () => { return isArrive; }, (value) => { isArrive = (bool)value; });
        hTN = new HTNPlanBuilder();
    }
    private void Start()
    {
        hTN.CompoundTask()
            
            .Method(() => { return life <= 80; })
            .CompoundTask()//让我血量保持在80以上
               .Method(() => { return medicine >= 1; })
                .EatMedi()
               .Back()
               .Method(() => { return gold >= 20; })
                .BuyMedi()
                .EatMedi()
               .Back()
             .Back()
            .Back()

           .Method(() => { return gold < 20; })
             .CompoundTask()//保持钱在20以上
              .Method(() => { return gold < 20 && tired < 10; })
               .GoToPos(this, pos[0])
               .WorkHard()
              .Back()
              .Method(() => { return gold < 20 && tired < 30; })
               .GoToPos(this, pos[1])
               .WorkSimple()
              .Back()
             .Back()
          .Back()

          .Method(() => { return true; })
           .CompoundTask()
            .Method(() => { return true; })
             .Idle()

             .End();
        
         

    }
    private void Update()
    {
        hTN.RunPlan();
    }
}

