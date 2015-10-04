﻿using System;
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

namespace clienteMail.comando
{
    public partial class comando : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        IntPtr autenticador;

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
            comandos.Add(new string[] { "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "contactos", "asuntos", "mensajes", "recibidos", "enviados", "eliminar", "leer", "redactar", "anterior", "siguiente", "volver", "aceptar", "para", "enviar", "cancelar" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(comandos);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            // int rv = AV.avf_crear_autenticador("osvaldo.pav", out autenticador);
            // if (rv != 0) MessageBox.Show(rv.ToString());
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
                case "volver":
                    richTextBox1.Text += "\nVolver";
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
            }

            RecognizedAudio audio = e.Result.Audio;
            TimeSpan duration = audio.Duration;
            // ATENCION!! PARA QUE ANDE CREAR LA CARPETA temporalproyecto en el disco C
            string path = @"C:\temporalproyecto\nameAudio.wav";
            using (Stream outputStream = new FileStream(path, FileMode.Create))
            {
                RecognizedAudio nameAudio = audio;
                nameAudio.WriteToWaveStream(outputStream);
                outputStream.Close();
            }
            string nombre_archivo = "C:\\temporalproyecto\\nameAudio.wav";
            int res = AV.avf_autenticar_WAV(autenticador, nombre_archivo);
            if (res < -10000)
            {
                richTextBox1.Text += " - error - " + res.ToString();
                return;
            }
            if (res > 0)
            {
                richTextBox1.Text += " - Autenticado - " + (((double)res) / 100).ToString("0.00") + "%";
                G.formulario_activo.manejar_comando(e.Result.Text);
            }
            else
            {
                res = -res;
                richTextBox1.Text += " - Acceso denegado - " + (((double)res) / 100).ToString("0.00") + "%";
            }

        }

        private void disableBtn_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            disableBtn.Enabled = false;
        }
    }
}