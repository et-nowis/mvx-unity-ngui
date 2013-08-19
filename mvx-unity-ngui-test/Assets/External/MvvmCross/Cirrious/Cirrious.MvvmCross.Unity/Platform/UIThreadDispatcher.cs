// UIThreadDispatcher.cs
//
// Copyright (c) 2013 Frenzoo Ltd.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using UnityEngine;

namespace Cirrious.MvvmCross.Unity.Platform
{
    public class UIThreadDispatcher : MvxSingleton<UIThreadDispatcher>
    {
        static UIThreadDispatcher()
        {
            // create new singleton - base class will store this as instance
            new UIThreadDispatcher();
        }

        public static Thread MainThread;

        public class DelegateItem
        {
            public int Frame = 0;

            public readonly Action Item;

            public string CallStack { get; private set; }

            public DelegateItem(Action item)
            {
                this.Item = item;
                this.CallStack = GetCallStack();
            }

            private string GetCallStack()
            {
                string callStack = "Generated from: ";
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                if (trace != null)
                {
                    foreach (System.Diagnostics.StackFrame frame in trace.GetFrames())
                    {
                        System.Reflection.MethodBase method = frame.GetMethod();
                        if (method == null) continue;
                        if ("GetCallStack".Equals(method.Name)) continue;
                        if ("EnqueueOnMainThread".Equals(method.Name)) continue;
                        if ("ExecuteOnMainThread".Equals(method.Name)) continue;
                        string methodTypeName = method.ReflectedType != null ? method.ReflectedType.Name : string.Empty;
                        if ("DelegateItem".Equals(methodTypeName)) continue;

                        callStack += methodTypeName + "::" + method.Name + "(";
                        System.Reflection.ParameterInfo[] paramInfos = method.GetParameters();
                        string[] paramNames = new string[paramInfos.Length];
                        for (int i = 0; i < paramInfos.Length; ++i)
                        {
                            paramNames[i] = paramInfos[i].ParameterType.Name;
                        }
                        callStack += string.Join(",", paramNames) + ")\r\n";
                    }
                }
                return callStack;
            }
        }

        public class TaskFailedEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
        }

        public class TaskItem
        {
            public event EventHandler Completed;
            public event EventHandler<TaskFailedEventArgs> Failed;

            public bool Started = false;
            public int Frame = 0;
            public float WaitSeconds = 0f;

            public int RunCount = 0;
            public string LastCurrent = "";

            public readonly IEnumerator Task;

            public readonly string CallStack;

            public TaskItem(IEnumerator task)
            {
                this.Task = task;
                this.CallStack = GetCallStack();
            }

            private string GetCallStack()
            {
                string callStack = "Generated from: ";
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                if (trace != null)
                {
                    foreach (System.Diagnostics.StackFrame frame in trace.GetFrames())
                    {
                        System.Reflection.MethodBase method = frame.GetMethod();
                        if (method == null) continue;
                        if ("GetCallStack".Equals(method.Name)) continue;
                        if ("EnqueueOnMainThread".Equals(method.Name)) continue;
                        if ("ExecuteOnMainThread".Equals(method.Name)) continue;
                        string methodTypeName = method.ReflectedType != null ? method.ReflectedType.Name : string.Empty;
                        if ("TaskItem".Equals(methodTypeName)) continue;

                        callStack += methodTypeName + "::" + method.Name + "(";
                        System.Reflection.ParameterInfo[] paramInfos = method.GetParameters();
                        string[] paramNames = new string[paramInfos.Length];
                        for (int i = 0; i < paramInfos.Length; ++i)
                        {
                            paramNames[i] = paramInfos[i].ParameterType.Name;
                        }
                        callStack += string.Join(",", paramNames) + ")\r\n";
                    }
                }
                return callStack;
            }

            public void RaiseCompleted()
            {
                var handler = Completed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }

            public void RaiseFailed(Exception exception)
            {
                var handler = Failed;
                if (handler != null)
                {
                    handler(this, new TaskFailedEventArgs { Exception = exception });
                }
            }
        }

