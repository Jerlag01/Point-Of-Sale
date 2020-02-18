using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;

namespace Dal
{
    public class FormattedDbEntityValidationException : Exception
    {
        public FormattedDbEntityValidationException(DbEntityValidationException innerException)
            : base((string)null, (Exception)innerException)
        {
        }

        public override string Message
        {
            get
            {
                if (!(this.InnerException is DbEntityValidationException innerException))
                    return base.Message;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                foreach (DbEntityValidationResult entityValidationError in innerException.EntityValidationErrors)
                {
                    stringBuilder.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", (object)entityValidationError.Entry.Entity.GetType().FullName, (object)entityValidationError.Entry.State));
                    foreach (DbValidationError validationError in (IEnumerable<DbValidationError>)entityValidationError.ValidationErrors)
                    {
                        object empty = (object)string.Empty;
                        try
                        {
                            empty = entityValidationError.Entry.CurrentValues.GetValue<object>(validationError.PropertyName);
                        }
                        catch (ArgumentException ex)
                        {
                        }
                        stringBuilder.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"", (object)validationError.PropertyName, empty, (object)validationError.ErrorMessage));
                    }
                }
                stringBuilder.AppendLine();
                return stringBuilder.ToString();
            }
        }
    }
}