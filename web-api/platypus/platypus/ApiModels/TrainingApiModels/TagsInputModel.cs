using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class TagsInputModel
    {
        [Required]
        public IEnumerable<long> Id { get; set; }

        [Required]
        public IEnumerable<string> Tags { get; set; }
    }
}
