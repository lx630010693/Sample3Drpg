using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTNPlanner
{
    //最终分解完成的所有原子任务存放的列表
    public Stack<PrimitiveTask> FinalTasks { get; private set; }
    //分解过程中，用来缓存被分解出的任务的栈，因为类型各异，故用IBaseTask类型
    private readonly Stack<IBaseTask> taskOfProcess;
    private readonly CompoundTask rootTask;//根任务

    public HTNPlanner(CompoundTask rootTask)
    {
        this.rootTask = rootTask;
        taskOfProcess = new Stack<IBaseTask>();
        FinalTasks = new Stack<PrimitiveTask>();
    }
    //规划（核心）
    public void Plan()
    {
        //先复制一份世界状态
        var worldState = HTNWorld.CopyWorldState();
        //将存储列表清空，避免上次计划结果的影响
        FinalTasks.Clear();
        //将根任务压进栈中，准备分解
        taskOfProcess.Push(rootTask);
        //只要栈还没空，就继续分解
        while (taskOfProcess.Count > 0)
        {
            //拿出栈顶的元素
            var task = taskOfProcess.Pop();
            //如果这个元素是复合任务
            if (task is CompoundTask cTask)
            {
                //判断是否可以执行
                if (cTask.MetCondition(worldState))
                {
                    /*如果可以执行，就肯定有可用的方法，
                    就将该方法的子任务都压入栈中，以便继续分解*/
                    if(cTask.ValidMethod is Method)
                    {
                        var subTask = cTask.ValidMethod.SubTask;
                        foreach (var t in subTask)
                        {
                            taskOfProcess.Push(t);
                        }
                    }
                   
                    
                    /*通过上面的步骤我们知道，能被压进栈中的只有
                    复合任务和原子任务，方法本身并不会入栈*/
                }
            }
            else //否则，这个元素就是原子任务
            {
                //将该元素转为原子任务，因为原本是IBaseTask类型
                var pTask = task as PrimitiveTask;
                /*在HTN结构中，原子任务只会依附任务存在，
                如果一个原子任务已经在taskOfProcess中，
                那就说明，它所在的Method是满足的，
                而Method能够满足的一大前提就是——所有子任务条件都满足，
                因此加入到taskOfProcess的原子任务，条件都是满足的。
                所以，这里可以直接让它对复制的世界状态产生影响（模拟其真实发生）*/
                //再将该原子任务加入存放分解完成的任务列表
                FinalTasks.Push(pTask);
            }
        }
    }
}