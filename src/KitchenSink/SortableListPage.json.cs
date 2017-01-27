using System;
using Starcounter;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;
        public int Nr;
    }

    partial class SortableListPage : Json
    {
        protected override void OnData()
        {
            base.OnData();

            CreateDataBaseIfNotExists();

            SetViewModelProperties();
        }

        private static void CreateDataBaseIfNotExists()
        {
            var existAnyone = Db.SQL<Person>("SELECT p FROM Person p").First;
            if (existAnyone == null)
            {
                int nrCounter = 0;
                #region create some dummy persons, (0 < Nr < nmb of persons)
                new Person
                {
                    FirstName = "Elvis",
                    LastName = "Operator",
                    Nr = nrCounter++
                };
                new Person
                {
                    FirstName = "Bo",
                    LastName = "Lean",
                    Nr = nrCounter++
                };
                new Person
                {
                    FirstName = "Lambda",
                    LastName = "Linq",
                    Nr = nrCounter++
                };
                new Person
                {
                    FirstName = "Delegate",
                    LastName = "009",
                    Nr = nrCounter++
                };
                #endregion
            }
        }

        private void SetViewModelProperties()
        {
            QueryResultRows<Person> persons = Db.SQL<Person>("SELECT s FROM Person s ORDER BY s.Nr");
            this.Persons = persons;

            //Set border Persons
            Persons[0].FirstElement = true;
            Persons[Persons.Count - 1].LastElement = true;
            for (int i = 1; i < Persons.Count - 1; i++)
            {
                Persons[i].MiddleElement = true;
            }
            Transaction.Commit();
        }

        [SortableListPage_json.Persons]
        partial class SortableListPagePerson : Json
        {
            void Handle(Input.UpDown upDown)
            {
                var root = (SortableListPage)this.Parent.Parent;
                int ind = this.IndexInParent;

                //Down
                if (upDown.OldValue < upDown.Value && ind < root.Persons.Count - 1)
                {
                    var personToMoveDown = root.Persons[ind];
                    root.Persons.RemoveAt(ind);
                    root.Persons.Insert(ind + 1, personToMoveDown);
                }
                //Up
                else if (upDown.OldValue > upDown.Value && ind > 0)
                {
                    var personToMoveUp = root.Persons[ind];
                    root.Persons.RemoveAt(ind);
                    root.Persons.Insert(ind - 1, personToMoveUp);
                }

                SetBorderPersons(root);
                SetNewPersonsOrder(root);
                Transaction.Commit();
            }

            private static void SetNewPersonsOrder(SortableListPage root)
            {
                for (int i = 0; i < root.Persons.Count; i++)
                {
                    root.Persons[i].Nr = i;
                }
            }

            private void SetBorderPersons(SortableListPage root)
            {
                root.Persons[0].FirstElement = true;
                root.Persons[0].MiddleElement = false;
                root.Persons[0].LastElement = false;

                root.Persons[root.Persons.Count - 1].FirstElement = false;
                root.Persons[root.Persons.Count - 1].MiddleElement = false;
                root.Persons[root.Persons.Count - 1].LastElement = true;

                for (int i = 1; i < root.Persons.Count - 1; i++)
                {
                    root.Persons[i].FirstElement = false;
                    root.Persons[i].MiddleElement = true;
                    root.Persons[i].LastElement = false;
                }
            }
        }
    }
}
