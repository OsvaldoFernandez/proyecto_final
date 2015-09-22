using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace clienteMail.comando
{
    public partial class comando : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();

        public comando()
        {
            InitializeComponent();
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            disableBtn.Enabled = true;
        }

        private void comando_Load(object sender, EventArgs e)
        {
            Choices comandos = new Choices();
            comandos.Add(new string[] { "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(comandos);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "uno":
                    richTextBox1.Text += "\nUno";
                    break;
                case "dos":
                    richTextBox1.Text += "\nDos";
                    break;
                case "tres":
                    richTextBox1.Text += "\nTres";
                    break;
                case "cuatro":
                    richTextBox1.Text += "\nCuatro";
                    break;
                case "cinco":
                    richTextBox1.Text += "\nCinco";
                    break;
                case "seis":
                    richTextBox1.Text += "\nSeis";
                    break;
                case "siete":
                    richTextBox1.Text += "\nSiete";
                    break;
                case "ocho":
                    richTextBox1.Text += "\nOcho";
                    break;
                case "nueve":
                    richTextBox1.Text += "\nNueve";
                    break;
            }
            G.formulario_activo.manejar_comando(e.Result.Text);

        }

        private void disableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            disableBtn.Enabled = false;
        }
    }
}