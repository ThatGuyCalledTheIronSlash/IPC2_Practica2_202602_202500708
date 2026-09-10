using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        
        // Elementos del buscador
        private TextBox txtBuscar;
        private Button btnBuscar;

        public Form1()
        {
            miServicio = new ReproductorService();
            ConstruirPantalla();
        }

// Método para redondear las esquinas de cualquier control usando GraphicsPath
        private void RedondearEsquinas(Control ctrl, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(ctrl.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(ctrl.Width - radio, ctrl.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, ctrl.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            ctrl.Region = new Region(path);
        }

// Método para hacer un control perfectamente circular
        private void HacerCirculo(Control ctrl)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, ctrl.Width, ctrl.Height);
            ctrl.Region = new Region(path);
        }

// Método donde definimos coordenadas, tamaños y colores de los botones
        private void ConstruirPantalla()
        {
            // 1. Definir paleta de colores (Modo oscuro + acento azul)
            Color colorFondo = Color.FromArgb(30, 30, 35);       // Fondo principal
            Color colorContenedor = Color.FromArgb(45, 45, 50);  // Fondo para visores de imágenes
            Color colorFondoLista = Color.FromArgb(15, 15, 18);  // Casi negro para las listas de texto
            Color colorAcento = Color.FromArgb(0, 122, 204);     // Azul brillante moderno
            Color colorTexto = Color.White;
            Font fuentePrincipal = new Font("Segoe UI", 10);
            Font fuenteNegrita = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fuenteIcono = new Font("Segoe UI", 18, FontStyle.Bold); // Más grande para el icono de play

            // Configuración de la Ventana Principal
            this.Text = "Reproductor de Música - Práctica 2";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = colorFondo;
            this.ForeColor = colorTexto;
            this.Font = fuentePrincipal;

            // --- COLUMNA IZQUIERDA (Controles) ---

            // Botón: Cargar Canciones
            btnCargarJson = new Button { Text = "Cargar JSON", Location = new Point(20, 20), Size = new Size(150, 40) };
            btnCargarJson.BackColor = colorContenedor; // Gris para no robar protagonismo al play
            btnCargarJson.ForeColor = Color.White;
            btnCargarJson.FlatStyle = FlatStyle.Flat;
            btnCargarJson.FlatAppearance.BorderSize = 0;
            btnCargarJson.Font = fuenteNegrita;
            RedondearEsquinas(btnCargarJson, 10);
            btnCargarJson.Click += BtnCargarJson_Click;
            this.Controls.Add(btnCargarJson);

            // Botón: Reproducir Siguiente (CÍRCULO)
            btnReproducir = new Button { Text = "▶", Location = new Point(20, 80), Size = new Size(50, 50) };
            btnReproducir.BackColor = colorAcento;
            btnReproducir.ForeColor = Color.White;
            btnReproducir.FlatStyle = FlatStyle.Flat;
            btnReproducir.FlatAppearance.BorderSize = 0;
            btnReproducir.Font = fuenteIcono;
            btnReproducir.Padding = new Padding(4, 0, 0, 0); // Empuja el triangulito un poco a la derecha para centrarlo visualmente
            HacerCirculo(btnReproducir);
            btnReproducir.Click += BtnReproducir_Click;
            this.Controls.Add(btnReproducir);

            // Textos informativos (Se mueven al lado del botón de Play)
            lblSonando = new Label { Text = "Reproduciendo ahora: [Nada]", Location = new Point(85, 85), Size = new Size(245, 20), Font = fuenteNegrita, ForeColor = colorAcento };
            this.Controls.Add(lblSonando);

            lblTiempoTotal = new Label { Text = "Tiempo estimado: 0 minutos", Location = new Point(85, 108), Size = new Size(245, 20) };
            this.Controls.Add(lblTiempoTotal);

            // Lista Visual (Más oscura)
            Label lblTituloLista = new Label { Text = "Próximas canciones:", Location = new Point(20, 150), Size = new Size(200, 20) };
            this.Controls.Add(lblTituloLista);
            
            listaColaUI = new ListBox { Location = new Point(20, 175), Size = new Size(310, 310) };
            listaColaUI.BackColor = colorFondoLista; // Aplicamos el color más oscuro
            listaColaUI.ForeColor = colorTexto;
            listaColaUI.BorderStyle = BorderStyle.None;
            this.Controls.Add(listaColaUI);

            // Buscador (Árbol)
            Label lblBuscar = new Label { Text = "Buscar en Biblioteca (Árbol):", Location = new Point(20, 510), Size = new Size(200, 20) };
            this.Controls.Add(lblBuscar);

            txtBuscar = new TextBox { Location = new Point(20, 535), Size = new Size(220, 25) };
            txtBuscar.BackColor = colorFondoLista; // Mismo color oscuro
            txtBuscar.ForeColor = colorTexto;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtBuscar);

            btnBuscar = new Button { Text = "Buscar", Location = new Point(250, 533), Size = new Size(80, 28) };
            btnBuscar.BackColor = colorAcento;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Font = fuenteNegrita;
            RedondearEsquinas(btnBuscar, 8);
            btnBuscar.Click += BtnBuscar_Click;
            this.Controls.Add(btnBuscar);

            // --- COLUMNA DERECHA (Graphviz a partir de X=360 para no traslapar) ---

            Label lblGraficoCola = new Label { Text = "Gráfico de la Cola:", Location = new Point(360, 20), Size = new Size(200, 20) };
            this.Controls.Add(lblGraficoCola);

            picGraphvizCola = new PictureBox { Location = new Point(360, 45), Size = new Size(600, 150), SizeMode = PictureBoxSizeMode.Zoom };
            picGraphvizCola.BackColor = colorContenedor;
            this.Controls.Add(picGraphvizCola);

            Label lblGraficoArbol = new Label { Text = "Gráfico del Árbol Binario:", Location = new Point(360, 210), Size = new Size(200, 20) };
            this.Controls.Add(lblGraficoArbol);

            picGraphvizArbol = new PictureBox { Location = new Point(360, 235), Size = new Size(600, 400), SizeMode = PictureBoxSizeMode.Zoom };
            picGraphvizArbol.BackColor = colorContenedor;
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

        private void BtnReproducir_Click(object? sender, EventArgs e)
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

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string textoBusqueda = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(textoBusqueda))
            {
                MessageBox.Show("Ingresa el título de una canción para buscar.");
                return;
            }

            Cancion encontrada = miServicio.ArbolCanciones.Buscar(textoBusqueda);
            
            if (encontrada != null)
            {
                MessageBox.Show($"¡Canción Encontrada en el Árbol!\n\nTítulo: {encontrada.Titulo}\nArtista: {encontrada.Artista}\nGénero: {encontrada.Genero}\nDuración: {encontrada.Duracion} min", "Búsqueda Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La canción no se encontró en la biblioteca musical.", "No encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            txtBuscar.Clear();
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