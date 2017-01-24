using Starcounter;

namespace KitchenSink
{
    partial class DialogPage : Json
    {
        void Handle(Input.OpenClick Action)
        {
            Action.Cancel();

            this.Opened = true;
        }

        void Handle(Input.ConfirmClick Action)
        {
            this.Opened = false;
            this.Message = "You have accepted the dialog box";
            this.MessageCss = "alert alert-success";
        }

        void Handle(Input.RejectClick Action)
        {
            this.Opened = false;
            this.Message = "You have rejected the dialog box";
            this.MessageCss = "alert alert-danger";
        }
    }
}