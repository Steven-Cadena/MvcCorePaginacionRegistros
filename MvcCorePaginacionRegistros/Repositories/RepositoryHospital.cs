﻿using Microsoft.Data.SqlClient;
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

//ALTER procedure SP_PAGINARGRUPO_DEPARTAMENTOS
//(@POSICION INT, @registros int out)
//AS
//    --UN PROCEDIMIENTO DE PAGINACION SIEMPRE
//	--DEBE DEVOLVER EL NUMERO DE REGISTROS TOTALES
//	select @registros = count(dept_no) from V_DEPT_INDIVIDUAL

//    select dept_no, dnombre, loc from V_DEPT_INDIVIDUAL
//	where posicion >= @POSICION and posicion < (@POSICION + 2)
//GO
#endregion
#region SQL-VISTA y PROCEDIMIENTO EMPLEADO/OFICIO
//create view V_EMP_INDIVIDUAL
//as
//	select * from emp
//go

//alter procedure SP_PAGINARGRUPO_EMPSALARIO
//(@POSICION INT, @OFICIO NVARCHAR(50), @REGISTROS INT OUT)
//AS
//    select @registros = COUNT(emp_no) from EMP

//    where OFICIO = @OFICIO

//    SELECT* FROM(
//    select CAST(
//    row_number() over(order by emp_no) as int)
//    as posicion, isnull(emp_no, 0) as emp_no
//	, apellido,oficio,salario,comision from EMP where oficio = @OFICIO)AS QUERY
//	where 
//	query.posicion >= @POSICION 
//	AND query.posicion <(@POSICION +3)
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
        /*variable de salida por referencia importante*/
        public List<Departamento> GetGrupoDepartamentos(int posicion, ref int numeroregistros)
        {
            /*entity framework procedimientos almacenados*/
            string sql = "SP_PAGINARGRUPO_DEPARTAMENTOS @POSICION , @registros OUT";
            SqlParameter paramposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paramregistros = new SqlParameter("@registros", -1);
            paramregistros.Direction = System.Data.ParameterDirection.Output;
            /*importante el registro*/
            var consulta = this.context.Departamentos.FromSqlRaw(sql, paramposicion,paramregistros);
            /*paso importante que debemos leer antes uqe procesar el dato de salida*/
            List<Departamento> departamentos = consulta.ToList();
            numeroregistros = (int)paramregistros.Value;
            //COMO ENVIAMOS EL NUMERO DE REGISTROS AL CONTROLLER ???
            return departamentos;
        }
        /*metodos para su paginacion de empleados por OFICIO*/
        public List<Empleado> GetEmpleadosOficio(int posicion, string oficio, ref int numeroregistros) 
        {
            string sql = "SP_PAGINARGRUPO_EMPSALARIO @POSICION, @OFICIO,@REGISTROS OUT";
            SqlParameter paramposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paramoficio = new SqlParameter("@OFICIO", oficio);
            SqlParameter paramregistros = new SqlParameter("@REGISTROS", -1);
            paramregistros.Direction = System.Data.ParameterDirection.Output;
            var consulta = this.context.Empleados.FromSqlRaw(sql, paramposicion, paramoficio, paramregistros);
            List<Empleado> empleados = consulta.ToList();
            numeroregistros = (int)paramregistros.Value;
            return empleados;
        }
    }
}
