using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundTask : IBaseTask
{
    //ѡ�еķ���
    public Method ValidMethod { get; private set; }
    //�����񣨷������б�
    private readonly List<Method> methods;

    public CompoundTask()
    {
        methods = new List<Method>();
    }

    public void AddNextTask(IBaseTask nextTask)
    {
        //Ҫ�ж���ӽ������ǲ��Ƿ����࣬�ǵĻ������
        if (nextTask is Method m)
        {
            methods.Add(m);
        }
    }

    public bool MetCondition(Dictionary<string, object> worldState)
    {
        for (int i = 0; i < methods.Count; ++i)
        {
            //ֻҪ��һ����������ǰ�������Ϳ���
            if (methods[i].MetCondition(worldState))
            {
                //��¼���������ķ���
                ValidMethod = methods[i];
                return true;
            }
        }
        return false;
    }
}