using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSimple : PrimitiveTask
{
    private float time;
    protected override bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        if ((int)worldState["tired"] < 30 && (bool)worldState["isArrive"])
        {
            return true;
        }
        return false;
    }
    protected override bool MetCondition_OnRun()
    {
        if (HTNWorld.GetWorldState<int>("tired") < 30 && HTNWorld.GetWorldState<bool>("isArrive"))
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
            Debug.Log("打完简单的工");
            time = 0;
            return E_HTNStatus.Success;
        }
        Debug.Log("正在打简单的工");
        return E_HTNStatus.Running;
    }
    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        int tpTired = (int)worldState["tired"];
        tpTired += 5;
        worldState["tired"] = tpTired;

        int tpGold = (int)worldState["gold"];
        tpGold += 5;
        worldState["gold"] = tpGold;
    }
    protected override void Effect_OnRun()
    {
        int tpTired = HTNWorld.GetWorldState<int>("tired");
        tpTired += 5;
        HTNWorld.UpdateState("tired", tpTired);

        int tpGold = HTNWorld.GetWorldState<int>("gold");
        tpGold += 5;
        HTNWorld.UpdateState("gold", tpGold);
    }
}


public partial class HTNPlanBuilder
{
    public HTNPlanBuilder WorkSimple()
    {
        var task = new WorkSimple();
        AddTask(task);
        return this;
    }
}