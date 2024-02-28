using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPos : PrimitiveTask
{
    protected TestHTN obj;
    protected Transform pos;
    
    public GotoPos(TestHTN obj, Transform pos)
    {
        this.obj = obj;
        this.pos = pos;
        
    }
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

        Vector3 temp = (pos.position - obj.transform.position).normalized;
        obj.transform.Translate(temp * obj.speed*Time.deltaTime);
        if (Vector3.Distance(obj.transform.position, pos.position) <= 0.1f)
        {
            Debug.Log("到达目的地"+pos.gameObject.name);
            return E_HTNStatus.Success;
        }
        else
        {
            HTNWorld.UpdateState("isArrive", false);
        }
        Debug.Log("前往目的地" + pos.gameObject.name);
        return E_HTNStatus.Running;
    }
    protected override void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        worldState["isArrive"] = true;
    }
    protected override void Effect_OnRun()
    {
        HTNWorld.UpdateState("isArrive", true);
    }


}

public partial class HTNPlanBuilder
{
    public HTNPlanBuilder GoToPos(TestHTN obj,Transform pos)
    {
        var task = new GotoPos(obj,pos);
        AddTask(task);
        return this;
    }
}
