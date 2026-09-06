using System.IO;
using System.Text.Json;
using ReproductorMusica.Modelo;
using ReproductorMusica.Estructuras;

namespace ReproductorMusica.Servicios
{
    public class ReproductorService
    {
    // Instancias estructuras de datos (TDA)       
         public Cola ColaReproduccion { get; private set; }
        public ArbolBinario ArbolCanciones { get; private set; }

// Constructor
        public ReproductorService()
        {
            ColaReproduccion = new Cola();
            ArbolCanciones = new ArbolBinario();
        }

// Método para leer el JSON y llenar la cola y el árbol con las canciones
        public void CargarDatosDesdeJson(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("No se encontró el archivo de canciones.");
            }

    //Lee todo el texto del archivo
            string jsonString = File.ReadAllText(rutaArchivo);

   //Convierte el JSON a un arreglo de objetos 'Cancion'
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Cancion[] cancionesCargadas = JsonSerializer.Deserialize<Cancion[]>(jsonString, opciones);

            if (cancionesCargadas != null)
            {
    //Recorre el arreglo y guardamos cada canción en la Cola y en el Árbol
                for (int i = 0; i < cancionesCargadas.Length; i++)
                {
                    Cancion cancionActual = cancionesCargadas[i];
                    
                    ColaReproduccion.Encolar(cancionActual);
                    ArbolCanciones.Insertar(cancionActual);
                }
            }
        }
    }   
}