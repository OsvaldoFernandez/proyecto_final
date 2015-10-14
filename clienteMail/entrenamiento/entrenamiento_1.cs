using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Windows.Forms;

namespace clienteMail.entrenamiento
{
    public partial class entrenamiento_1 : RichForm
    {
        public entrenamiento_1()
        {
            InitializeComponent();
        }

        private void entrenamiento_1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("Uno", "pendiente", "1_1");
            dataGridView1.Rows.Add("Dos", "pendiente", "2_1");
            dataGridView1.Rows.Add("Tres", "pendiente", "3_1");
            dataGridView1.Rows.Add("Cuatro", "pendiente", "4_1");
            dataGridView1.Rows.Add("Cinco", "pendiente", "5_1");
            dataGridView1.Rows.Add("Seis", "pendiente", "6_1");
            dataGridView1.Rows.Add("Siete", "pendiente", "7_1");
            dataGridView1.Rows.Add("Ocho", "pendiente", "8_1");
            dataGridView1.Rows.Add("Nueve", "pendiente", "9_1");
            dataGridView1.Rows.Add("Uno", "pendiente", "1_2");
            dataGridView1.Rows.Add("Dos", "pendiente", "2_2");
            dataGridView1.Rows.Add("Tres", "pendiente", "3_2");
            dataGridView1.Rows.Add("Cuatro", "pendiente", "4_2");
            dataGridView1.Rows.Add("Cinco", "pendiente", "5_2");
            dataGridView1.Rows.Add("Seis", "pendiente", "6_2");
            dataGridView1.Rows.Add("Siete", "pendiente", "7_2");
            dataGridView1.Rows.Add("Ocho", "pendiente", "8_2");
            dataGridView1.Rows.Add("Nueve", "pendiente", "9_2");
            dataGridView1.Rows.Add("Uno", "pendiente", "1_3");
            dataGridView1.Rows.Add("Dos", "pendiente", "2_3");
            dataGridView1.Rows.Add("Tres", "pendiente", "3_3");
            dataGridView1.Rows.Add("Cuatro", "pendiente", "4_3");
            dataGridView1.Rows.Add("Cinco", "pendiente", "5_3");
            dataGridView1.Rows.Add("Seis", "pendiente", "6_3");
            dataGridView1.Rows.Add("Siete", "pendiente", "7_3");
            dataGridView1.Rows.Add("Ocho", "pendiente", "8_3");
            dataGridView1.Rows.Add("Nueve", "pendiente", "9_3");
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            label2.Visible = false;
            btnRecibidos.Visible = false;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[0].Selected = true;
        }

        public override void manejar_comando_entrenamiento(SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.ToUpper() == (dataGridView1.SelectedRows[0].Cells[0].Value.ToString().ToUpper()))
            {

                RecognizedAudio audio = e.Result.Audio;
                TimeSpan duration = audio.Duration;
                
                string path = dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + ".wav";
                // Los archivos de audio se guardan por su nombre código en la carpeta DEBUG
                // Yo ahora lo guardo en la carpeta, pero acá deberíamos agregar la muestra al entrenador, así si nos da error "WAV MENOR A MEDIO SEGUNDO" hacemos un "return" y no lo damos por reconocido.
                
                using (Stream outputStream = new FileStream(path, FileMode.Create))
                {
                    RecognizedAudio nameAudio = audio;
                    nameAudio.WriteToWaveStream(outputStream);
                    outputStream.Close();
                }
                           
                dataGridView1.SelectedRows[0].Cells[1].Value = "Reconocido";
                int index = dataGridView1.SelectedRows[0].Index;
                dataGridView1.ClearSelection();
                if (dataGridView1.RowCount == (index + 1))
                {
                    lblTitle.Text = "Entrenando";
                    label1.Text = "El sistema se está entrenando para reconocer tu voz";
                    label2.Visible = true;
                    label2.Text = "La operación tardará aproximadamente 20 minutos";
                    dataGridView1.Visible = false;
                    cafe.Visible = true;
                    G.comando_form.Close();
                    // Pendiente
                    // EMPEZAR ENTRENAMIENTO ACÁ
                }
                else
                {
                    dataGridView1.Rows[index + 1].Selected = true;
                }
            }

        }
    }
}
