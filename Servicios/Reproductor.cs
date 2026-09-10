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

    //Reiniciar estructuras para evitar duplicados al cargar de nuevo
            ColaReproduccion = new Cola();
            ArbolCanciones = new ArbolBinario();

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
                    
                    // 1. Trim a los textos y rechazo de vacíos
                    cancionActual.Titulo = cancionActual.Titulo?.Trim();
                    cancionActual.Artista = cancionActual.Artista?.Trim();
                    cancionActual.Genero = cancionActual.Genero?.Trim();

                    if (string.IsNullOrWhiteSpace(cancionActual.Titulo)) continue; // Rechazar título vacío

                    // 2. Validación de género
                    string gen = cancionActual.Genero?.ToLower() ?? "";
                    if (gen != "pop" && gen != "rock" && gen != "jazz" && gen != "clásica" && gen != "clasica") 
                    {
                        continue; // Rechazado por género fuera de tabla
                    }

                    // 3. Normalizar duración
                    if (cancionActual.Duracion < 0) continue; // Rechazar negativa explícita
                    if (cancionActual.Duracion == 0) // Si no viene (asume 0 al deserializar) usamos promedio
                    {
                        switch (gen) 
                        {
                            case "pop": cancionActual.Duracion = 3; break;
                            case "rock": cancionActual.Duracion = 4; break;
                            case "jazz": cancionActual.Duracion = 5; break;
                            case "clásica": 
                            case "clasica": cancionActual.Duracion = 8; break;
                        }
                    }

                    ColaReproduccion.Encolar(cancionActual);
                    ArbolCanciones.Insertar(cancionActual);
                }
            }
        }
    }   
}