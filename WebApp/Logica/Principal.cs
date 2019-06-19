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
        //IQUERYABLE<Tipo> query = lista.where().AsQueryable();

        private string path = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"")}";
        
        
        //Singleton
        private static Principal instance = null;
        private Principal()
        {

        }
        public static Principal Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Principal();
                }
                return instance;
            }
        }

        public UsuarioLogueado LogearUsuario(string email, string password)
        {
            String[] strings = email.Split('/');
            UsuarioJson user;

            int rol = -1;
            if (strings.Length == 2)
            {
                user = GetUsersJson().Where(x => x.Email == strings[1]).FirstOrDefault();
                switch (strings[0])
                {
                    case "Director":
                        {
                            rol = 1;
                            break;
                        }
                    case "Docente":
                        {
                            rol = 2;
                            break;
                        }
                    case "Padre":
                        {
                            rol = 0;
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                user = GetUsersJson().Where(x => x.Email == email).FirstOrDefault();
            }

            if (user == null) return null;
            if (user.Password != password) return null;

            return new UsuarioLogueado()
            {
                Apellido = user.Apellido,
                Email = user.Email,
                Nombre = user.Nombre,
                Roles = user.Roles,
                RolSeleccionado = rol == -1? user.Roles.Max(): (Roles)rol, //TODO SELECCIONAR ROL DE ALGUNA MANERA..... ROL/email ?
            };
        }



        #region USUARIOS

        private int GuardarUserJson(UsuarioJson user)
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
        public Usuario GetUsuario(int id)
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

                UsuarioJson user = JsonConvert.DeserializeObject<List<UsuarioJson>>(conte).ToList().Where(x => x.Id == id).FirstOrDefault();
                return new Usuario()
                {
                    Id = user.Id,
                    Apellido = user.Apellido,
                    Email = user.Email,
                    Nombre = user.Nombre,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region DIRECTORAS

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
                    user.Roles = user.Roles.AddRol(Roles.Directora);

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
                Id = GuardarUserJson(new UsuarioJson()
                {
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
                IdUser = directora.Id,
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
        
        public Resultado ModificarDirectora(int id, Directora directora)
        {
            int indice = 0;
            Resultado Controlador = new Resultado();
            List<UsuarioJson> listaDirectoras = GetUsersJson();
            UsuarioJson usuarioDirector = listaDirectoras.Where(x => x.Id == id ).FirstOrDefault();

            indice = listaDirectoras.IndexOf(usuarioDirector);

            if (usuarioDirector==null)
            {
                Controlador.Errores.Add("No existe esta directora.");
                return Controlador;
            }
            else
            {
                usuarioDirector.Nombre = directora.Nombre;
                usuarioDirector.Apellido = directora.Apellido;
                usuarioDirector.Email = directora.Email;
            }
            listaDirectoras.RemoveAt(listaDirectoras.IndexOf(usuarioDirector));
            listaDirectoras.Insert(indice, usuarioDirector);

            string outputDocentes = JsonConvert.SerializeObject(listaDirectoras);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputDocentes);
            }


            List<DirectoraJson> listaDirectoras2 = GetDirectorasJson();
            DirectoraJson usuarioDirector2 = listaDirectoras2.Where(x => x.IdUser == id).FirstOrDefault();

            indice = listaDirectoras2.IndexOf(usuarioDirector2);

            if (usuarioDirector2 == null)
            {
                Controlador.Errores.Add("No existe esta directora.");
                return Controlador;
            }
            else
            {
                usuarioDirector2.Cargo = directora.Cargo;
                usuarioDirector2.FechaIngreso = directora.FechaIngreso;
            }
            listaDirectoras2.RemoveAt(listaDirectoras2.IndexOf(usuarioDirector2));
            listaDirectoras2.Insert(indice, usuarioDirector2);

            string outputDocentes2 = JsonConvert.SerializeObject(listaDirectoras2);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Directoras.txt", false))
            {
                strWriter.Write(outputDocentes2);
            }

            return Controlador;
        }
        public Resultado BajaDirectora(int id, Directora directora)
        {
            int indice = 0;
            Resultado Controlador = new Resultado();
            List<UsuarioJson> listaDirectoras = GetUsersJson();
            UsuarioJson usuarioDirector = listaDirectoras.Where(x => x.Id == id).FirstOrDefault();

            indice = listaDirectoras.IndexOf(usuarioDirector);

            if (usuarioDirector == null)
            {
                Controlador.Errores.Add("No existe esta directora.");
                return Controlador;
            }

            listaDirectoras.RemoveAt(listaDirectoras.IndexOf(usuarioDirector));
            string outputDocentes = JsonConvert.SerializeObject(listaDirectoras);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputDocentes);
            }


            List<DirectoraJson> listaDirectoras2 = GetDirectorasJson();
            DirectoraJson usuarioDirector2 = listaDirectoras2.Where(x => x.IdUser == id).FirstOrDefault();

            indice = listaDirectoras2.IndexOf(usuarioDirector2);

            if (usuarioDirector2 == null)
            {
                Controlador.Errores.Add("No existe esta directora.");
                return Controlador;
            }
            listaDirectoras2.RemoveAt(listaDirectoras2.IndexOf(usuarioDirector2));

            string outputDocentes2 = JsonConvert.SerializeObject(listaDirectoras2);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Directoras.txt", false))
            {
                strWriter.Write(outputDocentes2);
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
            List<UsuarioJson> users = GetUsersJson().Where(x => x.Roles.Contains(Roles.Directora)).ToList();
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
                    Email = item.Email,
                    Cargo = directorasJson.Where(x => x.IdUser == item.Id).FirstOrDefault().Cargo,
                    FechaIngreso = directorasJson.Where(x => x.IdUser == item.Id).FirstOrDefault().FechaIngreso,
                    /*TODO INSTITUCION CUANDO SE ARME; BUSCAR EN ARCHIVO; CREAR OBJETO Y ASIGNARLO*/
                });
            }

            return directoras;
        }

        #endregion

        #region DOCENTES

        public Resultado AltaDocente(Docente docente)
        {
            Resultado Controlador = new Resultado();
            int Id;

            List<UsuarioJson> users = GetUsersJson();
            UsuarioJson user = users.Where(x => x.Email == docente.Email).FirstOrDefault();
            if (user != null)
            {
                if (user.Roles.Contains(Roles.Docente))
                {
                    Controlador.Errores.Add("Docente cargada anteriormente.");
                    return Controlador;
                }
                else
                {
                    List<Roles> list = (user.Roles.ToList());
                    list.Add(Roles.Docente);
                    user.Roles = list.ToArray();

                    Id = user.Id;
                    string outputUsers = JsonConvert.SerializeObject(users);
                    using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
                    {
                        strWriter.Write(outputUsers);
                    }
                }
                docente.Id = user.Id;
            }
            else
            {
                Id = GuardarUserJson(new UsuarioJson()
                {
                    Apellido = docente.Apellido,
                    Nombre = docente.Nombre,
                    Email = docente.Email,
                    Password = new Random().Next(0, 999999).ToString("D6"),
                    Roles = new Roles[] { Roles.Docente }
                });

                docente.Id = Id;
            }

            List<DocenteJson> listaDocentes = GetDocentesJson();

            listaDocentes.Add(new DocenteJson()
            {
                IdUser = docente.Id,
                idSalas = docente.Salas != null? docente.Salas.Select(x => x.Id).ToArray() : new int[]{},

                //Institucion = directora.Institucion.Id,
            });

            string outputDirectoras = JsonConvert.SerializeObject(listaDocentes);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Docentes.txt", false))
            {
                strWriter.Write(outputDirectoras);
            }

            return Controlador;
        }
        public Resultado ModificarDocente(int id, Docente docente)
        {
            Resultado Controlador = new Resultado();

            int indice = 0;
            List<UsuarioJson> listaDocentes = GetUsersJson();
            UsuarioJson usuarioDocente = listaDocentes.Where(x => x.Id == id).FirstOrDefault();

            indice = listaDocentes.IndexOf(usuarioDocente);

            if (usuarioDocente == null)
            {
                Controlador.Errores.Add("No existe este docente.");
                return Controlador;
            }
            else
            {
                usuarioDocente.Id = id;
                usuarioDocente.Nombre = docente.Nombre;
                usuarioDocente.Apellido = docente.Apellido;
                usuarioDocente.Email = docente.Email;
            }
            
            listaDocentes.RemoveAt(listaDocentes.IndexOf(usuarioDocente));
            listaDocentes.Insert(indice, usuarioDocente);

            string outputDocentes = JsonConvert.SerializeObject(listaDocentes);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputDocentes);
            }

            return Controlador;
        }
        public Resultado BajaDocente(int id, Docente docente)
        {
            Resultado Controlador = new Resultado();

            //TODO Modificar Autousuario y Pasword
            int indice = 0;
            List<UsuarioJson> listaDocentes = GetUsersJson();

            UsuarioJson usuarioDocente = listaDocentes.Where(x => x.Id == id && x.Roles.Contains(Roles.Docente)).FirstOrDefault();
            if (usuarioDocente == null)
            {
                Controlador.Errores.Add("No existe este docente.");
                return Controlador;
            }
            else
                /*foreach (var item in usuarioDocente.Roles)
                {
                    if (item.ToString() == "Docente")
                    {
                        item.ToString() = null;
                        
                    }

                } */
                if (usuarioDocente.Roles == null)
                {
                    listaDocentes.RemoveAt(listaDocentes.IndexOf(usuarioDocente));

                    string outputDocentes = JsonConvert.SerializeObject(listaDocentes);
                    using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
                    {
                        strWriter.Write(outputDocentes);
                    }

                    return Controlador;
                }
                listaDocentes.Remove(usuarioDocente);
                return Controlador;
        }
        public Resultado AsignarDesasignarSala(int idSala, Docente docente, bool Asignar)
        {
            Resultado Controlador = new Resultado();
            List<DocenteJson> docentesJson = GetDocentesJson();

            if (Asignar)
            {
                if(docentesJson.Where(x=> x.IdUser == docente.Id).FirstOrDefault().idSalas.Contains(idSala))
                {
                    Controlador.Errores.Add("Ya tiene esta sala asignada");
                    return Controlador;
                }
                docentesJson.Where(x => x.IdUser == docente.Id).FirstOrDefault().idSalas = docentesJson.Where(x => x.IdUser == docente.Id).FirstOrDefault().idSalas.AddInt(idSala);
            }
            else
            {
                if (! docentesJson.Where(x => x.IdUser == docente.Id).FirstOrDefault().idSalas.Contains(idSala))
                {
                    Controlador.Errores.Add("No tiene esta sala asignada");
                    return Controlador;
                }
                docentesJson.Where(x => x.IdUser == docente.Id).FirstOrDefault().idSalas = docentesJson.Where(x => x.IdUser == docente.Id).FirstOrDefault().idSalas.RemoveInt(idSala);
            }

            string outpuDocentes = JsonConvert.SerializeObject(docentesJson);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Docentes.txt", false))
            {
                strWriter.Write(outpuDocentes);
            }

            return Controlador;
        }

        private List<DocenteJson> GetDocentesJson()
        {
            List<DocenteJson> listaDocentes;
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

                return JsonConvert.DeserializeObject<List<DocenteJson>>(conte).ToList();
                
            }
            catch (Exception)
            {
                listaDocentes = new List<DocenteJson>();
            }

            return listaDocentes;
        }
        public List<Docente> GetDocentes()
        {
            List<UsuarioJson> users = GetUsersJson().Where(x => x.Roles.Contains(Roles.Docente)).ToList();
            if (users.Count == 0)
            {
                return new List<Docente>();
            }

            List<Docente> docentes = new List<Docente>();

            List<DocenteJson> docentesJson = GetDocentesJson();


            foreach (UsuarioJson item in users)
            {
                
                docentes.Add(new Docente()
                {
                    Id = item.Id,
                    Apellido = item.Apellido,
                    Nombre = item.Nombre,
                    Email = item.Email,
                    Salas = docentesJson.Where(x => x.IdUser == item.Id).FirstOrDefault().idSalas.ToSalasArray(GetSalasJson()),
                    /*TODO INSTITUCION CUANDO SE ARME; BUSCAR EN ARCHIVO; CREAR OBJETO Y ASIGNARLO*/
                });
            }

            return docentes;
        }

        #endregion

        #region SALAS

        public void CrearSalas()
        {
            FileStream file;
            if (!File.Exists(path + "Salas.txt"))
            {

                List<Sala> salas = new List<Sala>();
                salas.Add(new Sala() { Id = 1, Nombre = "Sala 1" });
                salas.Add(new Sala() { Id = 2, Nombre = "Sala 2" });
                salas.Add(new Sala() { Id = 3, Nombre = "Sala 3" });
                salas.Add(new Sala() { Id = 4, Nombre = "Sala 4" });

                file = File.Create(path + "Salas.txt");
                file.Close();

                string outputSalas = JsonConvert.SerializeObject(salas);
                using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Salas.txt", false))
                {
                    strWriter.Write(outputSalas);
                }

            }
        }

        private List<SalaJson> GetSalasJson()
        {
            FileStream file;
            if (!File.Exists(path + "Salas.txt"))
            {
                file = File.Create(path + "Salas.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Salas.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<SalaJson>>(conte).ToList();
            }
            catch (Exception)
            {
                return new List<SalaJson>();
            }
        }
        public Sala[] GetSalas()
        {
            List<SalaJson> salasJson = GetSalasJson();
            List<Sala> salas = new List<Sala>();

            foreach (SalaJson item in salasJson)
            {
                salas.Add(new Sala()
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                });
            }

            return salas.ToArray<Sala>();
        }

        
        #endregion

        #region PADRES

        public Resultado AltaPadre(Padre padre)
        {
            Resultado Controlador = new Resultado();
            int Id;

            List<UsuarioJson> users = GetUsersJson();
            UsuarioJson user = users.Where(x => x.Email == padre.Email).FirstOrDefault();
            if (user != null)
            {
                if (user.Roles.Contains(Roles.Padre))
                {
                    Controlador.Errores.Add("Padre cargado anteriormente.");
                    return Controlador;
                }
                else
                {
                    List<Roles> list = (user.Roles.ToList());
                    list.Add(Roles.Padre);
                    user.Roles = list.ToArray();

                    Id = user.Id;
                    string outputUsers = JsonConvert.SerializeObject(users);
                    using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
                    {
                        strWriter.Write(outputUsers);
                    }
                }
                padre.Id = user.Id;
            }
            else
            {
                Id = GuardarUserJson(new UsuarioJson()
                {
                    Apellido = padre.Apellido,
                    Nombre = padre.Nombre,
                    Email = padre.Email,
                    Password = new Random().Next(0, 999999).ToString("D6"),
                    Roles = new Roles[] { Roles.Padre }
                });

                padre.Id = Id;
            }

            List<PadreJson> listaPadres = GetPadresJson();

            listaPadres.Add(new PadreJson()
            {
                IdUser = padre.Id,
                idHijos = padre.Hijos != null ? padre.Hijos.Select(x => x.Id).ToArray() : new int[] { },

            });

            string outputPadres = JsonConvert.SerializeObject(listaPadres);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Padres.txt", false))
            {
                strWriter.Write(outputPadres);
            }

            return Controlador;
        }
        public Resultado ModificarPadre(int id, Padre padre)
        {
            Resultado Controlador = new Resultado();

            int indice = 0;
            List<UsuarioJson> listaPadres = GetUsersJson();
            UsuarioJson usuarioPadre = listaPadres.Where(x => x.Id == id && x.Roles.Contains(Roles.Padre)).FirstOrDefault();

            indice = listaPadres.IndexOf(usuarioPadre);

            if (usuarioPadre == null)
            {
                Controlador.Errores.Add("No existe este padre.");
                return Controlador;
            }
            else
            {
                usuarioPadre.Nombre = padre.Nombre;
                usuarioPadre.Apellido = padre.Apellido;
                usuarioPadre.Email = padre.Email;
            }

            listaPadres.RemoveAt(listaPadres.IndexOf(usuarioPadre));
            listaPadres.Insert(indice, usuarioPadre);

            string outputPadres = JsonConvert.SerializeObject(listaPadres);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputPadres);
            }

            return Controlador;
         
        }
        public Resultado BajaPadre(int id, Padre padre)
        {
            Resultado Controlador = new Resultado();

            int indice = 0;
            List<UsuarioJson> listaPadres = GetUsersJson();
            UsuarioJson usuarioPadre = listaPadres.Where(x => x.Id == id && x.Roles.Contains(Roles.Padre)).FirstOrDefault();

            indice = listaPadres.IndexOf(usuarioPadre);

            if (usuarioPadre == null)
            {
                Controlador.Errores.Add("No existe este padre.");
                return Controlador;
            }
            else
            {
                usuarioPadre.Nombre = padre.Nombre;
                usuarioPadre.Apellido = padre.Apellido;
                usuarioPadre.Email = padre.Email;
            }

            listaPadres.RemoveAt(listaPadres.IndexOf(usuarioPadre));

            string outputPadres = JsonConvert.SerializeObject(listaPadres);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Usuarios.txt", false))
            {
                strWriter.Write(outputPadres);
            }

            return Controlador;

        }

        public Resultado AsignarDesasignarHijo(int idHijo, Padre padre, bool Asignar)
        {
            Resultado Controlador = new Resultado();
            List<PadreJson> padresJson = GetPadresJson();

            if (Asignar)
            {
                if (padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos.Contains(idHijo))
                {
                    Controlador.Errores.Add("Ya tiene este hijo asignado");
                    return Controlador;
                }
                padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos = padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos.AddInt(idHijo);
            }
            else
            {
                if (!padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos.Contains(idHijo))
                {
                    Controlador.Errores.Add("No tiene este hijo asignado");
                    return Controlador;
                }
                padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos = padresJson.Where(x => x.IdUser == padre.Id).FirstOrDefault().idHijos.RemoveInt(idHijo);
            }

            string outpuDocentes = JsonConvert.SerializeObject(padresJson);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Padres.txt", false))
            {
                strWriter.Write(outpuDocentes);
            }

            return Controlador;
        }

        private List<PadreJson> GetPadresJson()
        {
            List<PadreJson> listaPadres;
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

                return JsonConvert.DeserializeObject<List<PadreJson>>(conte).ToList();

            }
            catch (Exception)
            {
                listaPadres = new List<PadreJson>();
            }

            return listaPadres;
        }
        public List<Padre> GetPadres()
        {
            List<UsuarioJson> users = GetUsersJson().Where(x => x.Roles.Contains(Roles.Padre)).ToList();
            if (users.Count == 0)
            {
                return new List<Padre>();
            }

            List<Padre> padres = new List<Padre>();

            List<PadreJson> padresJson = GetPadresJson();


            foreach (UsuarioJson item in users)
            {

                padres.Add(new Padre()
                {
                    Id = item.Id,
                    Apellido = item.Apellido,
                    Nombre = item.Nombre,
                    Email = item.Email,
                    Hijos = padresJson.Where(x => x.IdUser == item.Id).FirstOrDefault().idHijos.ToHijosArray(GetHijosJson()),
                    /*TODO INSTITUCION CUANDO SE ARME; BUSCAR EN ARCHIVO; CREAR OBJETO Y ASIGNARLO*/
                });
            }

            return padres;
        }

        #endregion

        #region HIJOS

        public Resultado AltaHijo(Hijo hijo)
        {
            Resultado Controlador = new Resultado();

            List<HijoJson> hijos = GetHijosJson();
            if (hijos.Where(x => x.Email == hijo.Email).FirstOrDefault() != null)
            {
                Controlador.Errores.Add("Hijo cargado anteriormente.");
                return Controlador;
            }

            hijos.Add(new HijoJson()
            {
                Id = hijos.Count > 0 ? hijos.Max(x => x.Id) + 1 : 1,
                Nombre = hijo.Nombre,
                Apellido = hijo.Apellido,
                Email = hijo.Email,
                FechaNacimiento = hijo.FechaNacimiento,
                ResultadoUltimaEvaluacionAnual = hijo.ResultadoUltimaEvaluacionAnual,
                idInstitucion = hijo.Institucion != null ? hijo.Institucion.Id : 0,
                idNotasAlumno = new int[] { },
                idSala = hijo.Sala.Id,

            });

            string outputHijos = JsonConvert.SerializeObject(hijos);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Hijos.txt", false))
            {
                strWriter.Write(outputHijos);
            }

            return Controlador;
        }

        public Resultado ModificarAlumno(int id, Hijo hijo)
        {
            Resultado Controlador = new Resultado();

            int indice = 0;
            List<HijoJson> listaHijos = GetHijosJson();
            HijoJson usuarioHijo = listaHijos.Where(x => x.Id == id).FirstOrDefault();
    
            indice = listaHijos.IndexOf(usuarioHijo);

            if (usuarioHijo == null)
            {
                Controlador.Errores.Add("No existe este Alumno.");
                return Controlador;
            }
            else
            {
                usuarioHijo.Id = id;
                usuarioHijo.Nombre = hijo.Nombre;
                usuarioHijo.Apellido = hijo.Apellido;
                usuarioHijo.Email = hijo.Email;
                usuarioHijo.FechaNacimiento = hijo.FechaNacimiento;
                usuarioHijo.ResultadoUltimaEvaluacionAnual = hijo.ResultadoUltimaEvaluacionAnual;

            }

            listaHijos.RemoveAt(listaHijos.IndexOf(usuarioHijo));
            listaHijos.Insert(indice, usuarioHijo);

            string outputHijos = JsonConvert.SerializeObject(listaHijos);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Hijos.txt", false))
            {
                strWriter.Write(outputHijos);
            }


            return Controlador;
        }
        public Resultado BajaAlumno(int id, Hijo hijo)
        {
            Resultado Controlador = new Resultado();

            int indice = 0;
            List<HijoJson> listaHijosa = GetHijosJson();
            HijoJson usuarioHijo = listaHijosa.Where(x => x.Id == id).FirstOrDefault();

            indice = listaHijosa.IndexOf(usuarioHijo);

            if (usuarioHijo == null)
            {
                Controlador.Errores.Add("No existe este Alumno.");
                return Controlador;
            }

            listaHijosa.RemoveAt(listaHijosa.IndexOf(usuarioHijo));

            string outputHijos = JsonConvert.SerializeObject(listaHijosa);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Hijos.txt", false))
            {
                strWriter.Write(outputHijos);
            }
            return Controlador;
        }
        public List<HijoJson> GetHijosJson()
        {
            List<HijoJson> listaHijos;
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

                return JsonConvert.DeserializeObject<List<HijoJson>>(conte).ToList();

            }
            catch (Exception)
            {
                listaHijos = new List<HijoJson>();
            }

            return listaHijos;
        }
        public List<Hijo> GetHijos()
        {
            List<HijoJson> hijosJson = GetHijosJson();
            if (hijosJson.Count == 0)
            {
                return new List<Hijo>();
            }

            List<Hijo> hijos = new List<Hijo>();


            foreach (HijoJson item in hijosJson)
            {

                hijos.Add(new Hijo()
                {
                    Id = item.Id,
                    Apellido = item.Apellido,
                    Nombre = item.Nombre,
                    Email = item.Email,
                    Sala = GetSalas().Where(x => x.Id == item.idSala).FirstOrDefault(),
                    FechaNacimiento = item.FechaNacimiento,
                    Institucion = new Institucion(),// GetInstitucion(item.id),
                    Notas = GetNotas(item.idNotasAlumno),
                    ResultadoUltimaEvaluacionAnual = item.ResultadoUltimaEvaluacionAnual,
                });
            }

            return hijos;
        }

        #endregion

        #region NOTAS

        public Resultado AltaNota(Nota nota, List<Hijo> hijos)
        {
            Resultado Controlador = new Resultado();
            
            List<NotaJson> notas = GetNotasJson();

            List<HijoJson> hijosJson = GetHijosJson();
            HijoJson hijoJson;
            foreach (Hijo item in hijos)
            {
                int idNota = notas.Count > 0 ? notas.Max(x => x.Id) + 1 : 1;
                notas.Add(new NotaJson()
                {
                    Id = idNota,
                    Descripcion = nota.Descripcion,
                    FechaEventoAsociado = nota.FechaEventoAsociado,
                    Titulo = nota.Titulo,
                    Comentarios = new ComentarioJson[] { },
                    Leida = nota.Leida,
                });


                hijoJson = hijosJson.Where(x => x.Id == item.Id).FirstOrDefault();
                if (hijoJson.idNotasAlumno.Count() < 1)
                {
                    hijoJson.idNotasAlumno = new int[] { idNota };
                }
                else
                {
                    List<int> lista = hijoJson.idNotasAlumno.ToList();
                    lista.Add(idNota);
                    hijoJson.idNotasAlumno = lista.ToArray();
                }
            }

            string outputNotas = JsonConvert.SerializeObject(notas);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Notas.txt", false))
            {
                strWriter.Write(outputNotas);
            }
            
            string outputHijos = JsonConvert.SerializeObject(hijosJson);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Hijos.txt", false))
            {
                strWriter.Write(outputHijos);
            }
            
            return Controlador;
        }

        public Nota[] GetNotas(int[] idNotas)
        {
            List<Nota> notas = new List<Nota>();
            List<NotaJson> notasJson = GetNotasJson();

            foreach (int item in idNotas)
            {
                notas.Add(new Nota()
                {
                    Id = notasJson.Where(x => x.Id == item).FirstOrDefault().Id,
                    Comentarios = notasJson.Where(x => x.Id == item).FirstOrDefault().Comentarios.ToComentariosArray().OrderBy(x=>x.Fecha).ToArray(),
                    Descripcion = notasJson.Where(x => x.Id == item).FirstOrDefault().Descripcion,
                    FechaEventoAsociado = notasJson.Where(x => x.Id == item).FirstOrDefault().FechaEventoAsociado,
                    Leida = notasJson.Where(x => x.Id == item).FirstOrDefault().Leida,
                    Titulo = notasJson.Where(x => x.Id == item).FirstOrDefault().Titulo,
                });
            }

            return notas.ToArray();
        }

        public List<NotaJson> GetNotasJson()
        {
            List<NotaJson> listaNotas;
            FileStream file;
            if (!File.Exists(path + "Notas.txt"))
            {
                file = File.Create(path + "Notas.txt");
                file.Close();
            }

            try
            {
                string conte;
                using (StreamReader reader = new StreamReader(path + "Notas.txt"))
                {
                    conte = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<NotaJson>>(conte).ToList();

            }
            catch (Exception)
            {
                listaNotas = new List<NotaJson>();
            }

            return listaNotas;
        }

        public Resultado MarcarComoLeida(Nota nota)
        {
            Resultado Controlador = new Resultado();
            List<NotaJson> notasJson = GetNotasJson();
            notasJson.Where(x => x.Id == nota.Id).FirstOrDefault().Leida = true;


            string outputNotas = JsonConvert.SerializeObject(notasJson);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Notas.txt", false))
            {
                strWriter.Write(outputNotas);
            }

            return Controlador;
        }

        public Resultado ResponderNota(Nota nota, Comentario comentario)
        {
            ComentarioJson comentarioJson = new ComentarioJson()
            {
                Fecha = comentario.Fecha,
                idUser = GetUsersJson().Where(x=> x.Email == comentario.Usuario.Email).FirstOrDefault().Id,
                Mensaje = comentario.Mensaje,
            };

            Resultado Controlador = new Resultado();
            List<NotaJson> notasJson = GetNotasJson();
            notasJson.Where(x => x.Id == nota.Id).FirstOrDefault().Comentarios = notasJson.Where(x => x.Id == nota.Id).FirstOrDefault().Comentarios.AddComentario(comentarioJson);


            string outputNotas = JsonConvert.SerializeObject(notasJson);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Notas.txt", false))
            {
                strWriter.Write(outputNotas);
            }

            return Controlador;
        }
        #endregion
    }


    static class ExtensionMethods
    {

        public static int[] AddInt(this int[] IntArray, int value)
        {
            List<int> retorno = IntArray.ToList();
            
            retorno.Add(value);

            return retorno.ToArray();
        }
        public static int[] RemoveInt(this int[] IntArray, int value)
        {
            List<int> retorno = IntArray.ToList();

            retorno.Remove(value);

            return retorno.ToArray();
        }

        public static Roles[] AddRol(this Roles[] roles, Roles rolAdd)
        {
            List<Roles> listaRol = roles.ToList();
            listaRol.Add(rolAdd);

            return listaRol.ToArray();
        }


        public static Roles[] ToRolesArray(this int[] IntArray)
        {
            List<Roles> retorno = new List<Roles>();

            foreach (int item in IntArray)
            {
                retorno.Add((Roles)item);
            }

            return retorno.ToArray();
        }

        public static Sala[] ToSalasArray(this int[] IntArray, List<SalaJson> salas)
        {
            List<Sala> retorno = new List<Sala>();

            foreach (int item in IntArray)
            {
                retorno.Add(new Sala() {
                    Id = item,
                    Nombre = salas.Where(x => x.Id == item).FirstOrDefault().Nombre,
                });
            }

            return retorno.ToArray();
        }

        public static Hijo[] ToHijosArray(this int[] IntArray, List<HijoJson> hijos)
        {
            List<Hijo> retorno = new List<Hijo>();
            List<HijoJson> users = Principal.Instance.GetHijosJson();
            HijoJson user;

            foreach (int item in IntArray)
            {
                user = users.Where(x => x.Id == item).FirstOrDefault();

                retorno.Add(new Hijo()
                {
                    Id = item,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email,
                    FechaNacimiento = hijos.Where(x => x.Id == item).FirstOrDefault().FechaNacimiento,
                    Institucion = new Institucion(),//Principal.Instance.GetInstitucion(hijos.Where(x => x.IdUser == item).FirstOrDefault().idInstitucion),
                    Notas = Principal.Instance.GetNotas(hijos.Where(x => x.Id == item).FirstOrDefault().idNotasAlumno),
                    Sala = Principal.Instance.GetSalas().Where(x => x.Id == item).FirstOrDefault(),
                    ResultadoUltimaEvaluacionAnual = hijos.Where(x => x.Id == item).FirstOrDefault().ResultadoUltimaEvaluacionAnual,
                });
            }

            return retorno.ToArray();
        }

        public static Comentario[] ToComentariosArray(this ComentarioJson[] ComentArray)
        {
            List<Comentario> retorno = new List<Comentario>();
            if (ComentArray == null) return retorno.ToArray();
            foreach (ComentarioJson item in ComentArray)
            {
                retorno.Add(new Comentario
                {
                    Fecha = item.Fecha,
                    Mensaje = item.Mensaje,
                    Usuario = Principal.Instance.GetUsuario(item.idUser),
                });
            }

            return retorno.ToArray();
        }

        public static ComentarioJson[] AddComentario(this ComentarioJson[] ComentArray, ComentarioJson comentario)
        {
            List<ComentarioJson> retorno = ComentArray.ToList();

            retorno.Add(comentario);

            return retorno.ToArray();
        }
    }
}
