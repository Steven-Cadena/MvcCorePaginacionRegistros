using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#region SQL SERVER 
//alter view V_DEPT_INDIVIDUAL
//as
//	select CAST(
//	row_number() over(order by dept_no) as int)

//    as posicion, isnull(dept_no, 0) as dept_no
//	, dnombre,loc from dept
//go

//select * from V_DEPT_INDIVIDUAL 
//where posicion = 1
#endregion
#region PROCEDIMIENTOS ALMACENADOS
//create procedure SP_PAGINARGRUPO_DEPARTAMENTOS
//(@POSICION INT)
//AS
//    select dept_no, dnombre, loc from V_DEPT_INDIVIDUAL
//	where posicion >= @POSICION and posicion <(@POSICION +2)
//GO
#endregion
namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public int GetNumeroRegistros()
        {
            return this.context.VistaDepartamentos.Count();
        }
        public VistaDepartamento GetVistaDepartamento(int posicion)
        {
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion == posicion
                           select datos;
            /*firstordefault devuelva el primer campo, por si encuentra más campos iguales*/
            return consulta.FirstOrDefault();
        }

        public List<VistaDepartamento> GetGrupoVistaDepartamento(int posicion)
        {
            //select * from V_DEPT_INDIVIUAL
            //where posicion >=1 and posicion <(posicion +2)
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return consulta.ToList();
        }

        /*metodos para crear paginacion con procedimientos almacenados*/
        public List<Departamento> GetGrupoDepartamentos(int posicion)
        {
            /*entity framework procedimientos almacenados*/
            string sql = "SP_PAGINARGRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter paramposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.Departamentos.FromSqlRaw(sql, paramposicion);
            return consulta.ToList();
        }
    }
}
