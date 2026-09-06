using System;
using System.Drawing;
using System.Windows.Forms;
using ReproductorMusica.Servicios;
using ReproductorMusica.Utilidades;
using ReproductorMusica.Modelo;

namespace IPC2_Practica2_202602_202500708
{
    public partial class Form1 : Form
    {
// Declaración de variables de la clase Form1
        private ReproductorService miServicio;
// Declaración de los elementos visuales de la ventana
        private Button btnCargarJson;
        private Button btnReproducir;
        private Label lblSonando;
        private Label lblTiempoTotal;
        private ListBox listaColaUI;
        private PictureBox picGraphvizCola;
        private PictureBox picGraphvizArbol;

        public Form1()
        {
            miServicio = new ReproductorService();
            ConstruirPantalla();
        }

// Método donde definimos coordenadas, tamaños y colores de los botones
        private void ConstruirPantalla()
        {
            // Configuración de la Ventana Principal
            this.Text = "Reproductor de Música - Práctica 2";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen; // Inicia en el centro
            this.BackColor = Color.WhiteSmoke;

            //Botón: Cargar Canciones
            btnCargarJson = new Button { Text = "Cargar JSON", Location = new Point(20, 20), Size = new Size(150, 40) };
            btnCargarJson.Click += BtnCargarJson_Click; // Le asignamos su acción
            this.Controls.Add(btnCargarJson);

            //Botón: Reproducir Siguiente
            btnReproducir = new Button { Text = "Reproducir Siguiente ▶", Location = new Point(180, 20), Size = new Size(150, 40) };
            btnReproducir.Click += BtnReproducir_Click;
            this.Controls.Add(btnReproducir);

            //Textos informativos
            lblSonando = new Label { Text = "Reproduciendo ahora: [Nada]", Location = new Point(20, 75), Size = new Size(400, 20), Font = new Font("Arial", 10, FontStyle.Bold) };
            this.Controls.Add(lblSonando);

            lblTiempoTotal = new Label { Text = "Tiempo estimado de la cola: 0 minutos", Location = new Point(20, 100), Size = new Size(300, 20) };
            this.Controls.Add(lblTiempoTotal);

            //Lista Visual (El recuadro donde se verán las canciones en espera)
            Label lblTituloLista = new Label { Text = "Próximas canciones:", Location = new Point(20, 130), Size = new Size(200, 20) };
            this.Controls.Add(lblTituloLista);
            
            listaColaUI = new ListBox { Location = new Point(20, 150), Size = new Size(310, 480) };
            this.Controls.Add(listaColaUI);

            //Visores de Imágenes para Graphviz
            Label lblGraficoCola = new Label { Text = "Gráfico de la Cola:", Location = new Point(350, 20), Size = new Size(200, 20) };
            this.Controls.Add(lblGraficoCola);

            picGraphvizCola = new PictureBox { Location = new Point(350, 45), Size = new Size(600, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            this.Controls.Add(picGraphvizCola);

            Label lblGraficoArbol = new Label { Text = "Gráfico del Árbol Binario:", Location = new Point(350, 210), Size = new Size(200, 20) };
            this.Controls.Add(lblGraficoArbol);

            picGraphvizArbol = new PictureBox { Location = new Point(350, 235), Size = new Size(600, 400), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            this.Controls.Add(picGraphvizArbol);
        }

 // --- ACCIONES DE LOS BOTONES ---
        private void BtnCargarJson_Click(object sender, EventArgs e)
        {
            OpenFileDialog ventanaArchivos = new OpenFileDialog();
            ventanaArchivos.Filter = "Archivos JSON|*.json";
            
            if (ventanaArchivos.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    miServicio.CargarDatosDesdeJson(ventanaArchivos.FileName);
                    MessageBox.Show("Canciones cargadas con éxito.");
                    ActualizarPantalla();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un error al cargar: " + ex.Message);
                }
            }
        }

        private void BtnReproducir_Click(object sender, EventArgs e)
        {
            if (miServicio.ColaReproduccion.EstaVacia())
            {
                MessageBox.Show("La cola está vacía. Carga un archivo JSON primero.");
                lblSonando.Text = "Reproduciendo ahora: [Nada]";
                return;
            }

            Cancion cancionActual = miServicio.ColaReproduccion.Desencolar();
            lblSonando.Text = $"Reproduciendo: {cancionActual.Titulo} - {cancionActual.Artista}";
            ActualizarPantalla();
        }

        // Función que refresca los textos y carga las nuevas imágenes de Graphviz
        private void ActualizarPantalla()
        {
            //Refrescar tiempo total
            lblTiempoTotal.Text = $"Tiempo estimado de la cola: {miServicio.ColaReproduccion.CalcularTiempoTotal()} minutos";

            //Llenar la lista visual de la izquierda
            listaColaUI.Items.Clear();
            Cancion[] arrayCanciones = miServicio.ColaReproduccion.ObtenerArreglo();
            for(int i = 0; i < arrayCanciones.Length; i++)
            {
                listaColaUI.Items.Add(arrayCanciones[i].ToString());
            }

            //Hacer los dibujos de Graphviz
            GraphvizManager.GenerarGraficoCola(miServicio.ColaReproduccion);
            GraphvizManager.GenerarGraficoArbol(miServicio.ArbolCanciones);

            // 4. Mostrar los dibujos en pantalla desde la carpeta Reportes
            string rutaCola = System.IO.Path.Combine("Reportes", "cola_reproduccion.png");
            if (System.IO.File.Exists(rutaCola))
            {
                using (var stream = new System.IO.FileStream(rutaCola, System.IO.FileMode.Open))
                {
                    picGraphvizCola.Image = Image.FromStream(stream);
                }
            }

            string rutaArbol = System.IO.Path.Combine("Reportes", "arbol_canciones.png");
            if (System.IO.File.Exists(rutaArbol))
            {
                using (var stream = new System.IO.FileStream(rutaArbol, System.IO.FileMode.Open))
                {
                    picGraphvizArbol.Image = Image.FromStream(stream);
                }
            }
        }
    }
}