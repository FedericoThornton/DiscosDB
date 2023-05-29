using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Dominio 
{
    public class Disco
    {
        [DisplayName("Título")]

        public int Id { get; set; }
        public string Titulo { get; set; }
        
        public int Canciones { get; set; }

        public string UrlImagen { get; set; }

        [DisplayName("Género")]
        public Estilo Genero { get; set; }

        [DisplayName("Edición")]
        public Formato Edicion { get; set; }
    }
}
