using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTNPlanner
{
    //���շֽ���ɵ�����ԭ�������ŵ��б�
    public Stack<PrimitiveTask> FinalTasks { get; private set; }
    //�ֽ�����У��������汻�ֽ���������ջ����Ϊ���͸��죬����IBaseTask����
    private readonly Stack<IBaseTask> taskOfProcess;
    private readonly CompoundTask rootTask;//������

    public HTNPlanner(CompoundTask rootTask)
    {
        this.rootTask = rootTask;
        taskOfProcess = new Stack<IBaseTask>();
        FinalTasks = new Stack<PrimitiveTask>();
    }
    //�滮�����ģ�
    public void Plan()
    {
        //�ȸ���һ������״̬
        var worldState = HTNWorld.CopyWorldState();
        //���洢�б���գ������ϴμƻ������Ӱ��
        FinalTasks.Clear();
        //��������ѹ��ջ�У�׼���ֽ�
        taskOfProcess.Push(rootTask);
        //ֻҪջ��û�գ��ͼ����ֽ�
        while (taskOfProcess.Count > 0)
        {
            //�ó�ջ����Ԫ��
            var task = taskOfProcess.Pop();
            //������Ԫ���Ǹ�������
            if (task is CompoundTask cTask)
            {
                //�ж��Ƿ����ִ��
                if (cTask.MetCondition(worldState))
                {
                    /*�������ִ�У��Ϳ϶��п��õķ�����
                    �ͽ��÷�����������ѹ��ջ�У��Ա�����ֽ�*/
                    if(cTask.ValidMethod is Method)
                    {
                        var subTask = cTask.ValidMethod.SubTask;
                        foreach (var t in subTask)
                        {
                            taskOfProcess.Push(t);
                        }
                    }
                   
                    
                    /*ͨ������Ĳ�������֪�����ܱ�ѹ��ջ�е�ֻ��
                    ���������ԭ�����񣬷�������������ջ*/
                }
            }
            else //�������Ԫ�ؾ���ԭ������
            {
                //����Ԫ��תΪԭ��������Ϊԭ����IBaseTask����
                var pTask = task as PrimitiveTask;
                /*��HTN�ṹ�У�ԭ������ֻ������������ڣ�
                ���һ��ԭ�������Ѿ���taskOfProcess�У�
                �Ǿ�˵���������ڵ�Method������ģ�
                ��Method�ܹ������һ��ǰ����ǡ����������������������㣬
                ��˼��뵽taskOfProcess��ԭ������������������ġ�
                ���ԣ��������ֱ�������Ը��Ƶ�����״̬����Ӱ�죨ģ������ʵ������*/
                //�ٽ���ԭ����������ŷֽ���ɵ������б�
                FinalTasks.Push(pTask);
            }
        }
    }
}