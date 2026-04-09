using zalohovac_editor.Helpers;

namespace zalohovac_editor.Presentation.Components
{
    public class NumberBox : BaseComponent
    {
        public override bool Selectable => true;

        public int Value { get; set; }

        private string _text;
        private int _max;
        private int _size;

        public NumberBox(string text, int max = 999999, bool inline = false)
            : base(inline)
        {
            Value = 0;
            _text = text;
            _max = max;
            _size = (int)Math.Log10(max) + 1;
        }

        public override void Render(bool selected)
        {
            char pad = selected ? '_' : ' ';
            string content = Value.ToString().PadRight(_size, pad);

            ConsoleHelper.WriteConditionalColor($"{_text}{content}", selected, ConsoleColor.Red);

            base.Render(selected);
        }
        public override void HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (char.IsDigit(keyInfo.KeyChar))
            {
                int keyValue = int.Parse(keyInfo.KeyChar.ToString());
                int newValue = (Value * 10) + keyValue;
                if(newValue <= _max)
                    Value = newValue;
            }
            else if(keyInfo.Key == ConsoleKey.UpArrow && Value < _max)
            {
                Value++;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow && Value > 0)
            {
                Value--;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && Value > 0)
            {
                Value /= 10;
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                Value = 0;
            }
        }
    }
}
