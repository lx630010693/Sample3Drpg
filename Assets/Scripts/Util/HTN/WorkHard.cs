using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkHard : PrimitiveTask
{
    private float time;
    protected override bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        if ((int)worldState["tired"] < 10 && (bool)worldState["isArrive"])
        {
            return true;
        }
        return false;
    }
    protected override bool MetCondition_OnRun()
    {
        if (HTNWorld.GetWorldState<int>("tired") < 10 && HTNWorld.GetWorldState<bool>("isArrive"))
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
            Debug.Log("打完困难的工");
            time = 0;
            return E_HTNStatus.Success;
        }
        Debug.Log("正在打困难的工");
        return E_HTNStatus.Running;
    }
    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        int tpTired = (int)worldState["tired"];
        tpTired += 10;
        worldState["tired"] = tpTired;

        int tpGold = (int)worldState["gold"];
        tpGold += 10;
        worldState["gold"] = tpGold;


    }
    protected override void Effect_OnRun()
    {
        int tpTired = HTNWorld.GetWorldState<int>("tired");
        tpTired += 10;
        HTNWorld.UpdateState("tired", tpTired);

        int tpGold = HTNWorld.GetWorldState<int>("gold");
        tpGold += 10;
        HTNWorld.UpdateState("gold", tpGold);
    }
}


public partial class HTNPlanBuilder
{
    public HTNPlanBuilder WorkHard()
    {
        var task = new WorkHard();
        AddTask(task);
        return this;
    }
}