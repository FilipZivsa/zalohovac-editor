using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using zalohovac_editor.Presentation.Components;
using zalohovac_editor.Models; // Zkontroluj si, že máš správný namespace

namespace zalohovac_editor.Presentation.Windows
{
    public class JobEditWindow : BaseWindow
    {
        private TextBox _sourceTextBox;
        private TextBox _targetTextBox;
        private TextBox _timingTextBox;
        private SelectBox<BackupMethod> _methodSelectBox;
        private NumberBox _retCountNumberBox;
        private NumberBox _retSizeNumberBox;

        private Button _saveButton;
        private Button _cancelButton;

        public JobEditWindow(Application application, IWindow? returnWindow = null)
            : base("Novy Backup Job | Zalohovac Editor", application, returnWindow)
        {
            // ZMĚNA: Upravené texty, aby uživatel věděl o středníku
            _sourceTextBox = new TextBox("Zdroje (oddelte ;):\t", 40);
            _targetTextBox = new TextBox("Cile (oddelte ;):\t", 40);
            _timingTextBox = new TextBox("CRON Timing: \t\t", 15);

            _methodSelectBox = new SelectBox<BackupMethod>("Metoda: \t\t");
            _methodSelectBox.Items = Enum.GetValues(typeof(BackupMethod)).Cast<BackupMethod>().ToList();
            _methodSelectBox.Value = _methodSelectBox.Items[0];

            _retCountNumberBox = new NumberBox("Retention (pocet):\t", 999);
            _retSizeNumberBox = new NumberBox("Retention (velikost):\t", 9999);

            _saveButton = new Button("Ulozit do JSON", true);
            _cancelButton = new Button("Ukoncit", true);

            RegisterComponent(_sourceTextBox);
            RegisterComponent(_targetTextBox);
            RegisterComponent(_timingTextBox);
            RegisterComponent(_methodSelectBox);
            RegisterComponent(_retCountNumberBox);
            RegisterComponent(_retSizeNumberBox);
            RegisterComponent(new EmptyLine());
            RegisterComponent(_saveButton);
            RegisterComponent(_cancelButton);

            _saveButton.Clicked += SaveButtonClicked;
            _cancelButton.Clicked += CancelButtonClicked;
        }

        private void SaveButtonClicked()
        {
            try
            {
                // --- BONUS 4: Validace CRONu ---
                string cronText = _timingTextBox.Value.Trim();
                string[] cronParts = cronText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (cronParts.Length != 5)
                {
                    IWindow errorWindow = new ErrorWindow("Chyba validace", "CRON musi mit presne 5 casti (napr. '5 4 * * *').", _application, this);
                    errorWindow.Show();
                    return;
                }

                // --- BONUS 2: Více zdrojů a cílů ---
                // Rozsekneme text, odstraníme z cest mezery a uložíme do Listu
                List<string> zdroje = _sourceTextBox.Value
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(cesta => cesta.Trim())
                    .ToList();

                List<string> cile = _targetTextBox.Value
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(cesta => cesta.Trim())
                    .ToList();

                // Kontrola, jestli uživatel nezadal prázdné políčko
                if (zdroje.Count == 0 || cile.Count == 0)
                {
                    IWindow errorWindow = new ErrorWindow("Chyba validace", "Musite zadat alespon jeden zdroj a cil.", _application, this);
                    errorWindow.Show();
                    return;
                }
                // ------------------------------------

                // Místo textu nyní přidáváme rovnou naše vytvořené Listy 'zdroje' a 'cile'
                BackupJob novyJob = new BackupJob
                {
                    Sources = zdroje,
                    Targets = cile,
                    Timing = cronText,
                    Method = _methodSelectBox.Value,
                    Retention = new BackupRetention
                    {
                        Count = _retCountNumberBox.Value,
                        Size = _retSizeNumberBox.Value
                    }
                };

                List<BackupJob> konfigurace = new List<BackupJob> { novyJob };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

                string jsonString = JsonSerializer.Serialize(konfigurace, options);
                File.WriteAllText("config.json", jsonString);

                _application.Stop();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Jejda, chyba při ukládání: " + ex.Message);
                Console.ReadKey();
                _application.Stop();
            }
        }

        private void CancelButtonClicked()
        {
            _application.Stop();
        }
    }
}