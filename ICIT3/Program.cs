using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICIT3
{
    class Teachers
    {
        public Teachers() { }
        public Teachers(string FIO, string Institute, string VideoService)
        {
            this.FIO = FIO;
            this.Institute = Institute;
            this.VideoService = VideoService;
        }
        public string FIO = null;
        public string Institute = null;
        public string VideoService = null;
    }
    static class MyExtentionClass
    {
        public static string CutStr(this string str)
        {
            string[] fio = str.Split(" ");
            for (int i = 1; i < fio.Length; i++)
                fio[i] = fio[i].Remove(1, fio[i].Length - 1).Insert(1, ".");
            string fio2 = String.Join(" ", fio);
            return fio2;
        }
    }
    class Program
    {
        static object Create()
        {
            List<Teachers> teachers = new List<Teachers>();
            teachers.Add(new Teachers("Иванов Иван Иванович", "ИКИТ", "WebinarSFU"));
            teachers.Add(new Teachers("Антонов Александр Николаевич", "ИСИТ", "Zoom"));
            teachers.Add(new Teachers("Горбунов Владимир Михайло", "ИКИТ", "Discord"));
            teachers.Add(new Teachers("Пушкин Николай Сергеевич", "ГИ", "Zoom"));
            return teachers;
        }
        static Dictionary<string, ConsoleColor> CreateDict(List<Teachers> listTeachers)
        {
            /*          СПОСОБ №1
            foreach (var item in teachers)
                services.Add(item.VideoService);
            var dict = services.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            services.Clear();
            for (i = 0; i < dict.Count;)
            {
                services.Add(dict.FirstOrDefault(x => x.Value == dict.Values.Max()).Key);
                dict.Remove(dict.FirstOrDefault(x => x.Value == dict.Values.Max()).Key);
            }
            services = services.Distinct().ToList();
            */

            int i = 1;
            var services = listTeachers.Select(x => x.VideoService).GroupBy(x => x).OrderByDescending(x => x.Select(g => g).Count()).Select(x=>x.Key).ToList();
            Dictionary<string, ConsoleColor> keysColor = new Dictionary<string, ConsoleColor>();
            List<ConsoleColor> color = new List<ConsoleColor>();
            while (color.Count < services.Count)
            {
                color.Add(((ConsoleColor)(new Random().Next(1, 15))));
                if (color.Contains(ConsoleColor.Gray)) color.Remove(ConsoleColor.Gray);
                color = color.Distinct().ToList();
            }
            for (i = 0; i < services.Count; i++)
                keysColor.Add(services[i], color[i]);
            return keysColor;
        }
        static void Top3 (Dictionary<string, ConsoleColor> top3)
        {
            int i = 0;
            Console.WriteLine("Топ 3 популярных видеосервисов:");
            foreach (var item in top3.Take(3))
            {
                Console.Write($"{i + 1}: ");
                Console.ForegroundColor = (ConsoleColor)item.Value;
                Console.WriteLine($"[{item.Key}]");
                Console.ForegroundColor = ConsoleColor.White;
                i++;
            }
        }
        static void Get(List<Teachers> teachers, Dictionary<string, ConsoleColor> colorDict)
        {
            int i = 0;
            foreach (var item in teachers)
            {
                Console.Write($"{i+1} - {item.FIO.CutStr()}\t\t{item.Institute}\t\t");
                Console.ForegroundColor = (ConsoleColor)colorDict.GetValueOrDefault(item.VideoService);
                Console.WriteLine($"[{item.VideoService}]");
                Console.ForegroundColor = ConsoleColor.White;
                i++;
            }
        }
        private static Dictionary<string, ConsoleColor> AddDict(List<Teachers> teachers, Dictionary<string, ConsoleColor> keysColor)
        {
            /*             
                         СПОСОБ №1
            List<string> top3 = new List<string>();
            foreach (var item in teachers)
                top3.Add(item.VideoService);
            var dict = top3.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            top3.Clear();
            for (i = 0; i < dict.Count;)
            {
                top3.Add(dict.FirstOrDefault(x => x.Value == dict.Values.Max()).Key);
                dict.Remove(dict.FirstOrDefault(x => x.Value == dict.Values.Max()).Key);
            }
            top3 = top3.Distinct().ToList();
            */

            int i = 1;
            var services = teachers.Select(x => x.VideoService).GroupBy(x => x).OrderByDescending(x => x.Select(g => g).Count()).Select(x=>x.Key).ToList();
            Dictionary<string, ConsoleColor> dict2 = new Dictionary<string, ConsoleColor>();
            List<ConsoleColor> color = new List<ConsoleColor>();
            color.AddRange(keysColor.Values);
            while (color.Count < services.Count)
            {
                color.Add(((ConsoleColor)(new Random().Next(1, 15))));
                if (color.Contains(ConsoleColor.Gray)) color.Remove(ConsoleColor.Gray);
                color = color.Distinct().ToList();
            }
            for (i = 0; i < services.Count; i++)
            {
                if (keysColor.GetValueOrDefault(services[i]) == default) dict2.Add(services[i], color[i]);
                else dict2.Add(services[i], keysColor.GetValueOrDefault(services[i]));
            }
            return dict2;
        }
        private static void AddTeacher(List<Teachers> teachers)
        {
            IsCanAddTeacher isCanAddTeacher = (name) => teachers.Select(x => x.FIO).Any(x => x.Contains(name));
            Console.Write("Введите ФИО преподавателя: ");
            string fio = Console.ReadLine();
            while (isCanAddTeacher(fio.Split(" ").GetValue(1).ToString()))
            {
                Console.WriteLine("Преподаватель с таким именем уже существует!!!");
                Console.Write("Попробуйте ввести преподавателя с другим именем: ");
                fio = Console.ReadLine();
            }
            Console.Write("Введите институт преподавателя: ");
            string inst = Console.ReadLine();
            Console.Write("Введите видеосервис, которым пользуется преподаватель: ");
            string video = Console.ReadLine();
            teachers.Add(new Teachers(fio, inst, video));
        }

        delegate bool IsCanAddTeacher(string name);
        static void Main(string[] args)
        {
            List<Teachers> teachers = (List<Teachers>)Create();
            Dictionary<string, ConsoleColor> colorDict = CreateDict(teachers);
            ConsoleKey key;
            do
            {
                Console.WriteLine("A - Добавить преподователя\nT - Топ 3 видеосервисов" +
                    "\nG - Список преподователей\nEscape - Выход");
                Console.ForegroundColor = ConsoleColor.Black;
                key = (ConsoleKey)Console.ReadKey().Key;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                switch (key)
                {
                    case ConsoleKey.A:
                        Console.Clear();
                        AddTeacher(teachers);
                        break;
                    case ConsoleKey.T:
                        Top3(colorDict);
                        break;
                    case ConsoleKey.G:
                        Get(teachers, colorDict);
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
                if (colorDict.Keys.Count != teachers.Select(x => x.VideoService).Count()) colorDict = AddDict(teachers, colorDict);
            } while (key != ConsoleKey.Escape);
        }
    }
}