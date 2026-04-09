using GradeRegisterAdminApp.Entities;
using GradeRegisterAdminApp.Helpers;

namespace GradeRegisterAdminApp.Presentation.Components
{
    public class SelectBox<T> : BaseComponent
        where T : class, IEntity
    {
        public override bool Selectable => true;

        public T? Value { get; set; }
        public List<T> Items { get; set; }

        private string _text;
        private int _size;

        public SelectBox(string text, int size = 6)
        {
            Value = null;
            Items = new List<T>();
            _text = text;
            _size = size;
        }

        public override void Render(bool selected)
        {
            string value = Value?.ToString() ?? string.Empty;
            
            if(_size < value.Length)
                _size = value.Length;

            char pad = ' ';
            string content = Value != null ? value : "-----";
            string tail = string.Empty.PadRight(_size - value.Length, pad);

            ConsoleHelper.WriteConditionalColor($"{_text}[ {content} ]{tail}", selected, ConsoleColor.Red);

            base.Render(selected);
        }
        public override void HandleKey(ConsoleKeyInfo keyInfo)
        {
            T? item = Items.SingleOrDefault(i => i.ID == Value?.ID);
            int currentIndex = item != null ? Items.IndexOf(item) : -1;

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (currentIndex >= 0)
                {
                    int nextIndex = (currentIndex + 1) % Items.Count;
                    Value = Items[nextIndex];
                }
                else if (Items.Count > 0)
                {
                    Value = Items[0];
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (currentIndex >= 0)
                {
                    int previousIndex = (currentIndex - 1 + Items.Count) % Items.Count;
                    Value = Items[previousIndex];
                }
                else if (Items.Count > 0)
                {
                    Value = Items[Items.Count - 1];
                }
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                Value = null;
            }
        }
    }
}
