using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }

        internal Column(BusinessLayer.BoardPackage.Column c)
        {
            this.Name = c.name;
            this.Limit = c.limit;
            List<Task> tsk = new List<Task>();
            for (int i = 0; i < c.tasks.Count; i++)
            {
                tsk.Add(new Task(c.tasks[i]));
            }
            this.Tasks = tsk;
        }
    }
}
