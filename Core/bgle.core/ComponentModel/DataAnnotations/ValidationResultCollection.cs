using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace bgle.ComponentModel.DataAnnotations
{
    public class ValidationResultCollection 
    {
        public ICollection<ValidationResult> ValidationResults { get; private set; }

        public ValidationResultCollection()
        {
            this.ValidationResults = new Collection<ValidationResult>();
        }

        public ValidationResultCollection(IList<ValidationResult> validationResults)
        {
            this.ValidationResults = new Collection<ValidationResult>(validationResults);
        }

        public bool IsValid
        {
            get { return !ValidationResults.Any(); }
        }

        
    }
}