using System;
using System.Windows.Forms;
using NAudio.Wave;

public class GameSound : IDisposable
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public void PlayMusic(string filePath)
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            outputDevice.Init(audioFile);
            outputDevice.PlaybackStopped += (sender, args) =>
            {
                // Reinicia a reprodução quando atinge o fim
                if (outputDevice != null && audioFile != null)
                {
                    audioFile.Position = 0;
                    outputDevice.Play();
                }
            };

            outputDevice.Play();
        }
    }

    public void StopMusic()
    {
        outputDevice?.Stop();
    }

    public void Dispose()
    {
        if (outputDevice != null)
        {
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
        }

        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }
    }
}