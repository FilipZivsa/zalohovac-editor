using GradeRegisterAdminApp.Entities;
using GradeRegisterAdminApp.Helpers;
using System.ComponentModel;
using System.Reflection;

namespace GradeRegisterAdminApp.Presentation.Components
{
    public class Table<T> : BaseComponent
        where T : class, IEntity
    {
        public event Action? ItemSelected;

        public override bool Selectable => true;
        public T? SelectedItem => Items.Count > 0
            ? Items[Math.Min(_selectedIndex, Items.Count - 1)]
            : null;

        public List<T> Items { get; set; }

        private int _count;
        private int _offset;
        private int _selectedIndex;
        private List<string> _headers;
        private List<int> _widths;

        public Table(int count = 10)
        {
            Items = new List<T>();
            _count = count;
            _offset = 0;
            _selectedIndex = 0;
            _headers = ExtractPropertyNames(typeof(T));
            _widths = _headers.Select(h => h.Length).ToList();
        }

        public override void Render(bool selected)
        {
            List<List<string>> rows = Items
                .Select(i => ExtractPropertyValues(typeof(T), i))
                .ToList();

            CalculateWidths(rows);

            RenderRow(null, '+', '-', selected, ConsoleColor.Red);
            RenderRow(_headers, '+', ' ', selected, ConsoleColor.Red);
            RenderRow(null, '+', '=', selected, ConsoleColor.Red);

            for (int i = _offset; i < _offset + _count; i++)
            {
                if(i < Items.Count)
                {
                    bool selectedRow = Items[i] == SelectedItem;
                    RenderRow(rows[i], '|', ' ', selectedRow, ConsoleColor.Green);
                }
                else
                {
                    RenderRow(null, '|', ' ', false, ConsoleColor.White);
                }
            }

            RenderRow(null, '+', '-', selected, ConsoleColor.Red);
        }
        public override void HandleKey(ConsoleKeyInfo keyInfo)
        {
            if(_selectedIndex > Items.Count - 1)
                _selectedIndex = Items.Count - 1;

            if(keyInfo.Key == ConsoleKey.UpArrow && _selectedIndex > 0)
            {
                _selectedIndex--;

                if (_selectedIndex == _offset - 1)
                    _offset--;
            }
            else if(keyInfo.Key == ConsoleKey.DownArrow && _selectedIndex < Items.Count - 1)
            {
                _selectedIndex++;

                if (_selectedIndex == _offset + _count)
                    _offset++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                ItemSelected?.Invoke();
            }
        }

        private void RenderRow(List<string>? values, char sep, char pad, bool selected, ConsoleColor color)
        {
            for(int i = 0; i < _widths.Count; i++)
            {
                string value = values?[i] ?? string.Empty;
                string text = value.PadRight(_widths[i], pad);
                ConsoleHelper.WriteConditionalColor($"{sep}{pad}{text}{pad}", selected, color);
            }
            ConsoleHelper.WriteLineConditionalColor($"{sep}", selected, color);
        }
        private void CalculateWidths(List<List<string>> rows)
        {
            for (int i = 0; i < _widths.Count; i++)
            {
                foreach (List<string> values in rows)
                {
                    if (values[i].Length > _widths[i])
                        _widths[i] = values[i].Length;
                }
            }
        }

        private List<string> ExtractPropertyNames(Type type)
        {
            return type
            .GetProperties()
                .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Equals(BrowsableAttribute.Yes) ?? true)
                .Select(p => p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? p.Name)
                .ToList();
        }
        private List<string> ExtractPropertyValues(Type type, object obj)
        {
            return type
                .GetProperties()
                .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Equals(BrowsableAttribute.Yes) ?? true)
                .Select(p => p.GetValue(obj)?.ToString() ?? string.Empty)
                .ToList();
        }
    }
}
