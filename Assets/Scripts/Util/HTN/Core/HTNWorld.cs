using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����״ֻ̬��һ�����ɣ����ǽ�����Ϊ��̬��
public static class HTNWorld
{
    //�� ����״̬���ֵ�
    private static readonly Dictionary<string, Func<object>> get_WorldState;
    //д ����״̬���ֵ�
    private static readonly Dictionary<string, Action<object>> set_WorldState;

    static HTNWorld()
    {
        get_WorldState = new Dictionary<string, Func<object>>();
        set_WorldState = new Dictionary<string, Action<object>>();
    }
    //���һ��״̬����Ҫ����״̬������ȡ������д�뺯��
    public static void AddState(string key, Func<object> getter, Action<object> setter)
    {
        get_WorldState[key] = getter;
        set_WorldState[key] = setter;
    }
    //����״̬���Ƴ�ĳ������״̬
    public static void RemoveState(string key)
    {
        get_WorldState.Remove(key);
        set_WorldState.Remove(key);
    }
    //�޸�ĳ��״̬��ֵ
    public static void UpdateState(string key, object value)
    {
        //����ͨ��д���ֵ��޸ĵ�
        set_WorldState[key].Invoke(value);
    }
    //��ȡĳ��״̬��ֵ�����÷��ͣ����Խ���ȡ��objectתΪָ��������
    public static T GetWorldState<T>(string key)
    {
        return (T)get_WorldState[key].Invoke();
    }
    //����һ�ݵ�ǰ����״̬��ֵ�������Ҫ�����ڹ滮�У�
    public static Dictionary<string, object> CopyWorldState()
    {
        var copy = new Dictionary<string, object>();
        foreach (var state in get_WorldState)
        {
            copy.Add(state.Key, state.Value.Invoke());
        }
        return copy;
    }
}