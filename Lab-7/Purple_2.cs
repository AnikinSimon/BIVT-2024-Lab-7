using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            private readonly string _name;
            private readonly string _surname;
            private int _distance;
            private int[] _marks;
            private int _result;

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

            public int Distance
            {
                get
                {
                    return _distance;
                }
            }

            public int[] Marks
            {
                get
                {
                    if (_marks is null) return null;
                    int[] marksCopy = new int[_marks.Length];
                    Array.Copy(_marks, marksCopy, marksCopy.Length);
                    return marksCopy;
                }
            }

            public int Result
            {
                get
                {

                    return _result;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
                _result = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (_marks is null || marks is null || marks.Length != 5) return;

                for (int i = 0; i < 5; i++)
                {
                    if (marks[i] < 1 || marks[i] > 20)
                    {
                        return;
                    }
                }

                _distance = distance;
                int maxMarkIdx = 0, minMarkIdx = 0;

                for (int i = 0; i < 5; i++)
                {
                    _marks[i] = marks[i];
                    if(_marks[i] > _marks[maxMarkIdx])
                        maxMarkIdx = i;
                    if (_marks[i] < _marks[minMarkIdx])
                        minMarkIdx = i;
                    _result += _marks[i];
                }

                _result -= _marks[maxMarkIdx];
                _result -= _marks[minMarkIdx];

                _result += 60 + (_distance - target) * 2;

                if (_result < 0) _result = 0;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length < 2) return;

                int n = array.Length;
                for (int i = 1, j = 2; i < n;)
                {
                    if (i == 0 || array[i].Result < array[i - 1].Result)
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
                if (_surname == "" || _name == "")
                {
                    return;
                }

                Console.WriteLine($"{_name} {_surname} {Result}");
            }
        }

        public abstract class SkiJumping
        {
            private readonly string _name;
            private int _standard;
            private Participant[] _participants;
            private int _offsetJump;

            public string Name
            {
                get { return _name; }
            }

            public int Standard
            {
                get { return _standard; }
            }

            public Participant[] Participants
            {
                get {
                    return _participants;
                }
            }

            public SkiJumping(string name, int standart) { 
                _name = name;
                _standard = standart;
                _participants = new Participant[0];
                _offsetJump = 0;
            }

            public void Add(Participant participant)
            {

                if (_participants is null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);

                _participants[_participants.Length - 1] = participant;

            }

            public void Add(Participant[] participants)
            {
                if (participants is null)
                    return;

                if (_participants is null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + participants.Length);

                for (int i = 0, k = _participants.Length - participants.Length; i < participants.Length; i++)
                {
                    _participants[k++] = participants[i];
                }

            }

            public void Jump(int distance, int[] marks)
            {
                if (_participants == null || _participants.Length == 0 || _offsetJump == _participants.Length) return;

                _participants[_offsetJump++].Jump(distance, marks, _standard);
            }

            public void Print()
            {
                Console.WriteLine(_name);
                Console.WriteLine(_standard);
                if (_participants is null)
                    return;

                for (int i = 0; i < _participants.Length; i++)
                    _participants[i].Print();
            }
        }

        public class JuniorSkiJumping: SkiJumping
        {

            public JuniorSkiJumping(): base("100m", 100){}
        }

        public class ProSkiJumping : SkiJumping
        {

            public ProSkiJumping() : base("150m", 150) { }
        }
    }
}