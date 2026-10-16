using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Carniceria.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = "Debe escribir el salario mensual...")]
        [Range(0, 5000000, ErrorMessage = "El salario debe ser un monto entre ₡0 y ₡5000000")]
        [Display(Name = "Salario mensual")]
        public decimal SalarioMensual { get; set; }

        [Required(ErrorMessage = "Debe escribir la fecha de ingreso...")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el área de trabajo...")]
        [Display(Name = "Área de trabajo")]
        public string AreaTrabajo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el puesto...")]
        [Display(Name = "Puesto")]
        public string Puesto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el turno...")]
        [Display(Name = "Turno")]
        public string Turno { get; set; } 
    }
 }