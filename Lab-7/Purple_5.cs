using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_5;

namespace Lab_7
{
    public class Purple_5
    {
        public struct Response
        {
            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal
            {
                get
                {
                    return _animal;
                }
            }

            public string CharacterTrait
            {
                get
                {
                    return _characterTrait;
                }
            }

            public string Concept
            {
                get
                {
                    return _concept;
                }
            }

            public Response(string animal, string characterTrait, string concept)
            {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (questionNumber < 1 || questionNumber > 3) return 0;
                int count = 0;
                for (int i = 0; i < responses.Length; i++)
                {
                    switch (questionNumber)
                    {
                        case 1:
                            if (responses[i].Animal != null && responses[i].Animal == this.Animal)
                                count++;
                            break;
                        case 2:
                            if (responses[i].CharacterTrait != null && responses[i].CharacterTrait == this.CharacterTrait)
                                count++;
                            break;
                        case 3:
                            if (responses[i].Concept != null && responses[i].Concept == this.Concept)
                                count++;
                            break;
                    }
                }

                return count;
            }

            public void Print()
            {
                Console.WriteLine($"{_animal} {_characterTrait} {_concept}");
            }
        }

        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public Response[] Responses
            {
                get
                {
                    return _responses;
                }
            }

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (answers == null || answers.Length != 3 || _responses == null)
                    return;

                Console.WriteLine("{0}, {1}, {2}", answers[0], answers[1], answers[2]);

                Response response = new Response(answers[0], answers[1], answers[2]);
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = response;
                _responses[_responses.Length - 1].Print();
            }

            public string[] GetTopResponses(int question)
            {
                if (question < 1 || question > 3 || _responses == null)
                {
                    return null;
                }

                string[] responses = new string[_responses.Length];

                for (int i = 0; i < _responses.Length; i++)
                {
                    switch (question)
                    {
                        case 1:
                            responses[i] = _responses[i].Animal;
                            break;
                        case 2:
                            responses[i] = _responses[i].CharacterTrait;
                            break;
                        case 3:
                            responses[i] = _responses[i].Concept;
                            break;
                    };
                }

                var topResponses = responses
                .Where(x => x != null)
                .GroupBy(word => word)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToArray();

                return topResponses;

            }

            public void Print()
            {
                Console.WriteLine(_name);
                if (_responses is null)
                    return;
                for (int i = 0; i < _responses.GetLength(0); i++)
                {
                    _responses[i].Print();
                }
            }
        }

        public class Report
        {
            private Research[] _researches;
            private static int _id;

            public Research[] Researches
            {
                get
                {
                    return _researches;
                }
            }

            static Report()
            {
                _id = 1;
            }

            public Report() { 
                _researches = new Research[0];
            }

            public Research MakeResearch()
            {
                DateTime timeNow = DateTime.Now;

                string reseachName = $"No_{_id++}_{timeNow.Month:00}/{timeNow.Year % 100:00}";

                if (_researches is null)
                    _researches = new Research[0];

                Array.Resize(ref _researches, _researches.Length + 1);

                Research newResearch = new Research(reseachName);

                _researches[_researches.Length - 1] = newResearch;

                return newResearch;
            }

            public (string, double)[] GetGeneralReport(int question)
            {

                if (question < 1 || question > 3 || _researches == null)
                {
                    return null;
                }

                string[] res = (from resp in (
                                    from research in _researches
                                    from response in research.Responses
                                    select question switch
                                    {
                                        1 => response.Animal,
                                        2 => response.CharacterTrait,
                                        3 => response.Concept,
                                    }
                                )
                                where resp != null
                                select resp).ToArray();


                (string, double)[] generalReport = res
                                                .GroupBy(word => word)
                                                .Select(group => (group.Key, 100.0 * group.Count() / res.Length))
                                                .ToArray();


                return generalReport; 
            }
        }
    }
}