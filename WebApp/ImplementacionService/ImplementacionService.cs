using Contratos;
using Logica;
using System;
using System.Linq;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Resultado AsignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado DesasignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado DesasignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
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

            return Controlador;
        }

        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetHijos().ToArray<Hijo>();
        }

        public Sala[] ObtenerSalasPorInstitucion(UsuarioLogueado usuarioLogueado)
        {
            //TODO validar usuario logeado

            return Principal.Instance.GetSalas();
        }

        public UsuarioLogueado ObtenerUsuario(string email, string clave)
        {
            if (email == "" || clave == "")
                return null;

            return Principal.Instance.LogearUsuario(email, clave);
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
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
            return Controlador;
        }

        public Directora ObtenerDirectoraPorId(UsuarioLogueado usuarioLogueado, int id)
        {
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

        private string ErrorRol(UsuarioLogueado usuarioLogueado, Roles Rol)
        {
            bool Bool = false;
            foreach (Roles Lroles in usuarioLogueado.Roles)
            {
                if (Lroles == Rol)
                {
                    Bool = true;
                }
            }
            if (!Bool)
            {
                return $"El usuario no fue dado de alta con el rol de {usuarioLogueado.RolSeleccionado}.";
            }
            return "";
        }
    }
}
