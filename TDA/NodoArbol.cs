using ReproductorMusica.Modelo;

namespace ReproductorMusica.Estructuras
{
    public class NodoArbol
    {
// Clase NodoArbol para representar un nodo en el árbol de canciones
        public Cancion Dato { get; set; }
        public NodoArbol Izquierdo { get; set; }
        public NodoArbol Derecho { get; set; }

        public NodoArbol(Cancion dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
        }
    }
}