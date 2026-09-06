using System;
using ReproductorMusica.Modelo;

namespace ReproductorMusica.Estructuras
{
    public class ArbolBinario
    {
        private NodoArbol raiz;

        public ArbolBinario()
        {
            raiz = null;
        }

        public void Insertar(Cancion cancion)
        {
            raiz = InsertarRecursivo(raiz, cancion);
        }

        private NodoArbol InsertarRecursivo(NodoArbol nodo, Cancion cancion)
        {
            if (nodo == null)
            {
                return new NodoArbol(cancion);
            }

            int comparacion = string.Compare(cancion.Titulo, nodo.Dato.Titulo, StringComparison.OrdinalIgnoreCase);

            if (comparacion < 0)
            {
                nodo.Izquierdo = InsertarRecursivo(nodo.Izquierdo, cancion);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = InsertarRecursivo(nodo.Derecho, cancion);
            }
            else
            {
                nodo.Derecho = InsertarRecursivo(nodo.Derecho, cancion);
            }

            return nodo;
        }

        public Cancion Buscar(string titulo)
        {
            return BuscarRecursivo(raiz, titulo);
        }

        private Cancion BuscarRecursivo(NodoArbol nodo, string titulo)
        {
            if (nodo == null) return null; 

            int comparacion = string.Compare(titulo, nodo.Dato.Titulo, StringComparison.OrdinalIgnoreCase);

            if (comparacion == 0) return nodo.Dato; 
            
            if (comparacion < 0) 
                return BuscarRecursivo(nodo.Izquierdo, titulo);
            else 
                return BuscarRecursivo(nodo.Derecho, titulo);
        }

        public NodoArbol ObtenerRaiz()
        {
            return raiz;
        }
    }
}
