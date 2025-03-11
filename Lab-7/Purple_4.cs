using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_4;

namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            private readonly string _name;
            private readonly string _surname;
            private double _time;
            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public string Surname
            {
                get
                {
                    return _surname;
                }
            }

            public double Time
            {
                get
                {
                    return _time;
                }
            }

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
            }

            public void Run(double time)
            {
                if (_time != 0) return;
                _time = time;
            }

            public void Print()
            {
                Console.WriteLine($"{_name} {_surname} {_time}");
            }

            public static void Sort(Sportsman[] array)
            {
                if (array is null || array.Length == 0) return;

                int n = array.Length;
                for (int i = 1, j = 2; i < n;)
                {
                    if (i == 0 || array[i] is null)
                    {
                        i = j;
                        j++;
                    } else if (array[i - 1] is null)
                    {
                        Sportsman temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--;
                    }
                    else if (array[i].Time > array[i - 1].Time)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        Sportsman temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--;
                    }
                }
            }
        }
        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;
            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public Sportsman[] Sportsmen
            {
                get
                {
                    if (_sportsmen is null) return null;

                    Sportsman[] sportsmenCopy = new Sportsman[_sportsmen.Length];
                    Array.Copy(_sportsmen, sportsmenCopy, sportsmenCopy.Length);
                    return sportsmenCopy;
                }
            }

            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];

            }

            public Group(Group group)
            {
                if (group == null) 
                    return;
                _name = group.Name;
                if (group.Sportsmen is null)
                {
                    _sportsmen = null;
                    return;
                }
                _sportsmen = new Sportsman[group.Sportsmen.Length];
                Array.Copy(group.Sportsmen, _sportsmen, _sportsmen.Length);
            }

            public void Add(Sportsman sportsman)
            {
                if (sportsman is null || _sportsmen is null)
                    return;
                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = sportsman;
            }

            public void Add(Sportsman[] sportsmen)
            {
                if (_sportsmen is null || sportsmen is null)
                    return;

                int cnt = 0;
                for (int i = 0; i < sportsmen.Length; i++)
                {
                    if (sportsmen[i] != null)
                        cnt++;
                } 

                int iter = _sportsmen.Length;
                Array.Resize(ref _sportsmen, _sportsmen.Length + cnt);

                for (int i = 0, k = _sportsmen.Length - cnt; i < sportsmen.Length; i++)
                {
                    if (sportsmen[i] != null)
                        _sportsmen[k++] = sportsmen[i];
                }
            }

            public void Add(Group group)
            {
                if (group is null || group.Sportsmen is null)
                    return;

                if (_sportsmen is null)
                    _sportsmen = new Sportsman[0];

                Add(group.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen is null)
                    return;
                int n = _sportsmen.Length;
                for (int i = 1, j = 2; i < n;)
                {
                    if (i == 0 || _sportsmen[i].Time > _sportsmen[i - 1].Time)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        Sportsman temp = _sportsmen[i];
                        _sportsmen[i] = _sportsmen[i - 1];
                        _sportsmen[i - 1] = temp;
                        i--;
                    }
                }
            }

            public static Group Merge(Group group1, Group group2)
            {
                if (group1.Sportsmen == null && group2.Sportsmen == null)
                    return new Group("Финалисты");

                if (group1.Sportsmen == null) return group2;
                if (group2.Sportsmen == null) return group1;

                Sportsman[] sportsmen = new Sportsman[group1.Sportsmen.Length + group2.Sportsmen.Length];
                int l = 0, r = 0;

                Group final = new Group("Финалисты");

                while (l < group1.Sportsmen.Length && r < group2.Sportsmen.Length)
                {
                    if (group1.Sportsmen[l].Time < group2.Sportsmen[r].Time)
                    {
                        final.Add(group1.Sportsmen[l++]);
                    }
                    else
                    {
                        final.Add(group2.Sportsmen[r++]);
                    }

                }

                while (l < group1.Sportsmen.Length)
                {
                    final.Add(group1.Sportsmen[l++]);
                }

                while (r < group2.Sportsmen.Length)
                {
                    final.Add(group2.Sportsmen[r++]);
                }


                return final;
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = new Sportsman[0];
                women = new Sportsman[0];
                if (_sportsmen is null || _sportsmen.Length == 0)
                    return;

                int menCnt = 0, womenCnt = 0;
                for (int i = 0; i < _sportsmen.Length; i++)
                {
                    if (_sportsmen[i] is SkiMan)
                    {
                        menCnt++;
                    }
                    if ( _sportsmen[i] is SkiWoman)
                    {
                        womenCnt++;
                    }
                }
                men = new Sportsman[menCnt];
                women = new Sportsman[womenCnt];

                for (int i = 0, m = 0, w = 0; i < _sportsmen.Length; i++)
                {
                    if (_sportsmen[i] is SkiMan)
                    {
                        men[m++] = _sportsmen[i];   
                    }
                    if (_sportsmen[i] is SkiWoman)
                    {
                        women[w++] = _sportsmen[i];
                    }
                }
            }

            public void Shuffle()
            {
                Sportsman[] men, women;
                Split(out men, out women);

                Sportsman.Sort(men);
                Sportsman.Sort(women);

                int m = 0, w = 0, i = 0;

                while (m < men.Length && w < women.Length)
                {
                    _sportsmen[i++] = men[m++];
                    _sportsmen[i++] = women[w++];
                }

                while (m < men.Length)
                {
                    _sportsmen[i++] = men[m++];
                }

                while (w < women.Length)
                {
                    _sportsmen[i++] = women[w++];
                }

                Array.Resize(ref _sportsmen, men.Length + women.Length);
            }

            public void Print()
            {
                Console.WriteLine(_name);
                if (_sportsmen is null)
                    return;
                for (int i = 0; i < _sportsmen.Length; i++)
                    _sportsmen[i].Print();
            }
        }

        public class SkiMan: Sportsman
        {
            public SkiMan(string name, string surname): base(name, surname) { }

            public SkiMan(string name, string surname, int time) : base(name, surname) {
                Run(time);
            }
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname) { }

            public SkiWoman(string name, string surname, int time) : base(name, surname)
            {
                Run(time);
            }
        }
    }
}