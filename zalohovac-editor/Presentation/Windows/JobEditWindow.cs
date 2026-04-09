using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using zalohovac_editor.Presentation.Components;
using zalohovac_editor.Models; // Uprav, pokud se tvůj namespace jmenuje jinak

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
            // 1. Vytvoření políček
            _sourceTextBox = new TextBox("Zdrojova cesta: \t", 40);
            _targetTextBox = new TextBox("Cilova cesta: \t", 40);
            _timingTextBox = new TextBox("CRON Timing: \t", 15);

            _methodSelectBox = new SelectBox<BackupMethod>("Metoda: \t\t");
            _methodSelectBox.Items = Enum.GetValues(typeof(BackupMethod)).Cast<BackupMethod>().ToList();
            _methodSelectBox.Value = _methodSelectBox.Items[0];

            _retCountNumberBox = new NumberBox("Retention (pocet):\t", 999);
            _retSizeNumberBox = new NumberBox("Retention (velikost):\t", 9999);

            _saveButton = new Button("Ulozit do JSON", true);
            _cancelButton = new Button("Ukoncit", true);

            // 2. Registrace do okna (aby fungoval Tab a šipky)
            RegisterComponent(_sourceTextBox);
            RegisterComponent(_targetTextBox);
            RegisterComponent(_timingTextBox);
            RegisterComponent(_methodSelectBox);
            RegisterComponent(_retCountNumberBox);
            RegisterComponent(_retSizeNumberBox);
            RegisterComponent(new EmptyLine());
            RegisterComponent(_saveButton);
            RegisterComponent(_cancelButton);

            // 3. Přiřazení akcí tlačítkům
            _saveButton.Clicked += SaveButtonClicked;
            _cancelButton.Clicked += CancelButtonClicked;
        }

        private void SaveButtonClicked()
        {
            try
            {
                // --- BONUS: Validace CRONu ---
                // Vezmeme text a odstraníme mezery na začátku a na konci
                string cronText = _timingTextBox.Value.Trim();

                // Rozdělíme text podle mezer. RemoveEmptyEntries zajistí, že více mezer za sebou neudělá chybu.
                string[] cronParts = cronText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Kontrola: Má to přesně 5 částí?
                if (cronParts.Length != 5)
                {
                    // Vyhodíme chybové okno (ErrorWindow pochází z předchozího projektu)
                    IWindow errorWindow = new ErrorWindow("Chyba validace", "CRON musi mit presne 5 casti (napr. '5 4 * * *').", _application, this);
                    errorWindow.Show();

                    // Příkaz return okamžitě ukončí tuto metodu, takže se nic do JSONu neuloží!
                    return;
                }
                // -----------------------------

                // Pokud kód došel až sem, CRON je validní a můžeme ukládat
                BackupJob novyJob = new BackupJob
                {
                    Sources = new List<string> { _sourceTextBox.Value },
                    Targets = new List<string> { _targetTextBox.Value },
                    Timing = cronText, // Použijeme náš zkontrolovaný text
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

                // Ukončíme aplikaci po úspěšném uložení
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
            // ZMĚNA: Tlačítko storno nyní rovnou vypne aplikaci
            _application.Stop();
        }
    }
}