using System;
using System.IO;
using System.Diagnostics;
using ReproductorMusica.Estructuras;

namespace ReproductorMusica.Utilidades
{
// Clase para generar gráficos de estructuras de datos usando Graphviz
    public class GraphvizManager
    {
//Generar la imagen de la Cola (Se dibuja de izquierda a derecha)
        public static void GenerarGraficoCola(Cola cola)
        {
            string dot = "digraph Cola {\n";
            dot += "rankdir=LR;\n"; // Formato horizontal
            dot += "node [shape=box, style=filled, color=lightblue];\n";

            NodoCola actual = cola.ObtenerPrimero();
            if (actual == null)
            {
                dot += "\"Cola Vacía\";\n";
            }
            else
            {
                while (actual != null)
                {
                    // Usamos GetHashCode() para que cada nodo tenga un ID interno único
                    // y el "label" es lo que se ve dibujado (el título de la canción)
                    string idActual = "nodo" + actual.GetHashCode();
                    dot += $"{idActual} [label=\"{actual.Dato.Titulo}\"];\n";

                    if (actual.Siguiente != null)
                    {
                        string idSiguiente = "nodo" + actual.Siguiente.GetHashCode();
                        dot += $"{idActual} -> {idSiguiente};\n";
                    }
                    actual = actual.Siguiente;
                }
            }
            dot += "}";

            EjecutarDot(dot, "cola_reproduccion");
        }

//Generar la imagen del Árbol Binario (Formato vertical)
        public static void GenerarGraficoArbol(ArbolBinario arbol)
        {
            string dot = "digraph Arbol {\n";
            dot += "node [shape=ellipse, style=filled, color=lightgreen];\n";

            NodoArbol raiz = arbol.ObtenerRaiz();
            if (raiz == null)
            {
                dot += "\"Árbol Vacío\";\n";
            }
            else
            {
                dot += RecorrerArbol(raiz);
            }

            dot += "}";
            
            EjecutarDot(dot, "arbol_canciones");
        }

// Método recursivo para recorrer el árbol y dibujar sus ramas
        private static string RecorrerArbol(NodoArbol nodo)
        {
            string resultado = "";
            string idNodo = "nodo" + nodo.GetHashCode();
            
            resultado += $"{idNodo} [label=\"{nodo.Dato.Titulo}\"];\n";

            if (nodo.Izquierdo != null)
            {
                string idIzq = "nodo" + nodo.Izquierdo.GetHashCode();
                resultado += $"{idNodo} -> {idIzq} [label=\"Izq\"];\n";
                resultado += RecorrerArbol(nodo.Izquierdo);
            }
            if (nodo.Derecho != null)
            {
                string idDer = "nodo" + nodo.Derecho.GetHashCode();
                resultado += $"{idNodo} -> {idDer} [label=\"Der\"];\n";
                resultado += RecorrerArbol(nodo.Derecho);
            }

            return resultado;
        }

//Método que llama a la consola de Windows para crear el archivo PNG
        private static void EjecutarDot(string codigoDot, string nombreArchivo)
        {
            try
            {
                string rutaDot = nombreArchivo + ".dot";
                string rutaPng = nombreArchivo + ".png";
                
                File.WriteAllText(rutaDot, codigoDot);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "dot", // Comando de Graphviz
                    Arguments = $"-Tpng {rutaDot} -o {rutaPng}", // dot -Tpng archivo.dot -o imagen.png
                    UseShellExecute = false,
                    CreateNoWindow = true // Para que no parpadee una consola negra
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al generar imagen de Graphviz: " + ex.Message);
            }
        }
    }
}