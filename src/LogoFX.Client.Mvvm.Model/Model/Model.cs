using System.Runtime.Serialization;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents model with <see cref="int"/> as identifier.
    /// </summary>
    [DataContract]
    public class Model : Model<int>, IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class
        /// </summary>
        public Model()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Model(Model other):base(other)
        {
        }
    }
}
