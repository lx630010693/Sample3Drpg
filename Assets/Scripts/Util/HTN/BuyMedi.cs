using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMedi : PrimitiveTask
{
    
    private float time=0;

   

    protected override bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        if ((int)worldState["gold"] >= 20)
        {
            return true;
        }
        return false;
    }
    protected override bool MetCondition_OnRun()
    {
        if (HTNWorld.GetWorldState<int>("gold") >= 20)
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
            Debug.Log("买完药了");
            time = 0;
            return E_HTNStatus.Success;
        }
        Debug.Log("正在买药");
        return E_HTNStatus.Running;
    }
    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        int tpGold = (int)worldState["gold"];
        tpGold -= 20;
        worldState["gold"] = tpGold;

        int tpMedi = (int)worldState["medicine"];
        tpMedi += 1;
        worldState["medicine"] = tpMedi;
    }
    protected override void Effect_OnRun()
    {
        int tpGold = HTNWorld.GetWorldState<int>("gold");
        tpGold -= 20;
        HTNWorld.UpdateState("gold", tpGold);

        int tpMedi = (int)HTNWorld.GetWorldState<int>("medicine");
        tpMedi += 1;
        HTNWorld.UpdateState("medicine", tpMedi);
    }

}
public partial class HTNPlanBuilder
{
    public HTNPlanBuilder BuyMedi( )
    {
        var task = new BuyMedi();
        AddTask(task);
        return this;
    }
}
