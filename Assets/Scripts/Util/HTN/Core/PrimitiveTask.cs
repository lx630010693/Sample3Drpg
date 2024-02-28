using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimitiveTask : IBaseTask
{
    //ԭ�����񲻿����ٷֽ�Ϊ����������AddNextTask��������ʵ��
    void IBaseTask.AddNextTask(IBaseTask nextTask)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// ִ��ǰ�ж������Ƿ����㣬����nullʱֱ���޸�HTNWorld
    /// </summary>
    /// <param name="worldState">����plan������״̬����</param>
    public bool MetCondition(Dictionary<string, object> worldState = null)
    {
        if (worldState == null)//ʵ������ʱ
        {
            return MetCondition_OnRun();
        }
        else//ģ��滮ʱ
        {
            if (MetCondition_OnPlan(worldState))
            {
                Effect_OnPlan(worldState);
                return true;
            }
            return false;
        }
    }
    protected virtual bool MetCondition_OnPlan(Dictionary<string, object> worldState)
    {
        return true;
    }
    protected virtual bool MetCondition_OnRun()
    {
        return true;
    }

    //����ľ��������߼�������������ʵ��
    public abstract E_HTNStatus Operator();

    /// <summary>
    /// ִ�гɹ����Ӱ�죬����nullʱֱ���޸�HTNWorld
    /// </summary>
    /// <param name="worldState">����plan������״̬����</param>
    public void Effect( )
    {
        Effect_OnRun();
    }
    protected virtual void Effect_OnPlan(Dictionary<string, object> worldState)
    {
        
    }
    protected virtual void Effect_OnRun()
    {
        
    }

   
}