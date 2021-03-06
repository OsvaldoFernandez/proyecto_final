﻿﻿using System;
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
            Choices comandos = new Choices();
            comandos.Add(new string[] {
              "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "contactos", "asuntos",
              "mensajes", "recibidos", "enviados", "eliminar", "actualizar", "redactar", "anterior", "siguiente",
              "aceptar", "para", "enviar", "cerrar", "cancelar", "responder", "reenviar", "leer", "cerrar sesión"
            });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(comandos);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            if ((Form.ActiveForm.Name == "crear_cuenta") || (Form.ActiveForm.Name == "entrenamiento_1")) return;
            int rv;
            if (G.user.PAV != null)
              rv = AV.avf_crear_autenticador("perfiles\\" + G.user.PAV, out autenticador);
            else
              rv = AV.AVS_PUNTERO_NULO;
            if ((rv != 0) && (rv != AV.AVS_PUNTERO_NULO)) {
              MessageBox.Show("Hubo un error cargando su perfil de autenticación de voz. Por favor, cree la cuenta nuevamente." +
                              " (0x" + rv.ToString("X8") + ")", "Error cargando cuenta", MessageBoxButtons.OK, MessageBoxIcon.Error);
              Environment.Exit(1);
            }
        }

        public override void actualizar_estado_microfono(bool estado)
        {
            if (estado) {
                recEngine.RecognizeAsync(RecognizeMode.Multiple);
                disableBtn.Enabled = true;
            } else {
                recEngine.RecognizeAsyncStop();
                disableBtn.Enabled = false;
            }
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            disableBtn.Enabled = true;
        }

        private void comando_Load(object sender, EventArgs e)
        {
            #if !DEBUG
                Visible = false;
            #endif
        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            RichForm currentForm = (RichForm)Form.ActiveForm;
            if (currentForm == null) return;
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
            if (e.Result.Text != null) richTextBox1.Text += Environment.NewLine + char.ToUpperInvariant(e.Result.Text[0]) + e.Result.Text.Substring(1);
            #if DEBUG
                Console.WriteLine(e.Result.Confidence.ToString());
            #endif
            float confidence = e.Result.Confidence * 100;
            richTextBox1.Text += string.Format(" ({0:0.00}%)", e.Result.Confidence * 100);

            if (currentForm.Name == "entrenamiento_1")
                // Esta en modo entrenamiento, no autenticar y mandar el SpeechRecognized entero
                currentForm.manejar_comando_entrenamiento(e);
            else
            {
                string path = Path.GetTempFileName();
                Stream outputStream = new FileStream(path, FileMode.Create);
                e.Result.Audio.WriteToWaveStream(outputStream);
                outputStream.Close();
                int res = AV.avf_autenticar_WAV(autenticador, path);
                File.Delete(path);

                if (confidence > G.sensibilidad)
                {
                    G.registrar_comando(e.Result.Text, e.Result.Confidence, (double) res / 10000.0);
                    G.confianza_autenticacion = res;
                    if (res == AV.AVS_PUNTERO_NULO) {
                      currentForm.manejar_comando(e.Result.Text);
                      return;
                    } else if (res < -10000) {
                        richTextBox1.Text += " - error - " + res.ToString("X");
                        currentForm.manejar_comando(e.Result.Text);
                        return;
                    }
                    if (res > 0) {
                        richTextBox1.Text += " - Autenticado - " + (((double)res) / 100).ToString("0.00") + "%";
                        currentForm.manejar_comando(e.Result.Text);
                    } else { 
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
            RichForm form1 = new entrenamiento.entrenamiento_1("perfil.pav");
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