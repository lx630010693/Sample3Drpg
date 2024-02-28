using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTNPlanRunner
{
    //��ǰ����״̬
    private E_HTNStatus curState;
    //ֱ�ӽ��滮�������������������¹滮
    private readonly HTNPlanner planner;
    //��ǰִ�е�ԭ������
    private PrimitiveTask curTask;
    //��ǡ�ԭ�������б��Ƿ���Ԫ�ء��ܹ�������
    private bool canContinue;

    public HTNPlanRunner(HTNPlanner planner)
    {
        this.planner = planner;
        curState = E_HTNStatus.Failure;
    }

    public void RunPlan()
    {
        //�����ǰ����״̬��ʧ�ܣ�һ��ʼĬ��ʧ�ܣ�
        if (curState == E_HTNStatus.Failure)
        {
            //�͹滮һ��
            planner.Plan();
        }
        //�����ǰ����״̬�ǳɹ����ͱ�ʾ��ǰ���������
        if (curState == E_HTNStatus.Success)
        {
            //�õ�ǰԭ���������Ӱ��
            curTask.Effect();
        }
        /*�����ǰ״̬���ǡ�����ִ�С�����ȡ����һ��ԭ��������Ϊ��ǰ����
        ����ʧ�ܻ��ǳɹ�����Ҫ��ô������Ϊ�����ʧ�ܣ��϶��ڴ������е���
        ֮ǰ���Ѿ�������һ�ι滮����Ӧ��ȡ�¹滮�������������У��������
        Ϊ�ɹ�����ҲҪȡ��������������*/
        if (curState != E_HTNStatus.Running)
        {
            //��TryPop�ķ��ؽ���жϹ滮����FinalTasks�Ƿ�Ϊ��
            canContinue = planner.FinalTasks.TryPop(out curTask);
        }
        /*���canContinueΪfalse����curTask��ΪnullҲ����ʧ�ܣ���ʵӦ���ǡ�ȫ��
        ��ɡ�����ȫ����ɺ�ʧ����һ���ģ���Ҫ���¹滮��������ֻ�е�canContinue && curTask.MetCondition()������ʱ���Ŷ�ȡ��ǰԭ�����������״̬�������ʧ�ܡ�*/
        curState = canContinue && curTask.MetCondition() ? curTask.Operator() : E_HTNStatus.Failure;
    }
}