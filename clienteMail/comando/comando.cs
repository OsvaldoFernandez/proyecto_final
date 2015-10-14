using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace clienteMail.comando
{
    public partial class comando : RichForm
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        IntPtr autenticador;

        public comando()
        {
            InitializeComponent();
#if !DEBUG
            Visible = false;
#endif
            
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            disableBtn.Enabled = true;
        }

        public override void manejar_comando(string comando)
        {
           //NADA
        }

        private void comando_Load(object sender, EventArgs e)
        {
            Choices comandos = new Choices();
            comandos.Add(new string[] { "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "contactos", "asuntos", "mensajes", "recibidos", "enviados", "eliminar", "actualizar", "redactar", "anterior", "siguiente", "aceptar", "para", "enviar", "cerrar", "cancelar", "responder", "reenviar" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(comandos);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            int rv = AV.avf_crear_autenticador("osvaldo.pav", out autenticador);
            if (rv != 0) MessageBox.Show(rv.ToString("X"));
        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            RichForm currentForm = (RichForm)Form.ActiveForm;
            txtFormActivo.Text += currentForm.Text + '\n';
            int pos = -1;
            for (int linea = 0; linea < 15; linea++)
            {
                if (pos < 0)
                    pos = richTextBox1.Text.LastIndexOf('\n');
                else if (pos > 0)
                    pos = richTextBox1.Text.LastIndexOf('\n', pos - 1);
                if (pos < 0) break;
            }
            if (pos > 0) richTextBox1.Text = richTextBox1.Text.Substring(pos + 1);
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
                case "contactos":
                    richTextBox1.Text += "\nContactos";
                    break;
                case "asuntos":
                    richTextBox1.Text += "\nAsuntos";
                    break;
                case "mensajes":
                    richTextBox1.Text += "\nMensajes";
                    break;
                case "recibidos":
                    richTextBox1.Text += "\nRecibidos";
                    break;
                case "enviados":
                    richTextBox1.Text += "\nEnviados";
                    break;
                case "eliminar":
                    richTextBox1.Text += "\nEliminar";
                    break;
                case "leer":
                    richTextBox1.Text += "\nLeer";
                    break;
                case "redactar":
                    richTextBox1.Text += "\nRedactar";
                    break;
                case "anterior":
                    richTextBox1.Text += "\nAnterior";
                    break;
                case "siguiente":
                    richTextBox1.Text += "\nSiguiente";
                    break;
                case "cerrar":
                    richTextBox1.Text += "\nCerrar";
                    break;
                case "aceptar":
                    richTextBox1.Text += "\nAceptar";
                    break;
                case "para":
                    richTextBox1.Text += "\nPara";
                    break;
                case "enviar":
                    richTextBox1.Text += "\nEnviar";
                    break;
                case "cancelar":
                    richTextBox1.Text += "\nCancelar";
                    break;
                case "actualizar":
                    richTextBox1.Text += "\nActualizar";
                    break;
            }

            Console.WriteLine(e.Result.Confidence.ToString());

            float confidence = e.Result.Confidence * 100;
            richTextBox1.Text += string.Format(" ({0:0.00}%)", e.Result.Confidence * 100);

            if (currentForm.Name == "entrenamiento_1")
            //Está en modo entrenamiento, no autenticar y mandar el SpeechRecognized entero
            {
                currentForm.manejar_comando_entrenamiento(e);
            }
            else
            {
                // filtrar por e.Result.Confidence
                RecognizedAudio audio = e.Result.Audio;
                TimeSpan duration = audio.Duration;
                // Osvaldo hace cosas muy raras
                string path = Path.GetTempFileName();
                using (Stream outputStream = new FileStream(path, FileMode.Create))
                {
                    RecognizedAudio nameAudio = audio;
                    nameAudio.WriteToWaveStream(outputStream);
                    outputStream.Close();
                }
                int res = AV.avf_autenticar_WAV(autenticador, path);
                File.Delete(path);


                if (confidence > G.sensibilidad)
                {
                    if (res < -10000)
                    {
                        richTextBox1.Text += " - error - " + res.ToString("X");
                        currentForm.manejar_comando(e.Result.Text);
                        return;
                    }
                    if (res > 0)
                    {
                        richTextBox1.Text += " - Autenticado - " + (((double)res) / 100).ToString("0.00") + "%";
                        currentForm.manejar_comando(e.Result.Text);
                    }
                    else
                    {
                        res = -res;
                        richTextBox1.Text += " - Acceso denegado - " + (((double)res) / 100).ToString("0.00") + "%";
                        currentForm.manejar_comando(e.Result.Text);
                    }
                }
            }



        }

        private void disableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            disableBtn.Enabled = false;
        }

        private void btnActualizarSensibilidad_Click(object sender, EventArgs e)
        {
            G.sensibilidad = Convert.ToInt32(txtSensibilidad.Text.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RichForm form1 = new entrenamiento.entrenamiento_1();
            form1.Show();

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (disableBtn.Enabled)
            {
                recEngine.RecognizeAsyncStop();
                disableBtn.Enabled = false;
            }
        }

    }
}