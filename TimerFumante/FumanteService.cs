using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TimerFumante
{
    class FumanteService
    {
        private DateOnly hoje = DateOnly.FromDateTime(DateTime.Now);
        private int cigarrosFumados = 0;
        private List<DateTime> cigarrosFumadosTimes = new List<DateTime>();

        public FumanteService()
        {
            ImportCigarrosFumados();
        }

        public void IncrementCigarrosFumados()
        {
            this.cigarrosFumados++;
            this.cigarrosFumadosTimes.Add(DateTime.Now);
            SaveCigarrosFumados();
        }

        public void ImportCigarrosFumados()
        {
            string todayDate = this.hoje.ToString("yyyyMMdd");
            string fileName = $"{todayDate}.json";
            string directoryPath = Path.Combine(Environment.CurrentDirectory, "CigarrosFumadosHistory");
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent);

                if (data != null && data.ContainsKey("total") && data.ContainsKey("cigarrosFumadosTimes"))
                {
                    this.cigarrosFumados = JsonSerializer.Deserialize<int>(data["total"].ToString());

                    var timesList = JsonSerializer.Deserialize<List<string>>(data["cigarrosFumadosTimes"].ToString());
                    this.cigarrosFumadosTimes = new List<DateTime>();
                    foreach (var time in timesList)
                    {
                        if (DateTime.TryParseExact(time, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                        {
                            this.cigarrosFumadosTimes.Add(parsedTime);
                        }
                    }
                }
            }
        }

        public void SaveCigarrosFumados()
        {
            string todayDate = this.hoje.ToString("yyyyMMdd");
            string fileName = $"{todayDate}.json";
            string directoryPath = Path.Combine(Environment.CurrentDirectory, "CigarrosFumadosHistory");

            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, fileName);

            var data = new Dictionary<string, object>
            {
                { "total", this.cigarrosFumados },
                { "cigarrosFumadosTimes", FormatCigarrosFumadosTimes() }
            };

            string jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonContent);
        }

        private List<string> FormatCigarrosFumadosTimes()
        {
            List<string> formattedTimes = new List<string>();
            foreach (var time in cigarrosFumadosTimes)
            {
                formattedTimes.Add(time.ToString("HH:mm"));
            }
            return formattedTimes;
        }

        public DateOnly GetHoje()
        {
            return this.hoje;
        }

        public void SetHoje(DateOnly hoje)
        {
            this.hoje = hoje;
        }

        public int GetCigarrosFumados()
        {
            return this.cigarrosFumados;
        }

        public void SetCigarrosFumados(int cigarrosFumados)
        {
            this.cigarrosFumados = cigarrosFumados;
        }

        public List<DateTime> GetCigarrosFumadosTimes()
        {
            return this.cigarrosFumadosTimes;
        }
    }
}
