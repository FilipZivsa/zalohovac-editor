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
                // Vytvoření objektu zálohy z vyplněných polí
                BackupJob novyJob = new BackupJob
                {
                    Sources = new List<string> { _sourceTextBox.Value },
                    Targets = new List<string> { _targetTextBox.Value },
                    Timing = _timingTextBox.Value,
                    Method = _methodSelectBox.Value,
                    Retention = new BackupRetention
                    {
                        Count = _retCountNumberBox.Value,
                        Size = _retSizeNumberBox.Value
                    }
                };

                List<BackupJob> konfigurace = new List<BackupJob> { novyJob };

                // Nastavení JSONu (hezky odřádkované a s malými písmeny na začátku názvů)
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

                // Uložení
                string jsonString = JsonSerializer.Serialize(konfigurace, options);
                File.WriteAllText("config.json", jsonString);

                // ZMĚNA: Ukončíme aplikaci po úspěšném uložení
                _application.Stop();
            }
            catch (Exception ex)
            {
                // ZMĚNA: Pokud nastane chyba, vypíšeme ji a pak okno zavřeme
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