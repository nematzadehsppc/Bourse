using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{
    /// <summary>
    /// دسترسی به سرویسها
    /// </summary>
    public enum ServiceAccesType
    {
        /// <summary>
        /// هیچکدام
        /// </summary>
        None = 0,
        /// <summary>
        /// دسکتاپ
        /// </summary>
        Desktop = 1,
        /// <summary>
        /// پوسته وب
        /// </summary>
        Web = 2,
        /// <summary>
        /// اندروید
        /// </summary>
        Android = 4,
        /// <summary>
        /// وب سرویسها جانبی
        /// </summary>
        ThirdParty = 8
    }
}
