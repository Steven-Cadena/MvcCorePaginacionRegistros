using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class DepartamentosController : Controller
    {
        private RepositoryHospital repo;

        public DepartamentosController(RepositoryHospital repo) 
        {
            this.repo = repo;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        /*posicion es opcional y se usa ? */
        public IActionResult PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null) 
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistros();
            int siguiente = posicion.Value + 1;
            //DEBEMOS DE COMPROBAR SI NOS PASAMOS DEL NUMERO DE REGISTROS 
            if (siguiente > numregistros) 
            {
                //EFECTO OPTICO 
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1) 
            {
                anterior = 1;
            }
            VistaDepartamento vistaDept=this.repo.GetVistaDepartamento(posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vistaDept);
        }

        public IActionResult PaginarGrupoVistaDepartamento(int ? posicion) 
        {
            if (posicion == null) 
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistros();
            //<a href="PaginarGrupo?posicion=1"> Página 1 </a>
            //<a href="PaginarGrupo?posicion=3"> Página 2 </a>
            //<a href="PaginarGrupo?posicion=5"> Página 3 </a>
            //NECESITAMOS UN CONTADOR PARA DIBUJAR EL NUMERO DE PAGINA
            int numeroPagina = 1;
            //NECESITAMOS UN BUCLE QUE COMIENCE EN 1 
            // Y CUYO INCREMENTO SEA DE 2 EN 2
            //HASTA EL NUMERO DE REGISTROS
            string html = "<div>";
            for (int i = 1; i <= numeroregistros; i += 2) 
            {
                html += "<a href='PaginarGrupoVistaDepartamento?posicion="
                    + i + "'>Página " + numeroPagina + "</a> |";
                numeroPagina += 1;
            }
            html += "</div>";
            ViewData["LINKS"] = html;
            /*importante para pintar en la vista*/
            ViewData["NUMEROREGISTROS"] = numeroregistros;
            List<VistaDepartamento> lista = 
                this.repo.GetGrupoVistaDepartamento(posicion.Value);
            return View(lista);
        }
        /*vista creada para mostrar con procedimientos*/
        public IActionResult PaginarGrupoDepartamentos(int? posicion) 
        {
            if (posicion == null) 
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistros();
            ViewData["NUMEROREGISTROS"] = numeroregistros;
            List<Departamento> departamentos = this.repo.GetGrupoDepartamentos(posicion.Value);
            return View(departamentos);
        }
    }
}
