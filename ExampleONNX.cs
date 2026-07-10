using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace Supertonic
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Por favor, provee el texto a sintetizar como argumento.");
                return;
            }

            int totalStep = 50;
            float speed = 1.3f;
            string voice = "M2";
            List<string> textParts = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--steps" && i + 1 < args.Length)
                {
                    int.TryParse(args[++i], out totalStep);
                }
                else if (args[i] == "--speed" && i + 1 < args.Length)
                {
                    float.TryParse(args[++i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out speed);
                }
                else if (args[i] == "--voice" && i + 1 < args.Length)
                {
                    voice = args[++i];
                }
                else
                {
                    textParts.Add(args[i]);
                }
            }

            string texto = string.Join(" ", textParts);
            if (string.IsNullOrWhiteSpace(texto)) return;

            Console.WriteLine($"Generando TTS para: {texto} (Pasos: {totalStep}, Velocidad: {speed})");

            // --- Parámetros fijos --- //
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string onnxDir = Path.Combine(baseDir, "assets", "onnx");
            string saveDir = Path.Combine(baseDir, "results");
            bool useGpu = false;

            string voiceFile = voice.EndsWith(".json") ? voice : $"{voice}.json";
            var voiceStylePaths = new List<string> { Path.Combine(baseDir, "assets", "voice_styles", voiceFile) };
            var langList = new List<string> { "es" };
            var textList = new List<string> { texto };

            // --- Cargar Text to Speech --- //
            var textToSpeech = Helper.LoadTextToSpeech(onnxDir, useGpu);

            // --- Cargar estilo de voz --- //
            var style = Helper.LoadVoiceStyle(voiceStylePaths, verbose: false);

            var (wav, duration) = Helper.Timer("Generando voz desde texto", () =>
                textToSpeech.Call(textList[0], langList[0], style, totalStep, speed)
            );

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            string fname = $"{Helper.SanitizeFilename(textList[0], 20)}.wav";
            int wavLen = (int)(textToSpeech.SampleRate * duration[0]);
            var wavOut = new float[wavLen];

            Array.Copy(wav, 0, wavOut, 0, Math.Min(wavLen, wav.Length));

            string outputPath = Path.Combine(saveDir, fname);
            Helper.WriteWavFile(outputPath, wavOut, textToSpeech.SampleRate);
            
            // Reproducir el archivo generado
            try
            {
                Console.WriteLine($"Reproduciendo: {outputPath}");
                using (var player = new SoundPlayer(outputPath))
                {
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reproducir el audio: {ex.Message}");
            }
        }
    }
}