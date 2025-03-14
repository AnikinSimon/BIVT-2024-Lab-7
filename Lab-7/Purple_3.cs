using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            private readonly string _name;
            private readonly string _surname;
            private double[] _marks;
            private int[] _places;
            private int _curMark;
            private int _score;
            private double _totalMark;

            public readonly string Name
            {
                get
                {
                    return _name;
                }
            }

            public readonly string Surname
            {
                get
                {
                    return _surname;
                }
            }

            public double[] Marks
            {
                get
                {
                    if (_marks is null) return null;
                    double[] marksCopy = new double[_marks.Length];
                    Array.Copy(_marks, marksCopy, marksCopy.Length);
                    return marksCopy;
                }
            }

            public int[] Places
            {
                get
                {
                    if (_places is null) return null;
                    int[] placesCopy = new int[_places.Length];
                    Array.Copy(_places, placesCopy, placesCopy.Length);
                    return placesCopy;
                }
            }

            public int Score
            {
                get
                {
                    return _score;
                }
            }

            private double TotalMark
            {
                get
                {
                    return _totalMark;
                }
            }

            private int TopPlace
            {
                get
                {
                    if (_places is null) return 0;

                    return _places.Min();
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _curMark = 0;
                _score = 0;
                _totalMark = 0;
            }

            public void Evaluate(double result)
            {
                if (_marks is null || _curMark == 7) return;

                if (result < 0 || result > 6)
                {
                    return;
                }
                _marks[_curMark++] = result;
                _totalMark += result;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;

                for (int judge = 0; judge < 7; judge++)
                {
                    for (int i = 1, j = 2; i < participants.Length;)
                    {
                        if (i == 0)
                        {
                            i = j;
                            j++;
                        }
                        else if (participants[i]._marks is null)
                        {
                            i = j;
                            j++;
                        }
                        else if (participants[i - 1]._marks is null)
                        {
                            Participant tmp = participants[i];
                            participants[i] = participants[i - 1];
                            participants[i - 1] = tmp;
                            i--;
                        }
                        else if (participants[i].Marks[judge] <= participants[i - 1].Marks[judge])
                        {
                            i = j;
                            j++;
                        }
                        else
                        {
                            Participant tmp = participants[i];
                            participants[i] = participants[i - 1];
                            participants[i - 1] = tmp;
                            i--;
                        }
                    }
                    for (int idx = 0, place = 1; idx < participants.Length; idx++)
                    {
                        if (participants[idx]._places != null)
                        {
                            participants[idx]._places[judge] = place;
                            participants[idx]._score += place;
                            place++;
                        }
                            
                    }

                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length < 2) return;

                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0)
                    {
                        i = j;
                        j++;
                        continue;
                    }
                    int score1 = array[i].Score, score2 = array[i - 1].Score;
                    if (score1 > score2)
                    {
                        i = j;
                        j++;
                    }
                    else if (score1 < score2)
                    {
                        Participant tmp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = tmp;
                        i--;
                    }
                    else
                    {
                        if (score1 == score2 && score1 == 0)
                        {
                            i = j;
                            j++;
                            continue;
                        }
                        int isHigher = 0;
                        int[] places1 = array[i].Places, places2 = array[i - 1].Places;
                        for (int k = 0; k < 7; k++)
                        {
                            if (places1[k] < places2[k])
                            {
                                isHigher = 1;
                                break;
                            }
                            else if (places1[k] < places2[k])
                            {
                                isHigher = -1;
                                break;
                            }
                        }
                        if (isHigher == 1)
                        {
                            Participant tmp = array[i];
                            array[i] = array[i - 1];
                            array[i - 1] = tmp;
                            i--;
                            continue;
                        }
                        else if (isHigher == -1)
                        {
                            i = j;
                            j++;
                            continue;
                        }

                        double totalMark1 = array[i].TotalMark, totalMark2 = array[i - 1].TotalMark;
                        if (totalMark1 < totalMark2)
                        {
                            i = j;
                            j++;
                        }
                        else
                        {
                            Participant tmp = array[i];
                            array[i] = array[i - 1];
                            array[i - 1] = tmp;
                            i--;
                        }
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{_name} {_surname} {Score} {TopPlace} {TotalMark}");
            }

        }

        public abstract class Skating
        {
            protected Participant[] _participants;
            protected double[] _moods;
            private int _offsetParticipants;

            public Participant[] Participants
            {
                get
                {

                    return _participants;
                }
            }

            public double[] Moods
            {
                get
                {
                    if (_participants == null)
                        return null;

                    double[] moodsCopy = new double[_moods.Length];
                    Array.Copy(_moods, moodsCopy, _moods.Length);

                    return moodsCopy;
                }
            }

            protected abstract void ModificateMood();

            public Skating(double[] moods)
            {
                if (moods == null)
                    return;

                _moods = new double[moods.Length];
                Array.Copy(moods, _moods, moods.Length);

                ModificateMood();
                
                _participants = new Participant[0];
            }

            public void Evaluate(double[] marks)
            {
                if (_participants == null || _moods is null || marks is null) return;

                if (_participants.Length == 0 || _offsetParticipants == _participants.Length || marks.Length < _moods.Length) return;


                for (int i = 0; i < _moods.Length; i++)
                {
                        _participants[_offsetParticipants].Evaluate(marks[i] * _moods[i]);
                }
                _offsetParticipants++;
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
                {
                    _participants = new Participant[0];
                }

                Array.Resize(ref _participants, _participants.Length + participants.Length);

                for (int i = 0, k = _participants.Length - participants.Length; i < participants.Length; i++)
                {
                    _participants[k++] = participants[i];
                }

            }

        }

        public class FigureSkating: Skating
        {
            public FigureSkating(double[] moods): base(moods) { }
            protected override void ModificateMood()
            {
                if (_moods is null || _moods.Length == 0) return;

                for (int i = 0; i < _moods.Length; i++)
                    _moods[i] += ((i + 1) / 10.0);
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] moods) : base(moods) { }
            protected override void ModificateMood()
            {
                if (_moods is null || _moods.Length == 0) return;

                for (int i = 0; i < _moods.Length; i++)
                    _moods[i] *= (1 + (i + 1) / 100.0);
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

    }
}