using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exemplo.IPagedList.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        public int PessoaId { get; set; }


        [Required]
        public string Nome { get; set; }

        public string Profissao { get; set; }

        public string Endereco { get; set; }

        public string Email { get; set; }

    }
}