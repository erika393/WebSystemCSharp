﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)] //faz com que o endereço de email fique azul e chame o mailto: auto
        public string Email { get; set; }

        [Display(Name = "Birth Date")] //da um apelido para o attr
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] //0: eh padrao
        [DataType(DataType.Date)] //faz com que fique apenas dd/mm/yyyy e nao mais dd/mm/yyyy hh:mm
        public DateTime BirthDate { get; set; }

        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")] //para acrescentar duas casas decimais no produto
        public double BaseSalary { get; set; }
        public Department Department { get; set; }

        //abaixo estamos avisando ao framework que esse id do department deve existir
        //pois ai nao sera nulo
        //OBS: Department deve ser igual ao nome da classe para que consiga pegar o id
        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public  void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
