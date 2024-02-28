using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����������н����ö�٣�����п���һƪ��Ϊ���Ļ���Ҳ����ֱ������Ϊ����EStatus��
public enum E_HTNStatus
{
    Failure, Success, Running,
}
public interface IBaseTask
{
    //�Ƿ���������
    bool MetCondition(Dictionary<string, object> worldState);
    //���������
    void AddNextTask(IBaseTask nextTask);
}
