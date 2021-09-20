using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents model that wraps foreign object
    /// </summary>
    public class ObjectModel:ObjectModel<object>,IObjectModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        /// <param name="other"></param>
        public ObjectModel(ObjectModel other)
            : base(other)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        /// <param name="param"></param>
        public ObjectModel(object param):base(param)
        {}
    }
}