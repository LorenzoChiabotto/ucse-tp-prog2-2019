﻿using Contratos;
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

        /*Path.Combine(Appdomain.CurrentDomain.BaseDirectory,"Archivo.txt"); */
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
                    Email = item.Apellido,
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
        public Resultado  ModificarDocente(int id, Docente docente)
        {
            Resultado Controlador = new Resultado();

            //TODO Modificar Autousuario y Pasword

            List<UsuarioJson> listaDocentes = GetUsersJson();
            UsuarioJson usuarioDocente = listaDocentes.Where(x => x.Id == id && x.Roles.Contains(Roles.Docente)).FirstOrDefault();
            if (usuarioDocente == null)
            {
                Controlador.Errores.Add("No existe este docente.");
                return Controlador;
            }
            else
            {
                usuarioDocente.Nombre = docente.Nombre;
                usuarioDocente.Apellido = docente.Apellido;
                usuarioDocente.Email = docente.Email;
            }

            
            listaDocentes.RemoveAt(listaDocentes.IndexOf(usuarioDocente));

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
                    Email = item.Apellido,
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

            //TODO Modificar Autousuario y Pasword

            List<UsuarioJson> listaPadres = GetUsersJson();
            UsuarioJson usuarioPadre = listaPadres.Where(x => x.Id == id && x.Roles.Contains(Roles.Padre)).FirstOrDefault();
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
                    Email = item.Apellido,
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
            int Id;

            List<HijoJson> hijos = GetHijosJson();
            if (hijos.Where(x=>x.Email == hijo.Email).FirstOrDefault() != null)
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
                idInstitucion = hijo.Institucion != null? hijo.Institucion.Id:0,
                idNotas = new int[] { },
                idSala = hijo.Sala.Id,

            });

            string outputHijos = JsonConvert.SerializeObject(hijos);
            using (StreamWriter strWriter = new System.IO.StreamWriter(path + "Hijos.txt", false))
            {
                strWriter.Write(outputHijos);
            }

            return Controlador;
        }


        private List<HijoJson> GetHijosJson()
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
                    Email = item.Apellido,
                    Sala = GetSalas().Where(x=> x.Id == item.idSala).FirstOrDefault(),
                    FechaNacimiento = item.FechaNacimiento,
                    Institucion = new Institucion(),// GetInstitucion(item.id),
                    Notas = GetNotas(item.idNotas),
                    ResultadoUltimaEvaluacionAnual = item.ResultadoUltimaEvaluacionAnual,
                });
            }

            return hijos;
        }

        #endregion

        #region NOTAS

        public Nota[] GetNotas(int[] idNotas)
        {

            return null;
        }
        #endregion  
    }


    static class ExtensionMethods
    {
        public static Roles[] AddRol(this Roles[] roles, Roles rolAdd)
        {
            List<Roles> listaRol = roles.ToList();
            listaRol.Add(rolAdd);

            return listaRol.ToArray();
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
            List<UsuarioJson> users = new List<UsuarioJson>();
            UsuarioJson user;

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
                    Notas = Principal.Instance.GetNotas(hijos.Where(x => x.Id == item).FirstOrDefault().idNotas),
                    Sala = Principal.Instance.GetSalas().Where(x => x.Id == item).FirstOrDefault(),
                    ResultadoUltimaEvaluacionAnual = hijos.Where(x => x.Id == item).FirstOrDefault().ResultadoUltimaEvaluacionAnual,
                });
            }

            return retorno.ToArray();
        }
    }
}
