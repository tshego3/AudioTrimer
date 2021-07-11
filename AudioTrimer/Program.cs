using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTrimer
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan startTime = new TimeSpan(00, 00, 30); // Cuts the first 30 seconds of the audio file. 0,3 (No Math calculation).
            TimeSpan endTime = new TimeSpan(00, 00, 84); // Cuts the last 84 seconds of the audio file. For example 2,54 - 1,24 = 1,30 and 1,30 - 0,30 = 1,00 making it a 1 minute audio file.
            TrimWavFile("C:\\Users\\Dev Tshego\\Music\\Zuks - Oh God w_Ceazor.wav", "Zuks - Oh God w_Ceazor - Copy.wav", startTime, endTime);
            Console.WriteLine($"Start Total Seconds: {startTime.TotalSeconds} AND End Total Seconds: {endTime.TotalSeconds}");
            Console.ReadLine();
        }

        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }

        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            //byte[] buffer = new byte[endPos-startPos];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
