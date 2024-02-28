using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HTNPlanBuilder
{
    private HTNPlanner planner;
    private HTNPlanRunner runner;
    private readonly Stack<IBaseTask> taskStack;

    public HTNPlanBuilder()
    {
        taskStack = new Stack<IBaseTask>();
    }

    private void AddTask(IBaseTask task)
    {
        if (planner != null)//��ǰ�ƻ�����Ϊ��
        {
            //����������Ϊ����ջ��Ԫ�ص�������
            taskStack.Peek().AddNextTask(task);
        }
        else //����ƻ���Ϊ�գ���ζ���������Ǹ����񣬽��г�ʼ��
        {
            planner = new HTNPlanner(task as CompoundTask);
            runner = new HTNPlanRunner(planner);
        }
        //�����������ԭ�����񣬾Ͳ���Ҫ��ջ�ˣ���Ϊԭ�����񲻻���������
        if (task is not PrimitiveTask)
        {
            taskStack.Push(task);
        }
    }
    //ʣ�µĴ��붼�ܼ򵥣���������ֱ�ӿ��ö�
    public void RunPlan()
    {
        runner.RunPlan();
    }
    public HTNPlanBuilder Back()
    {
        taskStack.Pop();
        return this;
    }
    public HTNPlanner End()
    {
        taskStack.Clear();
        return planner;
    }
    public HTNPlanBuilder CompoundTask()
    {
        var task = new CompoundTask();
        AddTask(task);
        return this;
    }
    public HTNPlanBuilder Method(System.Func<bool> condition)
    {
        var task = new Method(condition);
        AddTask(task);
        return this;
    }
}