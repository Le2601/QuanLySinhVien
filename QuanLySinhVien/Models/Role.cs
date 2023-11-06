using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{

    [Table("Role")]
    public class Role
    {

        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

    }
}
