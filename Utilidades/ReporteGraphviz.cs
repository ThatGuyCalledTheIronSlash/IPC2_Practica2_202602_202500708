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
            dot += "graph [bgcolor=\"#2D2D32\"];\n"; // Fondo oscuro
            dot += "node [shape=box, style=\"rounded,filled\", fillcolor=\"#1E1E23\", color=\"#007ACC\", fontcolor=\"white\"];\n";
            dot += "edge [color=\"white\"];\n";

            NodoCola actual = cola.ObtenerPrimero();
            if (actual == null)
            {
                dot += "\"Cola Vacía\";\n";
            }
            else
            {
                while (actual != null)
                {
                    // Usamos GetHashCode() para que cada nodo tenga un ID interno único y el "label" es lo que se ve dibujado (el título de la canción)
                    string idActual = "nodo" + actual.GetHashCode();
                    string tituloEscapado = actual.Dato.Titulo.Replace("\"", "\\\"");
                    dot += $"{idActual} [label=\"{tituloEscapado}\"];\n";

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
            dot += "graph [bgcolor=\"#2D2D32\"];\n"; // Fondo oscuro
            dot += "node [shape=ellipse, style=\"filled\", fillcolor=\"#1E1E23\", color=\"#007ACC\", fontcolor=\"white\"];\n";
            dot += "edge [color=\"white\"];\n";

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
            
            string tituloEscapado = nodo.Dato.Titulo.Replace("\"", "\\\"");
            resultado += $"{idNodo} [label=\"{tituloEscapado}\"];\n";

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
                // Asegurar que la carpeta "Reportes" exista
                string carpeta = "Reportes";
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Guardar dentro de la carpeta Reportes
                string rutaDot = Path.Combine(carpeta, nombreArchivo + ".dot");
                string rutaPng = Path.Combine(carpeta, nombreArchivo + ".png");
                
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