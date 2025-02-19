using NUnit.Framework;
using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using Moq;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;

namespace Tests
{
    class Test2_AddColumn
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

        [TestCase("col1")]
        [TestCase("colkjasd1")]
        [TestCase("co129l1")]
        [TestCase("1")]
        [TestCase("*&c21")]
        public void AddColumn_With_Valid_Parameters(string name)
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 0;
            //act
            b.AddColumn(useremail, ordinal, name);
            //assert
            Assert.AreEqual(4, columns.Count, "Test failed. the column should have been added successfully.");
            Assert.AreSame(backlog.Object, columns[1], "Test failed. the backlog column should have been moved right.");
            Assert.AreSame(progress.Object, columns[2], "Test failed. the in progress column should have been moved right.");
            Assert.AreSame(done.Object, columns[3], "Test failed. the done column should have been moved right.");
        }

        [Test]
        public void AddColumn_Not_Host()
        {
            //arrange
            string name = "Col1";
            string useremail = "Not_Host@gmail.com";
            int ordinal = 0;
            //assert
            Assert.Throws(typeof(Exception), delegate { b.AddColumn(useremail, ordinal, name); }, "Test failed. only the host can add a column.");
            Assert.AreEqual(3, columns.Count, "Test failed. a new column should not appear in the board.");
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(4)]
        [TestCase(100)]
        public void AddColumn_Out_Of_Bounds_Ordinal(int ordinal)
        {
            //arrange
            string useremail = "waleed@gmail.com";
            string name = "monkey";
            //assert
            Assert.Throws(typeof(Exception), delegate { b.AddColumn(useremail, ordinal, name); }, "Test failed. should throw an exception for out of bound ordinal.");
            Assert.AreEqual(3, columns.Count, "Test failed. Should not be able to add column with out of bounds ordinal.");
        }

        [Test]
        public void AddColumn_Existing_name()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            string name = "backlog";
            int ordinal = 1;

            //assert
            Assert.Throws(typeof(Exception), delegate { b.AddColumn(useremail, ordinal, name); }, "Test failed. can't add column with an existing name.");
            Assert.AreEqual(3, columns.Count, "Test failed. a column with the same name should not be added.");
        }

        [TestCase("BACKLOG")]
        [TestCase("IN PROGRESS")]
        [TestCase("DONE")]
        public void AddColumn_Existing_Name_But_In_Uppercase_Letters(string name)
        {
            //arrange
            string useremail = "waleed@gmail.com";
            int ordinal = 3;

            //act
            b.AddColumn(useremail, ordinal, name);

            //assert
            Assert.AreEqual(4, columns.Count, "Test failed. a column should have been added.");
            Assert.AreSame(backlog.Object, columns[0], "Test failed. backlog column should stay in its place.");
            Assert.AreSame(progress.Object, columns[1], "Test failed. in progress column should stay in its place.");
            Assert.AreSame(done.Object, columns[2], "Test failed. done column should stay in its place.");
        }

        [Test]
        public void AddColumn_Max_name_Valid()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            string name = "op,dmfhhhhhhhhh";
            int ordinal = 1;

            //act
            b.AddColumn(useremail, ordinal, name);

            //assert
            Assert.AreEqual(4, columns.Count, "Test failed. should be able to add a task with max length (15 char).");
            Assert.AreSame(backlog.Object, columns[0], "Test failed. backlog column should stay in its place.");
            Assert.AreSame(progress.Object, columns[2], "Test failed. in progress column should move left.");
            Assert.AreSame(done.Object, columns[3], "Test failed. done column should move left.");
        }

        [Test]
        public void AddColumn_Surpasses_Max_name()
        {
            //arrange
            string useremail = "waleed@gmail.com";
            string name = "namethatsurpasseslengththat is more than 15 chars";
            int ordinal = 0;
            //assert
            Assert.Throws(typeof(Exception), delegate { b.AddColumn(useremail, ordinal, name); }, "Test failed. can't add column with more than max name.");
            Assert.AreEqual(3, columns.Count, "Test failed. should not be able to add a task with more than max length (15 char).");
        }

    }
}
