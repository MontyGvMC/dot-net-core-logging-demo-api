using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreLoggingDemoAPI.Scenario2.Models
{

    /// <summary>
    /// The model used in requests of our scenario 0.
    /// </summary>
    public class Scenario2RequestModel
    {

        /// <summary>
        /// A string field with some validation to cause bad requests.
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// A DateTime field.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Some integer value.
        /// </summary>
        public int Value { get; set; }

    }
    
}
