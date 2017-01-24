using System;
using System.Linq;
using Simplified.Ring3;
using Starcounter;

namespace KitchenSink
{
    partial class AutocompletePage : Partial
    {
        private static readonly Country[] AvailableCountries = new[]
        {
            new Country("Poland", "Warsaw"),
            new Country("Sweden", "Stockholm"),
            new Country("Germany", "Berlin"),
            new Country("Norway", "Oslo"),
            new Country("Pakistan", "Islamabad"),
            new Country("Portugal", "Lisbon"),
            new Country("Scotland", "Edinburgh"),
        };

        public AutocompletePage Init()
        {
            AddProduct("Bread", 1);
            AddProduct("Butter", 3);
            AddProduct("Scotch Whisky", 4);
            AddProduct("Irish Whiskey", 2);
            AddProduct("Milk", 5);
            AddProduct("Boiled Mutton", 7);
            Transaction.Commit();

            return this;
        }

        private static void AddProduct(string name, int price)
        {
            if (
                Db.SQL<long>("SELECT count (p) from Simplified.Ring3.Product p WHERE name = ? FETCH ?", name, 1).First ==
                0)
            {
                new Product() {Name = name, Price = price};
            }
        }

        public void Handle(Input.ProductsSearch action)
        {
            var searchTerm = action.Value == "*" ? "%" : $"%{action.Value}%";
            this.FoundProducts = Db.SQL("SELECT i FROM Simplified.Ring3.Product i WHERE Name LIKE ?", searchTerm);
            foreach (var product in FoundProducts)
            {
                product.SelectAction = () =>
                {
                    FoundProducts.Clear();
                    ProductsSearch = product.Name;
                    ProductsText = $"{product.Name} costs ${product.Price}";
                };
            }
        }

        public void Handle(Input.ClearProducts _)
        {
            this.FoundProducts.Clear();
        }

        [AutocompletePage_json.FoundProducts]
        public partial class FoundProductsItem
        {
            public Action SelectAction { get; set; }
            void Handle(Input.Select action) => SelectAction();
        }

        public void Handle(Input.PlacesSearch action)
        {
            Func<Country, bool> predicate;
            if (action.Value == "*")
            {
                predicate = s => true;
            }
            else
            {
                predicate = p => p.Name.ToLowerInvariant().StartsWith(action.Value.ToLowerInvariant());
            }

            this.FoundPlaces.Clear();
            foreach (var foundPlace in AvailableCountries.Where(predicate))
            {
                var json = FoundPlaces.Add();
                json.Name = foundPlace.Name;
                json.SelectAction = () =>
                {
                    FoundPlaces.Clear();
                    PlacesSearch = foundPlace.Name;
                    PlacesText = $"Capital of {foundPlace.Name} is {foundPlace.CapitalName}";
                };
            }
        }

        public void Handle(Input.ClearPlaces _)
        {
            this.FoundPlaces.Clear();
        }

        [AutocompletePage_json.FoundPlaces]
        public partial class FoundPlacesItem
        {
            public Action SelectAction { get; set; }
            void Handle(Input.Select action) => SelectAction();
        }
    }
}