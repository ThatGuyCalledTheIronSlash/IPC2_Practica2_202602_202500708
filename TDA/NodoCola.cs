using ReproductorMusica.Modelo;

namespace ReproductorMusica.Estructuras
{

// Clase NodoCola para representar un nodo en la cola de canciones
    public class NodoCola
    {
        public Cancion Dato { get; set; }
        public NodoCola Siguiente { get; set; }

        public NodoCola(Cancion dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}