using Contratos;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace ImplementacionService
{
    public class ImplementacionService : IServicioWeb
    {
        public ImplementacionService()
        {
            Principal.Instance.CrearSalas();
        }

        public Resultado AltaDirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora)
            {
                Controlador.Errores.Add("No tiene permisos para dar de alta una Directora");
                return Controlador;
            }

            return Principal.Instance.AltaDirectora(directora);

        }

        public Resultado AltaDocente(Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            //string Error = ErrorRol(usuarioLogueado, Roles.Docente);

            if (usuarioLogueado.RolSeleccionado != Roles.Directora)
            {
                Controlador.Errores.Add("No tiene permisos para dar de alta a un Docente.");
                return Controlador;
            }

            return Principal.Instance.AltaDocente(docente);

        }

        public Resultado AltaNota(Nota nota, Sala[] salas, Hijo[] hijos, UsuarioLogueado usuarioLogueado)
        {
            if (salas == null && hijos == null) return Principal.Instance.AltaNota(nota, Principal.Instance.GetHijos());

            List<Hijo> lista = new List<Hijo>();
            List<Hijo> listaHijos = Principal.Instance.GetHijos();

            if (hijos.Count() > 0)
            {
                lista.AddRange(hijos);
            }
            else
            {
                foreach (Sala item in salas)
                {
                    lista.AddRange(listaHijos.Where(x => x.Sala.Id == item.Id).ToList());
                }
            }

            return Principal.Instance.AltaNota(nota, lista);
        }

        public Resultado AltaPadreMadre(Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para dar de alta un Padre");
                return Controlador;
            }

            return Principal.Instance.AltaPadre(padre);
        }

        public Resultado AsignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.AsignarDesasignarSala(sala.Id, docente, true);
        }

        public Resultado AsignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.AsignarDesasignarHijo(hijo.Id, padre, true);
        }

        public Resultado DesasignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.AsignarDesasignarSala(sala.Id, docente, false);
        }

        public Resultado DesasignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.AsignarDesasignarHijo(hijo.Id, padre, false);
        }

        public Resultado EditarDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            if (usuarioLogueado.Roles.Contains(Roles.Directora))
            {
                Controlador = Principal.Instance.ModificarDirectora(id, directora);
            }
            else
            {
                Controlador.Errores.Add("No tiene permisos para editar a una Directora.");
            }
            return Controlador;
        }

        public Resultado EditarDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            if (usuarioLogueado.Roles.Contains(Roles.Directora))
            {
                Controlador = Principal.Instance.ModificarDocente(id, docente);
            }
            else
            {
                Controlador.Errores.Add("No tiene permisos para editar a un Docente.");
            }
            return Controlador;
        }

        public Resultado EditarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para editar un Padre");
                return Controlador;
            }
            Controlador = Principal.Instance.ModificarPadre(id, padre);
            return Controlador;
        }

        public Resultado EliminarDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            if (usuarioLogueado.Roles.Contains(Roles.Directora))
            {
                Controlador = Principal.Instance.ModificarDirectora(id, directora);
            }
            else
            {
                Controlador.Errores.Add("No tiene permisos para eliminar a una Directora.");
            }
            return Controlador;
        }

        public Resultado EliminarDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            if (usuarioLogueado.Roles.Contains(Roles.Directora))
            {
                Controlador = Principal.Instance.ModificarDocente(id, docente);
            }
            else
            {
                Controlador.Errores.Add("No tiene permisos para eliminar a un Docente.");
            }
            return Controlador;
        }

        public Resultado EliminarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para eliminar un Padre");
                return Controlador;
            }
            Principal.Instance.BajaPadre(id, padre);
            return Controlador;
        }

        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.MarcarComoLeida(nota);
        }

        public Grilla<Hijo> ObtenerAlumnos(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            return new Grilla<Hijo>()
            {
                Lista = Principal.Instance.GetHijos()
                .Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal))
                .Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray(),
                CantidadRegistros = Principal.Instance.GetHijos().Count
            };
        }

        public Nota[] ObtenerCuadernoComunicaciones(int idPersona, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.GetHijos().Where(x => x.Id == idPersona).FirstOrDefault().Notas;
        }

        public Grilla<Directora> ObtenerDirectoras(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return new Grilla<Directora>()
            {
                Lista = Principal.Instance.GetDirectoras()
                .Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal))
                .Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray(),
                CantidadRegistros = Principal.Instance.GetDirectoras().Count
            };
        }

        public Grilla<Docente> ObtenerDocentes(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return new Grilla<Docente>()
            {
                Lista = Principal.Instance.GetDocentes()
               .Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal))
               .Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray(),
                CantidadRegistros = Principal.Instance.GetDocentes().Count
            };
        }

        public Institucion[] ObtenerInstituciones()
        {
            throw new NotImplementedException();
        }

        public string ObtenerNombreGrupo()
        {
            return $"Musso Manuel - Spahn Andres - Chiabotto Lorenzo";
        }

        public Grilla<Padre> ObtenerPadres(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            //
            return new Grilla<Padre>()
            {
                Lista = Principal.Instance.GetPadres()
                .Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal))
                .Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray(),
                CantidadRegistros = Principal.Instance.GetPadres().Count
            };
        }

        public Hijo[] ObtenerPersonas(UsuarioLogueado usuarioLogueado)
        {
            Hijo[] lhijos = new Hijo[0];
            switch (usuarioLogueado.RolSeleccionado)
            {
                case Roles.Padre:
                    Padre padre = Principal.Instance.GetPadres().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var lh in padre.Hijos)
                    {
                        lhijos = Principal.Instance.GetHijos().Where(x => x.Id == lh.Id).ToArray();
                    }
                    break;
                case Roles.Directora:
                    Directora directora = Principal.Instance.GetDirectoras().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var lh in Principal.Instance.GetHijos())
                    {
                        lhijos = Principal.Instance.GetHijos().Where(x => x.Id == lh.Id && x.Institucion.Id == directora.Institucion.Id).ToArray();
                    }
                    break;
                case Roles.Docente:
                    Docente docente = Principal.Instance.GetDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var sd in docente.Salas)
                    {
                        lhijos = Principal.Instance.GetHijos().Where(x => x.Sala.Id == sd.Id).ToArray();
                    }
                    break;
                default:
                    break;
            }
            return lhijos;
        }

        public Sala[] ObtenerSalasPorInstitucion(UsuarioLogueado usuarioLogueado)
        {
            Sala[] lsalas = new Sala[0];
            switch (usuarioLogueado.RolSeleccionado)
            {
                case Roles.Padre:
                    Padre padre = Principal.Instance.GetPadres().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var lh in padre.Hijos)
                    {
                        lsalas = Principal.Instance.GetSalas().Where(x => x.Id == lh.Sala.Id).ToArray();
                    }
                    break;
                case Roles.Directora:
                    Directora directora = Principal.Instance.GetDirectoras().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var lh in Principal.Instance.GetHijos())
                    {
                        if (directora.Institucion.Id == lh.Institucion.Id)
                        {
                            lsalas = Principal.Instance.GetSalas().Where(x => x.Id == lh.Sala.Id).ToArray();
                        }
                    }
                    break;
                case Roles.Docente:
                    Docente docente = Principal.Instance.GetDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var sd in docente.Salas)
                    {
                        lsalas = Principal.Instance.GetSalas().Where(x => x.Id == sd.Id).ToArray();
                    }
                    break;
                default:
                    lsalas = Principal.Instance.GetSalas();
                    break;
            }
            return lsalas;
        }

        public UsuarioLogueado ObtenerUsuario(string email, string clave)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(clave))
            {
                return null;
            }

            try
            {
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException)
            {
                return null;
            }

            return Principal.Instance.LogearUsuario(email, clave);
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            return Principal.Instance.ResponderNota(nota, nuevoComentario);
        }

        public Resultado AltaAlumno(Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para dar de alta un Alumno");
                return Controlador;
            }

            return Principal.Instance.AltaHijo(hijo);
        }

        public Resultado EditarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para editar un Alumno");
                return Controlador;
            }
            Controlador = Principal.Instance.ModificarAlumno(id, hijo);
            return Controlador;
        }

        public Resultado EliminarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();

            if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            {
                Controlador.Errores.Add("No tiene permisos para eliminar un Alumno");
                return Controlador;
            }

            Controlador = Principal.Instance.BajaAlumno(id, hijo);

            return Controlador;
        }

        public Directora ObtenerDirectoraPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            //switch (usuarioLogueado.RolSeleccionado)
            //{
            //    case Roles.Padre:
            //        Padre padre = Principal.Instance.GetPadres().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
            //        foreach (var lh in padre.Hijos)
            //        {
            //            lhijos = Principal.Instance.GetHijos().Where(x => x.Id == lh.Id).ToArray();
            //        }
            //        break;
            //    case Roles.Directora:
            //        Directora directora = Principal.Instance.GetDirectoras().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
            //        foreach (var lh in Principal.Instance.GetHijos())
            //        {
            //            lhijos = Principal.Instance.GetHijos().Where(x => x.Id == lh.Id && x.Institucion.Id == directora.Institucion.Id).ToArray();
            //        }
            //        break;
            //    case Roles.Docente:
            //        Docente docente = Principal.Instance.GetDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
            //        foreach (var sd in docente.Salas)
            //        {
            //            lhijos = Principal.Instance.GetHijos().Where(x => x.Sala.Id == sd.Id).ToArray();
            //        }
            //        break;
            //    default:
            //        break;
            //}
            //return lhijos;
            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetDirectoras().Where(x => x.Id == id).FirstOrDefault();
        }

        public Docente ObtenerDocentePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetDocentes().Where(x => x.Id == id).FirstOrDefault();
        }

        public Padre ObtenerPadrePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetPadres().Where(x => x.Id == id).FirstOrDefault();
        }

        public Hijo ObtenerAlumnoPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetHijos().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
