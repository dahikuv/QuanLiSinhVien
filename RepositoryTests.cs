using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagementSystem;

namespace Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private Repository<Student> repository = new Repository<Student>();
        private List<string> eventMessages = new List<string>();

        [TestInitialize]
        public void Setup()
        {
            eventMessages.Clear();
            repository.DataChanged += (sender, args) => eventMessages.Add(args.Message);
        }

        [TestMethod]
        public void Add_ShouldAddItemAndRaiseEvent()
        {
            // Arrange
            Student student = new Student("S001", "John Doe");

            // Act
            repository.Add(student);

            // Assert
            Assert.AreEqual(1, eventMessages.Count);
            Assert.AreEqual("Added S001 - John Doe", eventMessages[0]);
            Assert.AreEqual(1, new List<Student>(repository.GetAll()).Count);
        }

        [TestMethod]
        public void Remove_ShouldRemoveItemAndRaiseEvent()
        {
            // Arrange
            Student student = new Student("S001", "John Doe");
            repository.Add(student);

            // Act
            repository.Remove(student);

            // Assert
            Assert.AreEqual(2, eventMessages.Count);
            Assert.AreEqual("Removed S001 - John Doe", eventMessages[1]);
            Assert.AreEqual(0, new List<Student>(repository.GetAll()).Count);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllItems()
        {
            // Arrange
            Student student1 = new Student("S001", "John Doe");
            Student student2 = new Student("S002", "Jane Smith");
            repository.Add(student1);
            repository.Add(student2);

            // Act
            IEnumerable<Student> students = repository.GetAll();
            List<Student> studentList = new List<Student>(students);

            // Assert
            Assert.AreEqual(2, studentList.Count);
            Assert.IsTrue(studentList.Contains(student1));
            Assert.IsTrue(studentList.Contains(student2));
        }
    }
}



