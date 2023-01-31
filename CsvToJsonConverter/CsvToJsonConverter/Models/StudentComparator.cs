namespace CsvToJsonConverter.Models;

public class StudentComparator : IEqualityComparer<Student>
{
        public bool Equals(Student s1, Student s2)
        {
                return StringComparer.InvariantCultureIgnoreCase.Equals($"{s1.IndexNumber} {s1.Email}",
                        $"{s2.IndexNumber} {s2.Email}");
        }

        public int GetHashCode(Student s)
        {
                return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{s.IndexNumber} {s.Email}");
        }
}