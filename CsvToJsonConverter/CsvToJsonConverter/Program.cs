using System.Text.Json.Serialization;
using System.Xml;
using CsvToJsonConverter.Errors;
using CsvToJsonConverter.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CsvToJsonConverter // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var s in args)
            {
                Console.WriteLine(s);

            }
            if (args.Length == 0)
                throw new ArgumentException("Please provide valid arguments: inputFile outputDirectory extension");
            var logList = new List<CustomError>();

            var inputData = args[0];
            var outputData = args[1];
            var extension = args[2];
            
            if (String.IsNullOrEmpty(inputData) || String.IsNullOrEmpty(outputData) || String.IsNullOrEmpty(extension))
                throw new ArgumentException("Seems one of the arguments is invalid");

            var fileInfo = new FileInfo(inputData);
            var dataList = new List<string>();

            using (var sr = new StreamReader(fileInfo.OpenRead()))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    dataList.Add(line);
                }
            }

            var studentHashSet = new HashSet<Student>(new StudentComparator());
            
            foreach (var s in dataList)
            {
                string[] columns = s.Split(",");
                if (!columns.Any(cell => string.IsNullOrEmpty(cell)))
                {
                    var student = new Student()
                    {
                        FirstName = columns[0],
                        LastName = columns[1],
                        Course = columns[2],
                        StudyMode = columns[3],
                        IndexNumber = columns[4],
                        BirthDate = DateTime.Parse(columns[5]),
                        Email = columns[6],
                        FathersName = columns[7],
                        MothersName = columns[8]
                    };
                    if (!studentHashSet.Contains(student))
                    {
                        studentHashSet.Add(student);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(JsonSerializer.Serialize(student));
                    }
                    else
                    {
                        logList.Add(
                            new CustomError()
                            {
                                type= ErrorType.ERROR,
                                reason = "Failed to add student because a duplicate was found",
                                data = JsonConvert.SerializeObject(student)
                            }
                            );
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed to add student because a duplicate was found");
                    }
                    Console.ForegroundColor = ConsoleColor.White;

                    College college = new College("PJWSTK", studentHashSet);

                    switch (extension)
                    {
                        case "json":
                            using (var sw = new StreamWriter(outputData + Path.GetFileNameWithoutExtension(fileInfo.Name) + ".json"))
                            {
                                var json = JsonConvert.SerializeObject(college, Formatting.Indented);
                                
                                sw.WriteLine(json);
                            }
                            break;
                    }
                }
                
            }
            
            using (var sw = new StreamWriter("log.txt"))
            {
                foreach (var log in logList)
                {
                    sw.WriteLine(log);
                }
            }
        }
    }
}
