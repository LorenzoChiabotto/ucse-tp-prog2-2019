using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class NotaAlumnoJson
    {
        public int IdNota { get; set; }
        public int Id { get; set; }
        public bool Leida { get; set; }
        public ComentarioJson[] Comentarios { get; set; }
    }
}
