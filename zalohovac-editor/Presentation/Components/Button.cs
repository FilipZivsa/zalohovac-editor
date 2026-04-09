using zalohovac_editor.Helpers;

namespace zalohovac_editor.Presentation.Components
{
    public class Button : BaseComponent
    {
        public event Action? Clicked;

        public override bool Selectable => true;

        private string _text;

        public Button(string text, bool inline = false)
            : base(inline)
        {
            _text = text;
        }

        public override void Render(bool selected)
        {
            ConsoleHelper.WriteConditionalColor($"[ {_text} ]", selected, ConsoleColor.Red);

            base.Render(selected);
        }
        public override void HandleKey(ConsoleKeyInfo keyInfo)
        {
            if(keyInfo.Key == ConsoleKey.Enter)
                Clicked?.Invoke();
        }
    }
}
