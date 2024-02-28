using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Method : IBaseTask
{
    //�������б������Ǹ�������Ҳ������ԭ������
    public List<IBaseTask> SubTask { get; private set; }
    //������ǰ������
    private readonly Func<bool> condition;

    public Method(Func<bool> condition)
    {
        SubTask = new List<IBaseTask>();
        this.condition = condition;
    }
    //��������������ж�=��������ǰ����������+������������������
    //��������������ж�=��������ǰ����������+������������������
    public bool MetCondition(Dictionary<string, object> worldState = null)
    {
        /*
        �ٸ���һ������״̬������׷��ÿ���������Effect�������ж��������
        ֻҪ����һ������������������������������������֮ǰ���������EffectҲ������
        �����tpWorld��¼������֤�˷�������������������������������������ٸ��ƻ�worldState
        */
        var tpWorld = new Dictionary<string, object>(worldState);
        if (condition())//���������ǰ�������Ƿ�����
        {
            for (int i = 0; i < SubTask.Count; ++i)
            {
                //һ����һ������������������㣬��������Ͳ�������
                if (!SubTask[i].MetCondition(tpWorld))
                {
                    return false;
                }
            }
            //���������������ٽ���Effect���µ�������״̬��tpWorld����worldState
            worldState = tpWorld;
            return true;//���������ȫ�������ˣ��Ǿͳ��ˣ�
        }
        return false;
    }
    //���������
    public void AddNextTask(IBaseTask nextTask)
    {
        SubTask.Add(nextTask);
    }
}