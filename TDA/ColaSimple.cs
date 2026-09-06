using ReproductorMusica.Modelo;

namespace ReproductorMusica.Estructuras
{
    public class Cola
    {
        private NodoCola primero;
        private NodoCola ultimo;
        private int cantidad;
// Constructor
        public Cola()
        {
            primero = null;
            ultimo = null;
            cantidad = 0;
        }
// Verifica si la cola está vacía
        public bool EstaVacia()
        {
            return primero == null;
        }

// Agrega una canción al final de la cola
        public void Encolar(Cancion cancion)
        {
            NodoCola nuevoNodo = new NodoCola(cancion);
            if (EstaVacia())
            {
                primero = nuevoNodo;
                ultimo = nuevoNodo;
            }
            else
            {
                ultimo.Siguiente = nuevoNodo;
                ultimo = nuevoNodo;
            }
            cantidad++;
        }

// Saca y devuelve la primera canción de la cola (para reproducirla y eliminarla de la lista)
        public Cancion Desencolar()
        {
            if (EstaVacia()) return null;

            Cancion dato = primero.Dato;
            primero = primero.Siguiente;

            if (primero == null)
            {
                ultimo = null; // Si se vació, el último también es null
            }

            cantidad--;
            return dato;
        }
// Calcula la suma de los minutos en espera
        public int CalcularTiempoTotal()
        {
            int tiempoTotal = 0;
            NodoCola actual = primero;

            while (actual != null)
            {
                int duracion = actual.Dato.Duracion;
                
                // Si la duración es 0 o no viene, aplicamos los promedios del enunciado
                if (duracion <= 0)
                {
                    switch (actual.Dato.Genero.ToLower())
                    {
                        case "pop": duracion = 3; break;
                        case "rock": duracion = 4; break;
                        case "jazz": duracion = 5; break;
                        case "clásica":
                        case "clasica": duracion = 8; break;
                    }
                }
                
                tiempoTotal += duracion;
                actual = actual.Siguiente;
            }

            return tiempoTotal;
        }

// Convierte la cola en un arreglo primitivo para mostrarlo fácil en Windows Forms
        public Cancion[] ObtenerArreglo()
        {
            Cancion[] arreglo = new Cancion[cantidad];
            NodoCola actual = primero;
            int i = 0;
            while (actual != null)
            {
                arreglo[i] = actual.Dato;
                actual = actual.Siguiente;
                i++;
            }
            return arreglo;
        }

// Necesitaremos esto más adelante para dibujar la cola en Graphviz
        public NodoCola ObtenerPrimero()
        {
            return primero;
        }
    }
}