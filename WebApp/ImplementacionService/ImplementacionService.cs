using Contratos;
using Logica;
using System;
using System.Linq;

namespace ImplementacionService
{
    public class ImplementacionService : IServicioWeb
    {
        public Resultado AltaDirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            string Error = ErrorRol(usuarioLogueado, Roles.Directora);

            //if (usuarioLogueado.RolSeleccionado != Roles.Directora)
            //{
            //    Controlador.Errores.Add("No tiene permisos para dar de alta una Directora");
            //    return Controlador;
            //}

            if (Error != "")
            {
                Controlador.Errores.Add(Error);
            }

            if (Controlador.EsValido)
            {
                return Principal.Instance.AltaDirectora(directora);
            }

            return Controlador;
        }

        public Resultado AltaDocente(Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado Controlador = new Resultado();
            string Error = ErrorRol(usuarioLogueado, Roles.Docente);

            if (usuarioLogueado.RolSeleccionado != Roles.Directora)
            {
                Controlador.Errores.Add("No tiene permisos para dar de alta a un Docente.");
                return Controlador;
            }

            //ErrorSalaYaAsignada
            //bool Bool = false;
            //foreach (Sala Salas in Principal.Instance.GetSalas())
            //{
            //    foreach (Sala SalaDocente in docente.Salas)
            //    {
            //        if (Salas.Id == SalaDocente.Id)
            //        {
            //            if (Salas.Nombre != docente.Nombre)
            //            {
            //                Bool = true;
            //            }
            //        }
            //    }
            //}
            //if (Bool)
            //{
            //    Controlador.Errores.Add("Sala seleccionada tiene a otro Docente asignado.");
            //}

            if (Error != "")
            {
                Controlador.Errores.Add(Error);
            }

            if (Controlador.EsValido)
            {
                return Principal.Instance.AltaDocente(docente);
            }

            return Controlador;
        }

        public Resultado AltaNota(Nota nota, Sala[] salas, Hijo[] hijos, UsuarioLogueado usuarioLogueado)
        {
            //Resultado Controlador = new Resultado();

            //if (usuarioLogueado.RolSeleccionado != Roles.Directora && usuarioLogueado.RolSeleccionado != Roles.Docente)
            //{
            //    Controlador.Errores.Add("No tiene permisos para dar de alta una Nota");
            //    return Controlador;
            //}

            //if (Controlador.EsValido)
            //{
            //    return Principal.Instance.AltaNota(nota);
            //}

            //return Controlador;
            throw new NotImplementedException();
        }

        public Resultado AltaPadreMadre(Padre padre, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
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

                }
            return Controlador;
        }

        public Resultado EditarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado EliminarDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado EliminarDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado EliminarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Grilla<Hijo> ObtenerAlumnos(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            throw new NotImplementedException();
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
                /*Lista = _padres
                .Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal))
                .Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray(),
                CantidadRegistros = _padres.Count*/
            };
        }

        public Hijo[] ObtenerPersonas(UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Sala[] ObtenerSalasPorInstitucion(UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public UsuarioLogueado ObtenerUsuario(string email, string clave)
        {

            if (email != "" && clave != "")
            {
                //return Principal.Instance.LogIn(email, clave);
            }
            throw new NotImplementedException();
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado AltaAlumno(Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado EditarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Resultado EliminarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            throw new NotImplementedException();
        }

        public Directora ObtenerDirectoraPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            //TODO VER PERMISOS DE LOGUEADO
            return Principal.Instance.GetDirectoras().Where(x => x.Id == id).FirstOrDefault();
        }

        public Docente ObtenerDocentePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            throw new NotImplementedException();
        }

        public Padre ObtenerPadrePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            throw new NotImplementedException();
        }

        public Hijo ObtenerAlumnoPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            throw new NotImplementedException();
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
