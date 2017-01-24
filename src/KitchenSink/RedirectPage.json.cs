using Starcounter;

namespace KitchenSink
{
    partial class RedirectPage : Json
    {
        void Handle(Input.GoToHomePartial Action)
        {
            this.MorphUrl = "/KitchenSink";
        }

        void Handle(Input.ChooseFood Action)
        {
            switch (Action.Value)
            {
                case "Fruit":
                    this.MorphUrl = "/KitchenSink/Redirect/apple";
                    break;

                case "Vegetable":
                    this.MorphUrl = "/KitchenSink/Redirect/carrot";
                    break;

                case "Bread":
                    this.MorphUrl = "/KitchenSink/Redirect/baguette";
                    break;
            }
        }

        void Handle(Input.GoToDocs Action)
        {
            this.RedirectUrl = "https://starcounter.io/";
        }
    }
}