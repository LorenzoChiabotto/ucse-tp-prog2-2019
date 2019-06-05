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
    public class Principal
    {
        private string path = "";

        private Usuario GetUsuario(int id)
        {

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

                return JsonConvert.DeserializeObject<List<Usuario>>(conte).ToList().Where(x => x.Id == usuaio.Id).FirstOrDefault();
            }
            catch (Exception)
            {
                return new Usuario();
            }
        }

        public List<Directora> GetDirectoras()
        {
            List<Directora> listaDirectoras;

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

                listaDirectoras = JsonConvert.DeserializeObject<List<Directora>>(conte).ToList();
            }
            catch (Exception)
            {
                listaDirectoras = new List<Directora>();
                return listaDirectoras;
            }

            Usuario user;
            foreach (Directora item in listaDirectoras)
            {
                user = GetUsuario(item.Id);
                item.Nombre = user.Nombre;
                item.Apellido = user.Apellido;
                item.Email = user.Email;
            }


            return listaDirectoras;
        }

        private List<Docente> GetDocente()
        {
            List<Docente> listaDocente;

            FileStream file;
            if (!File.Exists(path + "Docentes.txt"))
            {
                file = File.Create(path + "Docentes.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Docentes.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                listaDocente = JsonConvert.DeserializeObject<List<Docente>>(conte).ToList();
            }
            catch (Exception)
            {
                listaDocente = new List<Docente>();
                return listaDocente;
            }

            Usuario user;
            foreach (Docente item in listaDocente)
            {
                user = GetUsuario(item.Id);
                item.Nombre = user.Nombre;
                item.Apellido = user.Apellido;
                item.Email = user.Email;
            }


            return listaDocente;
        }


        private List<Padre> GetPadre()
        {
            List<Padre> listaPadre;

            FileStream file;
            if (!File.Exists(path + "Padres.txt"))
            {
                file = File.Create(path + "Padres.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Padres.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                listaPadre = JsonConvert.DeserializeObject<List<Padre>>(conte).ToList();
            }
            catch (Exception)
            {
                listaPadre = new List<Padre>();
                return listaPadre;
            }

            Usuario user;
            foreach (Padre item in listaPadre)
            {
                user = GetUsuario(item.Id);
                item.Nombre = user.Nombre;
                item.Apellido = user.Apellido;
                item.Email = user.Email;
                item.Hijos = GetHijos(item.Id);
            }

            return listaPadre;
        }


        private Hijo[] GetHijos(int idPadre)
        {
            Hijo[] listaHijo;

            FileStream file;
            if (!File.Exists(path + "Hijos.txt"))
            {
                file = File.Create(path + "Hijos.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Hijos.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                listaHijo = JsonConvert.DeserializeObject<List<Hijo>>(conte).ToArray().Where(x=> x.Id;
            }
            catch (Exception)
            {
                listaHijo = null;
                return listaHijo;
            }

            Usuario user;
            foreach (Hijo item in listaHijo)
            {
                user = GetUsuario(item.Id);
                item.Nombre = user.Nombre;
                item.Apellido = user.Apellido;
                item.Email = user.Email;
            }

            return listaHijo;
        }

    }
}
