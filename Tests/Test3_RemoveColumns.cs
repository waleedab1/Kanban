using NUnit.Framework;
using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using Moq;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;

namespace Tests
{

    class Test3_RemoveColumns
    {
        Board b;
        List<Column> columns;
        Mock<Column> backlog;
        Mock<Column> progress;
        Mock<Column> done;
        List<Mock<Column>> Mockcolumns;
        List<Task> tasks;
        Mock<Task> task1;
        Mock<Task> task2;
        List<Mock<Task>> MockTasks;

        [SetUp]
        public void Setup()
        {
            string host = "waleed@gmail.com";
            columns = new List<Column>();
            Mockcolumns = new List<Mock<Column>>();
            backlog = new Mock<Column>();
            progress = new Mock<Column>();
            done = new Mock<Column>();
            b = new Board(host, columns);
            tasks = new List<Task>();
            MockTasks = new List<Mock<Task>>();
            task1 = new Mock<Task>();
            task2 = new Mock<Task>();

            Prepare();
        }
        private void Prepare()
        {
            backlog.Object.name = "backlog";
            progress.Object.name = "in progress";
            done.Object.name = "done";

            backlog.Object.columnOrdinal = 0;
            progress.Object.columnOrdinal = 1;
            done.Object.columnOrdinal = 2;

            backlog.Object.limit = 100;
            progress.Object.limit = 100;
            done.Object.limit = 100;

            columns.Add(backlog.Object);
            columns.Add(progress.Object);
            columns.Add(done.Object);

            Mockcolumns.Add(backlog);
            Mockcolumns.Add(progress);
            Mockcolumns.Add(done);

            tasks.Add(task1.Object);
            tasks.Add(task2.Object);

            MockTasks.Add(task1);
            MockTasks.Add(task2);

            columns[0].tasks = tasks;
            columns[1].tasks = new List<Task>();
            columns[2].tasks = new List<Task>();
        }
        
        [Test]
        public void RemoveColumn_Leftmost_Column_Valid()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 0;
            task1.Setup(m => m.Update("ColumnName", "in progress"));
            task2.Setup(m => m.Update("ColumnName", "in progress"));
            //act
            b.RemoveColumn(useremail, ordinal);
            //assert
            Assert.AreEqual("in progress", columns[0].name, "Test failed. backlog column should be deleted and in progress takes it's place.");
            Assert.AreEqual(2, columns[0].tasks.Count, "Test failed. the tasks in backlog should have been moved to in progress.");
            Assert.AreEqual(2, columns.Count, "Test failed. 2 columns should remain.");
        }

        [Test]
        public void RemoveColumn_rightmost_Column_Valid()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 2;
            Mock<Task> task_end1 = new Mock<Task>();
            done.Object.tasks.Add(task_end1.Object);
            task_end1.Setup(m => m.Update("ColumnName", "in progress"));
            //act
            b.RemoveColumn(useremail, ordinal);
            //assert
            Assert.AreEqual(2, columns.Count, "Test failed. 2 columns should remain.");
            Assert.AreEqual(1, columns[1].tasks.Count, "Test failed. the tasks in done should have been moved to in progress.");
            Assert.AreSame(task_end1.Object , columns[1].tasks[0], "Test failed. in progress column should contain the task that was in done.");
        }

        [Test]
        public void RemoveColumn_Not_Host()
        {
            //arrange
            string useremail = "Not_Host@gmail.com";
            int ordinal = 0;
            task1.Setup(m => m.Update("ColumnName", "in progress"));
            task2.Setup(m => m.Update("ColumnName", "in progress"));
            //assert
            Assert.Throws(typeof(Exception), delegate { b.RemoveColumn(useremail, ordinal); }, "Test failed. only host can remove column.");
        }

        [TestCase(-1)]
        [TestCase(3)]
        [TestCase(100)]
        public void RemoveColumn_Out_Of_Bound_Ordinal(int ordinal)
        {
            //arrange
            string useremail = "waleed@gmail.com";
            //assert
            Assert.Throws(typeof(Exception), delegate { b.RemoveColumn(useremail, ordinal); }, "Test failed. should fail when ordinal is illegal.");
        }

        [Test]
        public void RemoveColumn_Minimum_Columns()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 0;
            task1.Setup(m => m.Update("ColumnName", "in progress"));
            task2.Setup(m => m.Update("ColumnName", "in progress"));
            b.RemoveColumn(useremail, 0);
            task1.Setup(m => m.Update("ColumnName", "done"));
            task2.Setup(m => m.Update("ColumnName", "done"));
            //assert
            Assert.Throws(typeof(Exception), delegate { b.RemoveColumn(useremail, ordinal); }, "Test failed. there should be a minimum of 2 columns.");
        }

        [Test]
        public void RemoveColumn_Leftmost_And_Right_Column_is_Full()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 0;
            progress.Object.limit = 1;
            task1.Setup(m => m.Update("ColumnName", "in progress"));
            task2.Setup(m => m.Update("ColumnName", "in progress"));

            //assert
            Assert.Throws(typeof(Exception), delegate { b.RemoveColumn(useremail, ordinal); }, "Test failed. remove a column only if there is enough space in the next column.");
        }

        [Test]
        public void RemoveColumn_And_Left_Column_is_Full()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 2;
            Mock<Task> task_end1 = new Mock<Task>();
            done.Object.tasks.Add(task_end1.Object);
            task_end1.Setup(m => m.Update("ColumnName", "in progress"));
            progress.Object.limit = 0;

            //assert
            Assert.Throws(typeof(Exception), delegate { b.RemoveColumn(useremail, ordinal); }, "Test failed. remove a column only if there is enough space in the next column.");
        }
    }
}
