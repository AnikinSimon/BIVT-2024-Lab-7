using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumpIndex;

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

            public double[] Coefs
            {
                get
                {
                    if (_coefs is null) return null;
                    double[] coefsCopy = new double[_coefs.Length];
                    Array.Copy(_coefs, coefsCopy, _coefs.Length);
                    return coefsCopy;
                }
            }

            public int[,] Marks
            {
                get
                {
                    if (_marks is null) return null;
                    int[,] marksCopy = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < _marks.GetLength(1); j++)
                        {
                            marksCopy[i, j] = _marks[i, j];
                        }
                    }
                    return marksCopy;
                }
            }

            public double TotalScore
            {
                get
                {
                    if (_marks is null || _coefs is null)
                    {
                        Console.WriteLine("error");
                        return 0;
                    }

                    int coefsLen = _marks.GetLength(0), marksLen = _marks.GetLength(1);
                    double score = 0;
                    for (int i = 0; i < coefsLen; i++)
                    {
                        int markSum = 0, maxMarkIdx = 0, minMarkIdx = 0;
                        for (int j = 0; j < marksLen; j++)
                        {
                            markSum += _marks[i, j];
                            if (_marks[i, j] >= _marks[i, maxMarkIdx])
                            {
                                maxMarkIdx = j;
                            }
                            if (_marks[i, j] <= _marks[i, minMarkIdx])
                            {
                                minMarkIdx = j;

                            }

                        }
                        markSum -= _marks[i, maxMarkIdx];
                        markSum -= _marks[i, minMarkIdx];

                        score += markSum * _coefs[i];
                    }
                    return score;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    _coefs[i] = 2.5;
                }
                _marks = new int[4, 7];
                _jumpIndex = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (_coefs is null || coefs is null || coefs.Length < 4)
                {
                    Console.WriteLine("error");
                    return;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (coefs[i] < 2.5 || coefs[i] > 3.5)
                    {
                        return;
                    }
                }
                Array.Copy(coefs, _coefs, 4);
            }

            public void Jump(int[] marks)
            {
                if (marks is null || marks.Length < 7 || _jumpIndex >= 4)
                {
                    return;
                }

                for (int i = 0; i < 7; i++)
                {
                    if (marks[i] < 1 || marks[i] > 6)
                    {
                        return;
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    _marks[_jumpIndex, i] = marks[i];
                }
                _jumpIndex++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length < 2) return;

                int n = array.Length;
                for (int i = 1, j = 2; i < n;)
                {
                    if (i == 0 || array[i] == null) 
                    {
                        i = j;
                        j++;
                    } else if (array[i - 1] == null)
                    {
                        Participant temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--;
                    } else if (array[i].TotalScore <= array[i - 1].TotalScore)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        Participant temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--;
                    }
                }
            }

            public void Print()
            {
                if (_surname == null || _name == null)
                {
                    return;
                }

                Console.WriteLine($"{_name} {_surname} {TotalScore:F2}");
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _marksOffset;
            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public Judge(string name, int[] marks)
            {
                _name = name;
                if (marks == null)
                {
                    return;
                }
                _marksOffset = 0;

                _marks = new int[marks.Length];

                Array.Copy(marks, _marks, marks.Length);
            }

            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0)
                    return 0;
                if (_marksOffset >= _marks.Length)
                    _marksOffset %= _marks.Length;
                return _marks[_marksOffset++];
            }

            public void Print()
            {
                if (_name == null)
                    return; 
                Console.WriteLine($"{_name}");

                if (_marks != null)

                PrintArr(_marks);
            }
        }

        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges {
                get
                {
                    if (_judges == null)
                        return null;

                    Judge[] judgesCopy = new Judge[_judges.Length];
                    Array.Copy(_judges, judgesCopy, _judges.Length);

                    return judgesCopy;
                }
            }
            
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null)
                        return null;

                    Participant[] participantsCopy = new Participant[_participants.Length];
                    Array.Copy(_participants, participantsCopy, _participants.Length);

                    return participantsCopy;
                }
            }

            public Competition(Judge[] judges)
            {

                _participants = new Participant[0];
                if (judges == null)
                {
                    return;
                }

                _judges = new Judge[judges.Length];
                
                Array.Copy(judges, _judges, judges.Length);
            }

            public void Evaluate(Participant jumper)
            {
                if (jumper is null || _judges is null)
                    return;

                int[] marks = new int[7];

                for (int j = 0, iter = 0; j < _judges.Length; j++)
                {
                    if (iter >= 7)
                        break;
                    if (_judges[j] != null)
                        marks[iter++] = _judges[j].CreateMark();
                }

                jumper.Jump(marks);
            }

            public void Add(Participant participant)
            {
                if (participant == null)
                    return;

                if (_participants == null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);

                _participants[_participants.Length - 1] = participant;   

                Evaluate(_participants[_participants.Length - 1]);
            }

            public void Add(Participant[] participants)
            {
                if (participants == null)
                    return;

                if (_participants == null)
                    _participants = new Participant[0];

                int prevLen = _participants.Length;

                Array.Resize(ref _participants, _participants.Length + participants.Length);

                for (int i = 0; i < participants.Length; i++)
                {
                      _participants[i + prevLen] = participants[i];
                      Evaluate(_participants[i + prevLen]); 
                }

            }

            public void Sort()
            {
                Participant.Sort(_participants);
            }

        }

        private static void PrintArr(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                return;
            for (int i = 0; i < arr.Length; i++)
                Console.Write($"{arr[i]} ");
            Console.WriteLine();
        }

        private static void PrintArr(double[] arr)
        {
            if (arr == null || arr.Length == 0)
                return;
            for (int i = 0; i < arr.Length; i++)
                Console.Write($"{arr[i]} ");
            Console.WriteLine();
        }

        private static void PrintArr(Participant[] arr)
        {
            if (arr == null || arr.Length == 0)
                return;
            for (int i = 0; i < arr.Length; i++)
                Console.Write($"{arr[i].TotalScore} ");
            Console.WriteLine();
        }

    }
}