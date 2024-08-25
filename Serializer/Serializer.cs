using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BookingApp.Serializer
{
    class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public bool ToCsv(string fileName, List<T> objects)
        {
            try
            {
                StringBuilder csv = new StringBuilder();

                // append header
                csv.AppendLine(new T().GetHeader());

                // append objects
                foreach (T obj in objects)
                {
                    
                    string line = string.Join(Delimiter.ToString(), obj.ToCsv());
                    csv.AppendLine(line);
                }

                if (!File.Exists(fileName)) { File.Create(fileName).Close(); }

                File.WriteAllText(fileName, csv.ToString());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            

            return true;
        }

        public List<T> FromCsv(string fileName)
        {
            List<T> objects = new List<T>();

            try
            {
                var lines = File.ReadLines(fileName).ToList();
                if (lines.Count == 0) { return objects; } // empty csv
                lines.RemoveAt(0);

                foreach (string line in lines)
                {
                    string[] csvValues = line.Split(Delimiter);
                    T obj = new T();

                    try
                    {
                        obj.FromCsv(csvValues);
                        objects.Add(obj);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine($"FromCsv fail, line: {line}");
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message);  }
            return objects;
        }
    }
}