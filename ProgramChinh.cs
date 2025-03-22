using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagementSystem
{
    public class Course
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }

        public Course(string courseId, string courseName, int credits)
        {
            CourseId = courseId;
            CourseName = courseName;
            Credits = credits;
        }

        public override string ToString()
        {
            return $"{CourseId} - {CourseName} ({Credits} credits)";
        }
    }

    public class Enrollment
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Enrollment(string studentId, string courseId, DateTime enrollmentDate)
        {
            StudentId = studentId;
            CourseId = courseId;
            EnrollmentDate = enrollmentDate;
        }

        public override string ToString()
        {
            return $"{StudentId} enrolled in {CourseId} on {EnrollmentDate.ToShortDateString()}";
        }
    }

    public class Department
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public Department(string departmentId, string departmentName)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
        }

        public override string ToString()
        {
            return $"{DepartmentId} - {DepartmentName}";
        }
    }

    public class Grade
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public double Score { get; set; }

        public Grade(string studentId, string courseId, double score)
        {
            StudentId = studentId;
            CourseId = courseId;
            Score = score;
        }

        public (string letterGrade, double gpa) GetGradeInfo()
        {
            (double minScore, string letterGrade, double gpa)[] gradeMapping = new (double, string, double)[]
            {
                (9.0, "A+", 4.0),
                (8.5, "A", 4.0),
                (8.0, "B+", 3.5),
                (7.0, "B", 3.0),
                (6.5, "C+", 2.5),
                (5.5, "C", 2.0),
                (5.0, "D+", 1.5),
                (0.0, "D", 1.0)
            };

            foreach ((double minScore, string letterGrade, double gpa) grade in gradeMapping)
            {
                if (Score >= grade.minScore)
                {
                    return (grade.letterGrade, grade.gpa);
                }
            }

            return ("D", 1.0); // Default case
        }

        public override string ToString()
        {
            (string letterGrade, double gpa) gradeInfo = GetGradeInfo();
            return $"{StudentId} - {CourseId}: {Score} ({gradeInfo.letterGrade}, GPA: {gradeInfo.gpa})";
        }
    }

    public class DataChangedEventArgs : EventArgs
    {
        public string Message { get; }

        public DataChangedEventArgs(string message)
        {
            Message = message;
        }
    }

    public interface IRepository<T>
    {
        event EventHandler<DataChangedEventArgs> DataChanged;

        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
    }

    public class Repository<T> : IRepository<T>
    {
        protected List<T> items = new List<T>();

        public event EventHandler<DataChangedEventArgs> DataChanged;

        protected virtual void OnDataChanged(string message)
        {
            DataChanged?.Invoke(this, new DataChangedEventArgs(message));
        }

        public void Add(T item)
        {
            items.Add(item);
            OnDataChanged($"Added {item}");
        }

        public void Remove(T item)
        {
            items.Remove(item);
            OnDataChanged($"Removed {item}");
        }

        public IEnumerable<T> GetAll()
        {
            return items;
        }
    }

    public class StudentRepository : Repository<Student>
    {
        // Additional methods specific to Student can be added here
    }

    public class CourseRepository : Repository<Course>
    {
        // Additional methods specific to Course can be added here
    }

    public class Student
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }

        public Student(string studentId, string studentName)
        {
            StudentId = studentId;
            StudentName = studentName;
        }

        public override string ToString()
        {
            return $"{StudentId} - {StudentName}";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            // Tạo các repository
            StudentRepository studentRepo = new StudentRepository();
            CourseRepository courseRepo = new CourseRepository();

            // Đăng ký sự kiện
            studentRepo.DataChanged += (sender, e) => Console.WriteLine($"Student Repository: {e.Message}");
            courseRepo.DataChanged += (sender, e) => Console.WriteLine($"Course Repository: {e.Message}");

            // Thêm sinh viên
            Student student1 = new Student("S001", "Nguyễn Văn A");
            Student student2 = new Student("S002", "Trần Thị B");
            studentRepo.Add(student1);
            studentRepo.Add(student2);

            // Thêm khóa học
            Course course1 = new Course("C001", "Lập trình OOP", 3);
            Course course2 = new Course("C002", "Cơ sở dữ liệu", 4);
            courseRepo.Add(course1);
            courseRepo.Add(course2);

            // In danh sách sinh viên
            Console.WriteLine("\nDanh sách sinh viên:");
            foreach (Student student in studentRepo.GetAll())
            {
                Console.WriteLine(student);
            }

            // In danh sách khóa học
            Console.WriteLine("\nDanh sách khóa học:");
            foreach (Course course in courseRepo.GetAll())
            {
                Console.WriteLine(course);
            }

            // Xóa một sinh viên
            Console.WriteLine("\nXóa sinh viên Nguyễn Văn A:");
            studentRepo.Remove(student1);

            // In lại danh sách sinh viên
            Console.WriteLine("\nDanh sách sinh viên sau khi xóa:");
            foreach (Student student in studentRepo.GetAll())
            {
                Console.WriteLine(student);
            }
        }
    }
}