        public class DispatchFailedEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
        }

        private class DispatchQueue
        {
            public event EventHandler<DispatchFailedEventArgs> DispatchFailed;

            public LinkedList<DelegateItem> ThreadQueue { get; private set; }
            public LinkedList<TaskItem> TaskQueue { get; private set; }
            public LinkedList<TaskItem> WaitQueue { get; private set; }

            public int ActiveCount { get { return TaskQueue.Count + WaitQueue.Count; } }

            public DispatchQueue()
            {
                ThreadQueue = new LinkedList<DelegateItem>();
                TaskQueue = new LinkedList<TaskItem>();
                WaitQueue = new LinkedList<TaskItem>();
            }

            public void Enqueue(DelegateItem item)
            {
                lock (this)
                {
                    ThreadQueue.AddLast(item);
                }
            }

            public void Enqueue(TaskItem item)
            {
                lock (this)
                {
                    TaskQueue.AddLast(item);
                }
            }

            public void Remove(TaskItem item)
            {
                lock (this)
                {
                    TaskQueue.Remove(item);
                    WaitQueue.Remove(item);
                }
            }

            public void Remove(IEnumerable<TaskItem> items)
            {
                lock (this)
                {
                    foreach (var item in items)
                    {
                        TaskQueue.Remove(item);
                        WaitQueue.Remove(item);
                    }
                }
            }

            public bool Run()
            {
                bool hasRunSomething = false;
                if (ThreadQueue.Count > 0)
                {
                    lock (this)
                    {
                        if (ThreadQueue.Count > 0)
                        {
                            var delegateItem = ThreadQueue.First.Value;
                            try
                            {
                                if (delegateItem.Frame == UIThreadDispatcher.Instance.CurrentFrame)
                                {
                                    // do nothing
                                }
                                else
                                {
                                    delegateItem.Frame = UIThreadDispatcher.Instance.CurrentFrame;
                                    ThreadQueue.RemoveFirst();
                                    delegateItem.Item();
                                    hasRunSomething = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                RaiseDispatchFailed(ex);
                                hasRunSomething = true;
                            }
                        }
                    }
                }

                if (TaskQueue.Count > 0)
                {
                    lock (this)
                    {
                        if (TaskQueue.Count > 0)
                        {
                            var taskItem = TaskQueue.First.Value;
                            try
                            {
                                taskItem.Started = true;
                                taskItem.Frame = UIThreadDispatcher.Instance.CurrentFrame;
                                TaskQueue.RemoveFirst();
                                if (taskItem.Task.MoveNext())
                                {
                                    hasRunSomething = true;
                                    ++taskItem.RunCount;
                                    taskItem.LastCurrent = taskItem.Task.Current.ToString();
                                    if (taskItem.Task.Current is WaitForEndOfFrame)
                                    {
                                        WaitQueue.AddLast(taskItem);
                                    }
                                    else if (taskItem.Task.Current is WWW)
                                    {
                                        WaitQueue.AddLast(taskItem);
                                    }
                                    else if (taskItem.Task.Current is ContinueAfterSeconds)
                                    {
                                        ContinueAfterSeconds wait = (ContinueAfterSeconds)taskItem.Task.Current;
                                        taskItem.WaitSeconds = wait.Seconds;
                                        WaitQueue.AddLast(taskItem);
                                    }
                                    else
                                    {
                                        TaskQueue.AddLast(taskItem);
                                    }
                                }
                                else
                                {
                                    taskItem.RaiseCompleted();
                                }
                            }
                            catch (Exception ex)
                            {
                                hasRunSomething = true;
                                RaiseDispatchFailed(ex);
                                taskItem.RaiseFailed(ex);
                            }
                        }
                    }
                }

                return hasRunSomething;
            }

            public void ResetStatus()
            {
                ResetTaskStatus();
                ResetWaitStatus();
            }

            public void Dump()
            {
                if (ActiveCount > 0)
                {
                    lock (this)
                    {
                        int count = 0;
                        foreach (TaskItem taskItem in TaskQueue)
                        {
                            Mvx.Trace("Task {0}: {1}", ++count, taskItem.CallStack);
                        }
                        foreach (TaskItem taskItem in WaitQueue)
                        {
                            Mvx.Trace("Task {0}: {1}", ++count, taskItem.CallStack);
                        }
                    }
                }
            }

            private void ResetTaskStatus()
            {
                if (TaskQueue.Count > 0)
                {
                    lock (this)
                    {
                        foreach (var ti in TaskQueue)
                        {
                            if (ti.RunCount > 1)
                            {
                                Mvx.Error("Task run {0} times" +
                                    "\nTask Stack: {1}" +
                                    "\nTask type: {2}",
                                    ti.RunCount,
                                    ti.CallStack,
                                    ti.LastCurrent);
                            }
                            ti.RunCount = 0;
                        }
                    }
                }
            }

            private void ResetWaitStatus()
            {
                if (WaitQueue.Count > 0)
                {
                    lock (this)
                    {
                        var rewaitList = new LinkedList<TaskItem>();
                        while (WaitQueue.Count > 0)
                        {
                            var waitingTaskItem = WaitQueue.First.Value;

                            waitingTaskItem.RunCount = 0;
                            if (!(waitingTaskItem.Task.Current is ContinueAfterSeconds))
                            {
                                TaskQueue.AddLast(waitingTaskItem);
                            }
                            else
                            {
                                // we keep it in the wait queue until time
                                float remaining = waitingTaskItem.WaitSeconds;
                                remaining -= UIThreadDispatcher.Instance.ElapsedSecondsSinceLastFrame;
                                if (remaining <= 0f)
                                {
                                    TaskQueue.AddLast(waitingTaskItem);
                                }
                                else
                                {
                                    waitingTaskItem.WaitSeconds = remaining;
                                    rewaitList.AddLast(waitingTaskItem);
                                }
                            }
                            WaitQueue.RemoveFirst();
                        }
                        while (rewaitList.Count > 0)
                        {
                            WaitQueue.AddLast(rewaitList.First.Value);
                            rewaitList.RemoveFirst();
                        }
                    }
                }
            }

            private void RaiseDispatchFailed(Exception ex)
            {
                var handler = DispatchFailed;
                if (handler != null)
                {
                    handler(this, new DispatchFailedEventArgs { Exception = ex });
                }
            }
        }

        public bool UseDispatchThreshold { get; set; }
        public float DispatchThreshold { get; set; }
        public int CurrentFrame { get; set; }
        public float ElapsedSecondsSinceLastFrame { get; set; }

        public event Action<Exception> ExceptionCatchedWhileRun;

        private DispatchQueue[] dispatchQueueList = new DispatchQueue[] {
            new DispatchQueue(),
            new DispatchQueue()
        };

        private UIThreadDispatcher()
        {
            UseDispatchThreshold = true;
            DispatchThreshold = 0.05f;

            foreach (var dispatchQueue in dispatchQueueList)
            {
                dispatchQueue.DispatchFailed += RaiseExceptionCatchedWhileRun;
            }
        }

        #region Enqueue

        public DelegateItem EnqueueOnMainThread(Action myDelegate)
        {
            return EnqueueOnMainThread(new DelegateItem(myDelegate));
        }

        public DelegateItem EnqueueOnMainThread(DelegateItem item)
        {
            dispatchQueueList[0].Enqueue(item);
            return item;
        }

        public TaskItem EnqueueOnMainThread(IEnumerable enumerable)
        {
            return EnqueueOnMainThread(enumerable.GetEnumerator());
        }

        public TaskItem EnqueueOnMainThread(IEnumerator enumerator)
        {
            return EnqueueOnMainThread(new TaskItem(enumerator));
        }

        public TaskItem EnqueueOnMainThread(TaskItem item)
        {
            dispatchQueueList[0].Enqueue(item);
            return item;
        }

        #endregion

        #region Execute

        public DelegateItem ExecuteOnMainThread(Action myDelegate)
        {
            return ExecuteOnMainThread(new DelegateItem(myDelegate));
        }

        public DelegateItem ExecuteOnMainThread(DelegateItem item)
        {
            dispatchQueueList[1].Enqueue(item);
            return item;
        }

        public TaskItem ExecuteOnMainThread(IEnumerable enumerable)
        {
            return ExecuteOnMainThread(enumerable.GetEnumerator());
        }

        public TaskItem ExecuteOnMainThread(IEnumerator enumerator)
        {
            return ExecuteOnMainThread(new TaskItem(enumerator));
        }

        public TaskItem ExecuteOnMainThread(TaskItem item)
        {
            dispatchQueueList[1].Enqueue(item);
            return item;
        }

        #endregion

        #region Remove

        public void RemoveTaskFromMainThread(TaskItem ti)
        {
            dispatchQueueList[1].Remove(ti);
            dispatchQueueList[0].Remove(ti);
        }

        public void RemoveTasksFromMainThread(IEnumerable<TaskItem> tasks)
        {
            dispatchQueueList[1].Remove(tasks);
            dispatchQueueList[0].Remove(tasks);
        }

        #endregion

        public int GetActiveCount()
        {
            int count = 0;
            foreach (var dispatchQueue in dispatchQueueList)
            {
                count += dispatchQueue.ActiveCount;
            }
            return count;
        }

        public int GetMainQueueActiveCount()
        {
            return dispatchQueueList[0].ActiveCount;
        }

        public int GetSuperQueueActiveCount()
        {
            return dispatchQueueList[1].ActiveCount;
        }

        public void DumpActiveTaskStack()
        {
            for (int i = dispatchQueueList.Length - 1; i >= 0; --i)
            {
                Mvx.Trace("Dispatch Queue {0}", i);
                dispatchQueueList[i].Dump();
            }
        }

        public bool Run()
        {
            DateTime startTime = DateTime.Now;
            bool isTimeUp = false;
            bool hasRunSomething = false;
            for (int i = dispatchQueueList.Length - 1; !isTimeUp && i >= 0; --i)
            {
                do
                {
                    hasRunSomething = dispatchQueueList[i].Run();
                    isTimeUp = UseDispatchThreshold && (DateTime.Now - startTime).TotalSeconds > DispatchThreshold;
                } while (hasRunSomething && !isTimeUp);
            }
            for (int i = dispatchQueueList.Length - 1; i >= 0; --i)
            {
                dispatchQueueList[i].ResetStatus();
            }

            return hasRunSomething;
        }

        private void RaiseExceptionCatchedWhileRun(object sender, DispatchFailedEventArgs e)
        {
            Exception exception = e.Exception;
            Stack<Exception> exceptionStack = new Stack<Exception>();
            exceptionStack.Push(exception);
            while (exception != null)
            {
                exceptionStack.Push(exception);
                exception = exception.InnerException;
            }
            while (exceptionStack.Count > 0)
            {
                exception = exceptionStack.Pop();
                Mvx.Error("{0}\n{1}", exception.Message, exception.StackTrace);
            }
            var handler = ExceptionCatchedWhileRun;
            if (handler != null)
            {
                handler(e.Exception);
            }
        }

    }
}
