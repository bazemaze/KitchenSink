using Starcounter;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;
        public long Nr;
    }

    partial class SortableListPage : Json
    {
        [SortableListPage_json.Persons]
        partial class SortableListPagePerson : Json
        {
            protected override void OnData()
            {
                base.OnData();
            }
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
            }
        }
        void Handle(Input.Save action)
        {
            for (int i = 0; i < Persons.Count; i++)
            {
                Persons[i].Nr = i;
            }
            Transaction.Commit();
        }

        protected override void OnData()
        {
            base.OnData();
            //    //QueryResultRows<Person> persons = Db.SQL<Person>("SELECT s FROM Person s");

            //    //SortableListPage.SortableListPagePerson person; //SortableListPage.PersonsElementJson person;
            //    //foreach (var p in Persons)
            //    //{
            //    //    person = this.Persons.Add();
            //    //    person.FirstName = p.FirstName;
            //    //    person.LastName = p.LastName;
            //    //    person.Nr = p.Nr;
            //    //    //Persons.Add(person);
            //    //}
            //    //person = this.Persons.Add();
            //    //person.FirstName = "XBo";
            //    //person.LastName = "LeXan";
            //    //person.Nr = 0;
        }
    }
}
