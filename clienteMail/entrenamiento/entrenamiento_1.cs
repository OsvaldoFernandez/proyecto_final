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
using System.Data.SQLite;

namespace clienteMail.entrenamiento
{
    public partial class entrenamiento_1 : RichForm
    {
        public readonly int minutos_entrenamiento = 20;
        public readonly int segundos_entrenamiento = 0;

        IntPtr entrenador = IntPtr.Zero;
        string siguiente_comando = null;
        int fila = -1;
        private string perfil;

        public entrenamiento_1(string perfil)
        {
            InitializeComponent();
            int rv = AV.avf_crear_entrenador(out entrenador);
            if (rv == AV.AVS_SIN_MEMORIA) {
                errorlabel.Text = "No hay memoria disponible, por favor cierre \nalgunas aplicaciones y reinteéntelo más tarde";
                errorlabel.Visible = true;
                errorpanel.Visible = true;
                throw new OutOfMemoryException();
            }
            if (rv != 0) throw new InvalidOperationException();
            this.perfil = perfil;
            G.crear_form_comando();
            #if DEBUG
              G.comando_form.Show();
            #endif
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
            pausaBtn.Visible = true;
            G.comando_form.actualizar_estado_microfono(true);
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
                    errorlabel.Text = "La grabación está dañada. \nPor favor, reintente la operación.";
                    errorlabel.Visible = true;
                    errorpanel.Visible = true;
                    return;
                  case AV.AVS_ARCHIVO_INACCESIBLE:
                    errorlabel.Text = "No se pudo acceder a la voz grabada. \nPor favor, verifique que se pueda escribir en el disco \ny reintente la operación.";
                    errorlabel.Visible = true;
                    errorpanel.Visible = true;
                    return;
                  case AV.AVS_MUESTREO_DEMASIADO_BAJO: case AV.AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ:
                    errorlabel.Text = "La grabación no puede ser utilizada por la aplicación. \n Por favor, utilice otro micrófono y \nreinicie el proceso de entrenamiento.";
                    errorlabel.Visible = true;
                    errorpanel.Visible = true;
                    return;
                  case AV.AVS_DURACION_MENOR_A_MEDIO_SEGUNDO:
                    errorlabel.Text = "La grabación es demasiado corta. \nSe necesita una grabación de al menos medio segundo. \nPor favor, grabe el comando nuevamente, hablando lento y claro.";
                    errorlabel.Visible = true;
                    errorpanel.Visible = true;
                    return;
                  default:
                    if (resultado >= 0) break;
                    errorlabel.Text = "Ocurrió un error inesperado, por favor reintente.";
                    errorlabel.Visible = true;
                    errorpanel.Visible = true;
                    return;
                }

                errorlabel.Visible = false;
                errorpanel.Visible = false;
                           
                dataGridView1[1, fila].Value = "Reconocido";
                dataGridView1.ClearSelection();
                if (dataGridView1.RowCount == (fila + 1))
                {
                    lblTitle.Text = "Entrenando";
                    label1.Text = "El sistema se está entrenando para reconocer tu voz";
                    label2.Visible = true;
                    label2.Text = "La operación tardará aproximadamente 20 minutos";
                    dataGridView1.Visible = false;
                    pausaBtn.Visible = false;
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
          label2.Text = "La operación tardará aproximadamente " + minutos_entrenamiento.ToString() + 
                            ((segundos_entrenamiento == 0) ? "" : (":" + segundos_entrenamiento.ToString("00"))) +
                            " minutos. Preparando...";
          G.comando_form.actualizar_estado_microfono(false);
          Application.DoEvents();
          foreach (string subdir in Directory.EnumerateDirectories("aud")) {
            persona ++;
            foreach (string archivo in Directory.EnumerateFiles(subdir, "*.wav")) {
              rv = AV.avf_agregar_muestra_WAV(entrenador, persona, archivo.Contains("_3") ? AV.AVP_MUESTRA_VALIDACION :
                   (AV.AVP_MUESTRA_ENTRENAMIENTO | AV.AVP_MUESTRA_VALIDACION), archivo);
              if (rv < 0) {
                errorlabel.Text = "Ocurrio un error mientras se reconocían sus comandos \nPor favor, reinicie el entrenamiento";
                errorlabel.Visible = true;
                errorpanel.Visible = true;
                return;
              }
              Application.DoEvents();
            }
          }
          bool continuar;
          uint limite_tiempo = 0;
          do {
            limite_tiempo += (uint) ((60 * minutos_entrenamiento + segundos_entrenamiento) * 1000);
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
              label2.Text = "La operación tardará aproximadamente " + minutos_entrenamiento.ToString() + 
                            ((segundos_entrenamiento == 0) ? "" : (":" + segundos_entrenamiento.ToString("00"))) +
                            " minutos. Restan " + minutos_restantes.ToString() + ":" + segundos_restantes.ToString("00");
              if (estado.redes_satisfactorias >= 8) break;
              if (estado.tiempo_transcurrido > limite_tiempo) break;
            }
            AV.avf_detener_entrenamiento(entrenador);
            rv = AV.avf_exportar_entrenamiento(entrenador, perfil);
            switch (rv) {
              case AV.AVS_SIN_MEMORIA:
                Environment.Exit(1);
                return;
              case AV.AVS_ARCHIVO_INACCESIBLE: case AV.AVS_FALLO_ESCRITURA_ARCHIVO:
                errorlabel.Text = "No se pudo generar el archivo de perfil";
                errorlabel.Visible = true;
                errorpanel.Visible = true;
                return;
              case AV.AVS_NADA_PARA_EXPORTAR:
                continuar = true;
                break;
              default:
                // EXITOSO 
                continuar = false;
                // SETEARLE A MI USUARIO ACTUAL EL PERFIL QUE GENERÉ
                SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
                cmd.CommandText = "UPDATE Usuario SET perfil= ? WHERE id=? ";
                SQLiteParameter paramPerfil = new SQLiteParameter();
                cmd.Parameters.Add(paramPerfil);
                paramPerfil.Value = perfil;
                SQLiteParameter paramID = new SQLiteParameter();
                cmd.Parameters.Add(paramID);
                paramID.Value = G.user.ID;
                //ARRANCAR EL FORM1 COMO LO HARIA NORMALMENTE
                G.user.PAV = perfil;
                RichForm formulario_activo = new Form1();
                formulario_activo.Show();
                G.comando_form = new comando.comando();
                #if DEBUG
                  G.comando_form.Show();
                #endif
                break;
            }
            if (continuar)
              continuar = MessageBox.Show("El entrenamiento no generó suficiente información. ¿Desea continuar por "
                                          + minutos_entrenamiento.ToString() + 
                                          ((segundos_entrenamiento == 0) ? "" : (":" + segundos_entrenamiento.ToString("00"))) +
                                          " minutos más?", "Falta información", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                          ) == System.Windows.Forms.DialogResult.Yes;
          } while (continuar);
          AV.avf_destruir_entrenador(entrenador);
          entrenador = IntPtr.Zero;
          new Form1().Show();
          G.comando_form = new comando.comando();
          #if DEBUG
            G.comando_form.Show();
          #endif
          this.Close();
        }

        private void pausaBtn_Click(object sender, EventArgs e)
        {
            if (pausaBtn.Text == "Pausar") {
                pausaBtn.Text = "Reaundar";
                G.comando_form.actualizar_estado_microfono(false);
            } else {
                pausaBtn.Text = "Pausar";
                G.comando_form.actualizar_estado_microfono(true);
            }
                 
        }
    }
}
