using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Interfaces
{
    /// <summary>
    /// Basic interface for all business models classes
    /// </summary>
    public interface IBusinessModel
    {
        /// <summary>
        /// Unique identifier of business object
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name of business object
        /// </summary>
        public string Name { get; set; }
    }
}
