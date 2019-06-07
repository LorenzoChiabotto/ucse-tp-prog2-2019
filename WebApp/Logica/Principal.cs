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
        private string path = @"C:\Users\loren\Documents\UCSE\ucse-tp-prog2-2019\Archivos\";


        //Singleton
        private static Principal instance = null;
        private Principal()
        {

        }
        public static Principal Instance
        {
            get
            {
                if (instance == null){
                    instance = new Principal();
                }
                return instance;
            }
        }
        //

        private int AltaUserJson(UsuarioJson user)
        {
            List<UsuarioJson> users = GetUsersJson();
            
            user.Id = users.Count > 0 ? users.Max(x => x.Id) + 1 : 1;
            
            users.Add(user);

            string outputUsers = JsonConvert.SerializeObject(users);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputUsers);
            }

            return user.Id;
        }
        private List<UsuarioJson> GetUsersJson()
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

                return JsonConvert.DeserializeObject<List<UsuarioJson>>(conte).ToList();
            }
            catch (Exception)
            {
                return new List<UsuarioJson>();
            }
        }
        

        public Resultado AltaDirectora(Directora directora)
        {
            Resultado Controlador = new Resultado();
            int Id;

            List<UsuarioJson> users = GetUsersJson();
            UsuarioJson user = users.Where(x => x.Email == directora.Email).FirstOrDefault();
            if (user != null)
            {
                if (user.Roles.Contains(Roles.Directora))
                {
                    Controlador.Errores.Add("Directora cargada anteriormente.");
                    return Controlador;
                }
                else
                {
                    List<Roles> list = (user.Roles.ToList());
                    list.Add(Roles.Directora);
                    user.Roles = list.ToArray();
                    
                    Id = user.Id;
                    string outputUsers = JsonConvert.SerializeObject(users);
                    using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
                    {
                        strWriter.Write(outputUsers);
                    }
                }
                directora.Id = user.Id;
            }
            else
            {
                Id = AltaUserJson(new UsuarioJson() {
                    Apellido = directora.Apellido,
                    Nombre = directora.Nombre,
                    Email = directora.Email,
                    Password = new Random().Next(0, 999999).ToString("D6"),
                    Roles = new Roles[] { Roles.Directora }
                });

                directora.Id = Id;
            }


            List<DirectoraJson> listaDirectoras;

            listaDirectoras = GetDirectorasJson();

            listaDirectoras.Add(new DirectoraJson()
            {
                Id = directora.Id,
                Cargo = directora.Cargo,
                FechaIngreso = directora.FechaIngreso,
                //Institucion = directora.Institucion.Id,
            });

            string outputDirectoras = JsonConvert.SerializeObject(listaDirectoras);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Directoras.txt", false))
            {
                strWriter.Write(outputDirectoras);
            }


            return Controlador;
        }
        private List<DirectoraJson> GetDirectorasJson()
        {
            List<DirectoraJson> listaDirectoras;
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

                listaDirectoras = JsonConvert.DeserializeObject<List<DirectoraJson>>(conte).ToList();
                if (listaDirectoras == null)
                {
                    listaDirectoras = new List<DirectoraJson>();
                }
            }
            catch (Exception)
            {
                listaDirectoras = new List<DirectoraJson>();
            }

            return listaDirectoras;
        }
        public List<Directora> GetDirectoras()
        {
            List<UsuarioJson> users = GetUsersJson().Where(x=> x.Roles.Contains(Roles.Directora)).ToList();
            if (users.Count == 0)
            {
                return new List<Directora>();
            }

            List<Directora> directoras = new List<Directora>();

            List<DirectoraJson> directorasJson = GetDirectorasJson();

            foreach (UsuarioJson item in users)
            {
                directoras.Add(new Directora()
                {
                    Id = item.Id,
                    Apellido = item.Apellido,
                    Nombre = item.Nombre,
                    Email = item.Apellido,
                    Cargo = directorasJson.Where(x => x.Id == item.Id).FirstOrDefault().Cargo,
                    FechaIngreso = directorasJson.Where(x => x.Id == item.Id).FirstOrDefault().FechaIngreso,
                    /*TODO INSTITUCION CUANDO SE ARME; BUSCAR EN ARCHIVO; CREAR OBJETO Y ASIGNARLO*/
                }); 
            }

            return directoras;
        }
        
    }
}
