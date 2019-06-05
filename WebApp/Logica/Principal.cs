using Contratos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    class Principal
    {
        private string path = "";

        private Usuario GetUsuario(int id)
        {
            List<Directora> listaDirectoras = new List<Directora>();

            FileStream file;
            if (!File.Exists(path + "Usuarios.txt"))
            {
                file = File.Create(path + "Usuarios.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Usuarios.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<Usuario>>(conte).ToList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return new Usuario();
            }
        }
        private Usuario SaveUsuario(Usuario usuaio)
        {
            List<Directora> listaDirectoras = new List<Directora>();

            FileStream file;
            if (!File.Exists(path + "Usuarios.txt"))
            {
                file = File.Create(path + "Usuarios.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Usuarios.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<Usuario>>(conte).ToList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return new Usuario();
            }
        }

        private List<Directora> GetDirectoras()
        {
            List<Directora> listaDirectoras = new List<Directora>();

            FileStream file;
            if (!File.Exists(path + "Directoras.txt"))
            {
                file = File.Create(path + "Directoras.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Directoras.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<Directora>>(conte).ToList();
            }
            catch (Exception)
            {
                return new List<Directora>();
            }
        }

    }
}
