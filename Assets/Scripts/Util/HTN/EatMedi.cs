using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMedi : PrimitiveTask
{
   
    private float time=0;
   
    protected override bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        if ((int)worldState["life"] <= 80)
        {
            return true;
        }
        return false;
    }
    protected override bool MetCondition_OnRun()
    {
        if (HTNWorld.GetWorldState<int>("life") <= 80)
        {
            return true;
        }
        return false;
    }
    public override E_HTNStatus Operator()
    {
        if (time == 0)
        {
            time = Time.time;
        }
        if (Time.time - time >= 2)
        {
            Debug.Log("吃完药了");
            time = 0;
            return E_HTNStatus.Success;
        }
        Debug.Log("正在吃药");
        return E_HTNStatus.Running;
        
    }

    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        int tpLife = (int)worldState["life"];
        tpLife += 20;
        worldState["life"] = tpLife;
        
        int tpMedi = (int)worldState["medicine"];
        tpMedi -= 1;
        worldState["medicine"] = tpMedi;

    }
    protected override void Effect_OnRun()
    {
        int tpLife = HTNWorld.GetWorldState<int>("life");
        tpLife += 20;
        HTNWorld.UpdateState("life", tpLife);

        int tpMedi = HTNWorld.GetWorldState<int>("medicine");
        tpMedi -= 1;
        HTNWorld.UpdateState("medicine", tpMedi);
    }


}
public partial class HTNPlanBuilder
{
    public HTNPlanBuilder EatMedi()
    {
        var task = new EatMedi();
        AddTask(task);
        return this;
    }
}