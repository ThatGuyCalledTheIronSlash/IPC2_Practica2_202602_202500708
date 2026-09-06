using System;

namespace ReproductorMusica.Modelo
{
    public class Cancion
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Genero { get; set; }
        public int Duracion { get; set; } // En minutos


//Atributos de Cancion
        public Cancion(string titulo, string artista, string genero, int duracion)
        {
            Titulo = titulo;
            Artista = artista;
            Genero = genero;
            Duracion = duracion;
        }

// Convertir a cadena con ToString para que sea fácil mostrarla en la Interfaz Gráfica
        public override string ToString()
        {
            return $"{Titulo} - {Artista} ({Duracion} min)";
        }
    }
}