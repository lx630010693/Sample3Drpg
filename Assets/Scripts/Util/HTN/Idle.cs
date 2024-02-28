using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : PrimitiveTask
{
    private float time = 0;
    protected override bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        return true;
    }
    protected override bool MetCondition_OnRun()
    {
        return true;
    }
    public override E_HTNStatus Operator()
    {
        if (time == 0)
        {
            time = Time.time;   
        }
        if (Time.time - time >= 2)
        {
            Debug.Log("休息了一次");
            time = 0;
            return E_HTNStatus.Success;
        }
        Debug.Log("休息中");
        return E_HTNStatus.Running;
    }
    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        int tpTired = (int)worldState["tired"];
        tpTired -= 10;
        worldState["tired"] = tpTired;
    }
    protected override void Effect_OnRun()
    {
        int tpTired = HTNWorld.GetWorldState<int>("tired");
        tpTired -= 10;
        HTNWorld.UpdateState("tired", tpTired);
    }


}

public partial class HTNPlanBuilder
{
    public HTNPlanBuilder Idle()
    {
        var task = new Idle();
        AddTask(task);
        return this;
    }
}