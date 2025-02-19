using NUnit.Framework;
using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using Moq;
using System.Collections.Generic;

namespace Tests
{
    public class Test1_MoveColumn_Right
    {
        Board b;
        List<Column> columns;
        Mock<Column> backlog;
        Mock<Column> progress;
        Mock<Column> done;
        List<Mock<Column>> Mockcolumns;

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
        }

        [Test]
        public void MoveColumnright_Valid()
        {
            //Arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 0;
            backlog.Setup(m => m.Update("Ordinal", ordinal + 1));
            progress.Setup(m => m.Update("Ordinal", ordinal));
            //Act
            b.MoveColumnRight(useremail, ordinal);
            //Assert
            Assert.AreSame(backlog.Object, columns[1], "Test failed. the backlog column should be moved to the right.");
            Assert.AreSame(progress.Object, columns[0], "Test failed. the in progress column should be moved to the left.");
            Assert.AreSame(done.Object, columns[2], "Test failed. the done column should stay in place.");
        }

        [Test]
        public void MoveColumnright_Not_Host()
        {
            //Arrange
            string useremail = "not_host@gmail.com";
            int ordinal = 0;
            backlog.Setup(m => m.Update("Ordinal", ordinal + 1));
            progress.Setup(m => m.Update("Ordinal", ordinal));

            //Assert
            Assert.Throws(typeof(Exception),delegate { b.MoveColumnRight(useremail, ordinal); }, "Test failed. only the host can move the column");
        }

        [Test]
        public void MoveColumnwright_Rightmost_Column()
        {
            //Arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 2;
            backlog.Setup(m => m.Update("Ordinal", ordinal + 1));
            progress.Setup(m => m.Update("Ordinal", ordinal));

            //Assert
            Assert.Throws(typeof(Exception), delegate { b.MoveColumnRight(useremail, ordinal); }, "Test failed. the rightmost column can't move right");
        }

        [TestCase(-1)]
        [TestCase(2)]
        [TestCase(20)]
        public void MoveColumnwright_OutofBound_Ordinal(int ordinal)
        {
            //Arrange
            string useremail = "waleed@gmail.com";
            backlog.Setup(m => m.Update("Ordinal", ordinal + 1));
            progress.Setup(m => m.Update("Ordinal", ordinal));

            //Assert
            Assert.Throws(typeof(Exception), delegate { b.MoveColumnRight(useremail, ordinal); }, "Test failed. should throw exception when the ordinal is out of bounds.");
        }
    }
}
