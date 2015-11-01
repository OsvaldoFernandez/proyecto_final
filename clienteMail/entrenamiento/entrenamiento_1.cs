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
        IntPtr entrenador = IntPtr.Zero;
        string siguiente_comando = null;
        int fila = -1;
        private string perfil;

        public entrenamiento_1(string perfil)
        {
            InitializeComponent();
            int rv = AV.avf_crear_entrenador(out entrenador);
            if (rv == AV.AVS_SIN_MEMORIA) {
              MessageBox.Show("No hay memoria disponible, por favor cierre algunas aplicaciones y reinteéntelo más tarde", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
              throw new OutOfMemoryException();
            }
            if (rv != 0) throw new InvalidOperationException();
            this.perfil = perfil;
        }

        ~ entrenamiento_1 () {
          AV.avf_destruir_entrenador(entrenador);
        }

        private void entrenamiento_1_Load(object sender, EventArgs e)
        {
            int iteracion;
            for (iteracion = 1; iteracion <= 3; iteracion ++) {
              dataGridView1.Rows.Add("Uno", "pendiente", "1_" + iteracion.ToString());
              dataGridView1.Rows.Add("Dos", "pendiente", "2_" + iteracion.ToString());
              dataGridView1.Rows.Add("Tres", "pendiente", "3_" + iteracion.ToString());
              dataGridView1.Rows.Add("Cuatro", "pendiente", "4_" + iteracion.ToString());
              dataGridView1.Rows.Add("Cinco", "pendiente", "5_" + iteracion.ToString());
              dataGridView1.Rows.Add("Seis", "pendiente", "6_" + iteracion.ToString());
              dataGridView1.Rows.Add("Siete", "pendiente", "7_" + iteracion.ToString());
              dataGridView1.Rows.Add("Ocho", "pendiente", "8_" + iteracion.ToString());
              dataGridView1.Rows.Add("Nueve", "pendiente", "9_" + iteracion.ToString());
              dataGridView1.Rows.Add("Contactos", "pendiente", "con_" + iteracion.ToString());
              dataGridView1.Rows.Add("Asuntos", "pendiente", "asu_" + iteracion.ToString());
              dataGridView1.Rows.Add("Mensajes", "pendiente", "men_" + iteracion.ToString());
              dataGridView1.Rows.Add("Recibidos", "pendiente", "rec_" + iteracion.ToString());
              dataGridView1.Rows.Add("Enviados", "pendiente", "env_" + iteracion.ToString());
              dataGridView1.Rows.Add("Eliminar", "pendiente", "eli_" + iteracion.ToString());
              dataGridView1.Rows.Add("Actualizar", "pendiente", "act_" + iteracion.ToString());
              dataGridView1.Rows.Add("Redactar", "pendiente", "red_" + iteracion.ToString());
              dataGridView1.Rows.Add("Anterior", "pendiente", "ant_" + iteracion.ToString());
              dataGridView1.Rows.Add("Siguiente", "pendiente", "sig_" + iteracion.ToString());
              dataGridView1.Rows.Add("Aceptar", "pendiente", "ace_" + iteracion.ToString());
              dataGridView1.Rows.Add("Para", "pendiente", "par_" + iteracion.ToString());
              dataGridView1.Rows.Add("Enviar", "pendiente", "env_" + iteracion.ToString());
              dataGridView1.Rows.Add("Cerrar", "pendiente", "cer_" + iteracion.ToString());
              dataGridView1.Rows.Add("Cancelar", "pendiente", "can_" + iteracion.ToString());
              dataGridView1.Rows.Add("Responder", "pendiente", "res_" + iteracion.ToString());
              dataGridView1.Rows.Add("Reenviar", "pendiente", "ree_" + iteracion.ToString());
            }
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            label2.Visible = false;
            btnRecibidos.Visible = false;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[0].Selected = true;
            siguiente_comando = dataGridView1[0, 0].Value.ToString().ToUpperInvariant();
            fila = 0;
        }

        public override void manejar_comando_entrenamiento(SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.ToUpperInvariant() == siguiente_comando)
            {

                RecognizedAudio audio = e.Result.Audio;
                TimeSpan duration = audio.Duration;
                int resultado;
                
                string path = Path.GetTempFileName();

                using (Stream outputStream = new FileStream(path, FileMode.Create))
                {
                    RecognizedAudio nameAudio = audio;
                    nameAudio.WriteToWaveStream(outputStream);
                    outputStream.Close();
                }

                resultado = AV.avf_agregar_muestra_WAV(entrenador, 0,
                  (dataGridView1[2, fila].Value.ToString().Split('_')[1] == "3") ? AV.AVP_MUESTRA_VALIDACION :
                  (AV.AVP_MUESTRA_ENTRENAMIENTO | AV.AVP_MUESTRA_VALIDACION), path);

                #if DEBUG
                  File.Copy(path, dataGridView1[2, fila].Value.ToString() + ".wav", true);
                #endif
                File.Delete(path);

                switch (resultado) {
                  case AV.AVS_SIN_MEMORIA:
                    Environment.Exit(1);
                    return;
                  case AV.AVS_FORMATO_ARCHIVO_NO_VALIDO:
                    MessageBox.Show("La grabación está dañada. Por favor, reintente la operación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                  case AV.AVS_ARCHIVO_INACCESIBLE:
                    MessageBox.Show("No se pudo acceder a la voz grabada. Por favor, verifique que se pueda escribir en el disco y reintente la operación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                  case AV.AVS_MUESTREO_DEMASIADO_BAJO: case AV.AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ:
                    MessageBox.Show("La grabación no puede ser utilizada por la aplicación. Por favor, utilice otro micrófono y reinicie el proceso de entrenamiento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                  case AV.AVS_DURACION_MENOR_A_MEDIO_SEGUNDO:
                    MessageBox.Show("La grabación es demasiado corta. Se necesita una grabación de al menos medio segundo. Por favor, grabe el comando nuevamente, hablando lento y claro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                  default:
                    if (resultado >= 0) break;
                    MessageBox.Show("Ocurrió un error inesperado, por favor reintente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                           
                dataGridView1[1, fila].Value = "Reconocido";
                dataGridView1.ClearSelection();
                if (dataGridView1.RowCount == (fila + 1))
                {
                    lblTitle.Text = "Entrenando";
                    label1.Text = "El sistema se está entrenando para reconocer tu voz";
                    label2.Visible = true;
                    label2.Text = "La operación tardará aproximadamente 20 minutos";
                    dataGridView1.Visible = false;
                    cafe.Visible = true;
                    G.comando_form.Close();
                    entrenar();
                }
                else
                {
                    dataGridView1.Rows[++ fila].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = fila;
                    siguiente_comando = dataGridView1[0, fila].Value.ToString().ToUpperInvariant();
                }
            }

        }

        void entrenar () {
          int rv, persona = 0;
          label2.Text = "La operación tardará aproximadamente 20 minutos. Preparando...";
          Application.DoEvents();
          foreach (string subdir in Directory.EnumerateDirectories("aud")) {
            persona ++;
            foreach (string archivo in Directory.EnumerateFiles(subdir, "*.wav")) {
              rv = AV.avf_agregar_muestra_WAV(entrenador, persona, archivo.Contains("_3") ? AV.AVP_MUESTRA_VALIDACION :
                   (AV.AVP_MUESTRA_ENTRENAMIENTO | AV.AVP_MUESTRA_VALIDACION), archivo);
              if (rv < 0) {
                MessageBox.Show("ERROR!!!!!!!!!!!!!!");
                return;
              }
              Application.DoEvents();
            }
          }
          bool continuar;
          uint limite_tiempo = 0;
          do {
            limite_tiempo += 1200000; // 20 min
            AV.avf_iniciar_entrenamiento(entrenador);
            AV.avt_estado estado;
            while (true) {
              Application.DoEvents();
              System.Threading.Thread.Sleep(250);
              rv = AV.avf_estado_entrenamiento(entrenador, out estado);
              if (rv != 0) continue;
              uint segundos_restantes = (limite_tiempo - estado.tiempo_transcurrido) / 1000;
              if (segundos_restantes > 9999999u) segundos_restantes = 0;
              uint minutos_restantes = segundos_restantes / 60;
              segundos_restantes %= 60;
              label2.Text = "La operación tardará aproximadamente 20 minutos. Restan " + minutos_restantes.ToString() + ":" + segundos_restantes.ToString("00");
              if (estado.redes_satisfactorias >= 8) break;
              if (estado.tiempo_transcurrido > limite_tiempo) break;
            }
            AV.avf_detener_entrenamiento(entrenador);
            rv = AV.avf_exportar_entrenamiento(entrenador, "perfiles\\" + perfil);
            switch (rv) {
              case AV.AVS_SIN_MEMORIA:
                Environment.Exit(1);
                return;
              case AV.AVS_ARCHIVO_INACCESIBLE: case AV.AVS_FALLO_ESCRITURA_ARCHIVO:
                MessageBox.Show("No se pudo generar el archivo de perfil", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
              case AV.AVS_NADA_PARA_EXPORTAR:
                continuar = true;
                break;
              default:
                continuar = false;
                break;
            }
            if (continuar)
              continuar = MessageBox.Show("El entrenamiento no generó suficiente información. ¿Desea continuar por 20 minutos más?", "Falta información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes;
          } while (continuar);
          AV.avf_destruir_entrenador(entrenador);
          entrenador = IntPtr.Zero;
          this.Close();
        }
    }
}
