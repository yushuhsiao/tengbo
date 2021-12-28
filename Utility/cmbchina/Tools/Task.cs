using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace System.Threading
{
    public delegate void TaskStartHandler(Task task);
    public sealed class Task
    {
        private static Queue<Task> pooling = new Queue<Task>();
        public static Task Start(TaskStartHandler start)
        {
            Task task;
            lock (pooling)
                if (pooling.Count > 0)
                    task = pooling.Dequeue();
                else
                    task = new Task();
            task.IsCanceled = false;
            task.IsSuccessed = false;
            ThreadPool.QueueUserWorkItem(task.start, start);
            return task;
        }

        public static Task operator +(Task a, TaskStartHandler h)
        {
            if (a == null)
            {
                if (h != null)
                    return Task.Start(h);
            }
            else
            {
                if (h == null)
                    a.IsCanceled = true;
            }
            return null;
        }

        public void Invoke(Control c, TaskStartHandler h)
        {
            if (c.InvokeRequired)
                c.Invoke(h, this);
            else
                h(this);
        }

        void start(object state)
        {
            TaskStartHandler taskStart = (TaskStartHandler)state;
            try
            {
                taskStart(this);
            }
            catch
            {
            }
            finally
            {
                lock (pooling)
                    pooling.Enqueue(this);
            }
        }

        private Task() { }

        object isCanceled;
        public bool IsCanceled
        {
            get { return Interlocked.CompareExchange(ref this.isCanceled, null, null) != null; }
            set { Interlocked.Exchange(ref this.isCanceled, value ? this : null); }
        }

        object isSuccessed;
        public bool IsSuccessed
        {
            get { return Interlocked.CompareExchange(ref this.isSuccessed, null, null) != null; }
            set { Interlocked.Exchange(ref this.isSuccessed, value ? this : null); }
        }
    }
}