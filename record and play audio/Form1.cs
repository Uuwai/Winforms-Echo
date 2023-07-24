using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace record_and_play_audio
{
    public partial class Form1 : Form
    {
        private WasapiLoopbackCapture loopbackCapture;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider buffer;

        private void InitializeAudioComponents()
        {
            loopbackCapture = new WasapiLoopbackCapture();

            waveOut = new WaveOutEvent();

            buffer = new BufferedWaveProvider(loopbackCapture.WaveFormat);

            loopbackCapture.DataAvailable += (sender, e) =>
            {
                buffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            };
        }
        public Form1()
        {
            InitializeComponent();
            InitializeAudioComponents();
        }
        private void StartRecordingAndPlayback()
        {
            loopbackCapture.StartRecording();

            var volumeSampleProvider = new VolumeSampleProvider(buffer.ToSampleProvider());
            volumeSampleProvider.Volume = (float)numericUpDown1.Value;
            waveOut.Init(volumeSampleProvider);
            waveOut.Play();
        }

        private void StopRecordingAndPlayback()
        {
            loopbackCapture.StopRecording();

            waveOut.Stop();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            loopbackCapture.StopRecording();

            waveOut.Stop();
        }
        bool enabled = false;
        private void button2_Click(object sender, EventArgs e)
        {
            StopRecordingAndPlayback();
            button1.Text = "Start";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (enabled == false)
            {
                StartRecordingAndPlayback();
                enabled = true;
                button1.Text = "Stop";
            }
            else
            {
                StopRecordingAndPlayback();
                enabled = false;
                button1.Text = "Start";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0.25m;
        }
    }
}
