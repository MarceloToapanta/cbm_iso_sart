﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBM_SART.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class cbm_iso_sart_entities : DbContext
    {
        public cbm_iso_sart_entities()
            : base("name=cbm_iso_sart_entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<iso_cargo> iso_cargo { get; set; }
        public virtual DbSet<iso_consulta_medica> iso_consulta_medica { get; set; }
        public virtual DbSet<iso_departamento> iso_departamento { get; set; }
        public virtual DbSet<iso_empresa> iso_empresa { get; set; }
        public virtual DbSet<iso_historia_clinica> iso_historia_clinica { get; set; }
        public virtual DbSet<iso_puesto_trabajo> iso_puesto_trabajo { get; set; }
        public virtual DbSet<iso_personal> iso_personal { get; set; }
        public virtual DbSet<iso_accidente> iso_accidente { get; set; }
        public virtual DbSet<iso_avance_plan> iso_avance_plan { get; set; }
        public virtual DbSet<iso_detalle_matriz_mrl> iso_detalle_matriz_mrl { get; set; }
        public virtual DbSet<iso_detalle_matriz_ocupacional> iso_detalle_matriz_ocupacional { get; set; }
        public virtual DbSet<iso_detalle_plan> iso_detalle_plan { get; set; }
        public virtual DbSet<iso_equipo_caracteristica> iso_equipo_caracteristica { get; set; }
        public virtual DbSet<iso_equipo_proteccion> iso_equipo_proteccion { get; set; }
        public virtual DbSet<iso_incidente_trabajo> iso_incidente_trabajo { get; set; }
        public virtual DbSet<iso_matriz_mrl> iso_matriz_mrl { get; set; }
        public virtual DbSet<iso_matriz_profesiograma> iso_matriz_profesiograma { get; set; }
        public virtual DbSet<iso_matriz_riesgo_ocupacional> iso_matriz_riesgo_ocupacional { get; set; }
        public virtual DbSet<iso_plan> iso_plan { get; set; }
        public virtual DbSet<iso_riesgo_mrl> iso_riesgo_mrl { get; set; }
        public virtual DbSet<iso_riesgo_ocupacional> iso_riesgo_ocupacional { get; set; }
        public virtual DbSet<iso_tipo_riesgo_ocupacional> iso_tipo_riesgo_ocupacional { get; set; }
        public virtual DbSet<iso_tipo_consulta_m> iso_tipo_consulta_m { get; set; }
        public virtual DbSet<iso_ante_familiar_consulta_m> iso_ante_familiar_consulta_m { get; set; }
        public virtual DbSet<iso_ante_mujer_consulta_m> iso_ante_mujer_consulta_m { get; set; }
        public virtual DbSet<iso_ante_personal_consulta_m> iso_ante_personal_consulta_m { get; set; }
        public virtual DbSet<iso_antecedente_familiar> iso_antecedente_familiar { get; set; }
        public virtual DbSet<iso_antecedente_mujer> iso_antecedente_mujer { get; set; }
        public virtual DbSet<iso_antecedente_personal> iso_antecedente_personal { get; set; }
        public virtual DbSet<iso_antecedente_vacuna> iso_antecedente_vacuna { get; set; }
        public virtual DbSet<iso_exam_fisico_parametro> iso_exam_fisico_parametro { get; set; }
        public virtual DbSet<iso_exam_fp_consulta_m> iso_exam_fp_consulta_m { get; set; }
        public virtual DbSet<iso_pedido_exam_consulta_m> iso_pedido_exam_consulta_m { get; set; }
        public virtual DbSet<iso_pedido_examen> iso_pedido_examen { get; set; }
        public virtual DbSet<iso_sindrome> iso_sindrome { get; set; }
        public virtual DbSet<iso_sindrome_consulta_m> iso_sindrome_consulta_m { get; set; }
        public virtual DbSet<iso_sintoma> iso_sintoma { get; set; }
        public virtual DbSet<iso_sintoma_consulta_m> iso_sintoma_consulta_m { get; set; }
        public virtual DbSet<iso_tipo_exam_fisico> iso_tipo_exam_fisico { get; set; }
        public virtual DbSet<iso_tipo_pedido_exam> iso_tipo_pedido_exam { get; set; }
        public virtual DbSet<iso_tipo_sintoma> iso_tipo_sintoma { get; set; }
        public virtual DbSet<iso_vacuna_consulta_m> iso_vacuna_consulta_m { get; set; }
    }
}
